using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using IdentityModel.Client;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Net.Http.Headers;

namespace ZarizNavigation
{
    public class WebInterface
    {
        protected string mCSRF = String.Empty;
        protected string mSessionID = String.Empty;
        public HttpClient mClient;
        public CookieContainer mCookieContainer;
        public HttpClientHandler mHandler;
        private static int busy = 0;
        Dictionary<string, string> mDictCookies;

        private static WebInterface instance;
        private WebInterface()
        {
            mClient = new HttpClient();
            mCookieContainer = new CookieContainer();
            mHandler = new HttpClientHandler();
            mHandler.CookieContainer = mCookieContainer;
            mClient.BaseAddress = new Uri(Constants.baseIP + ":" + Constants.basePort.ToString());
            mDictCookies = new Dictionary<string, string>();

        }

        public static WebInterface Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new WebInterface();
                }
                return instance;
            }
        }
        public async Task GetCSRF()
        {
            var a = new HttpClient();
            string sUrl = Constants.baseIP + ":" + Constants.basePort.ToString() + "/";
            mClient.BaseAddress = new Uri(sUrl);
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                mClient.Timeout = new TimeSpan(0, 0, 0, 30);
                response = await mClient.GetAsync(sUrl);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }
            HandleResponse(response);

            return;


        }
        public void SetAuthHeader(string user)
        {
            var authData = string.Format("{0}:{1}", user, "");
            var authHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(authData));

            mClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeaderValue);
        }
        public async Task<Dictionary<string, string>> MakeGetRequest(string sUrl, Dictionary<string, string> data)
        {
            Dictionary<string, string> responsePayload = new Dictionary<string, string>();
            if (1 == Interlocked.Exchange(ref busy, 1))
            {   
                responsePayload["success"] = "false";
                responsePayload["error"] = "busy";
                return responsePayload;
            }
            try
            {
                var baseAddress = new Uri(Constants.baseIP + ":" + Constants.basePort.ToString() + sUrl);
                //mCookieContainer.Add(baseAddress, new Cookie("csrftoken", mCSRF));
                HttpResponseMessage response;


                if (mCSRF == string.Empty)
                {
                    await GetCSRF();
                }

                data.Add("csrfmiddlewaretoken", mCSRF);
                var postData = new FormUrlEncodedContent(data);
                //postData.Headers.Add("X-CSRFToken", mCSRF);
                //mClient.BaseAddress = baseAddress;
                //mHandler.CookieContainer = mCookieContainer; 

                mClient.DefaultRequestHeaders.Add("Accept", "application/json");
                if (!mClient.DefaultRequestHeaders.Contains("Referer"))
                    mClient.DefaultRequestHeaders.Add("Referer", Constants.baseIP);

                response = await mClient.PostAsync(baseAddress, postData);

                HandleResponse(response);

                var responseContent = await response.Content.ReadAsStringAsync();
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var responseString = await response.Content.ReadAsStringAsync();

                    var dictRes = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseString);
                    dictRes.ToList().ForEach(x => responsePayload[x.Key] = x.Value); // merge dictionaries
                    addOrUpdate(responsePayload, "success", "true");
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    responsePayload["success"] = "false";
                    responsePayload["error"] = response.StatusCode.ToString();
                }
            }
            catch (Exception e)
            {
                responsePayload["success"] = "false";
                responsePayload["error"] = e.Message;
            }
            Interlocked.Exchange(ref busy, 0);
            return responsePayload;
        }

        void HandleResponse(HttpResponseMessage response)
        {
            foreach (var h in response.Headers)
            {
                if (h.Key == "set-cookie")
                {
                    foreach (var hField in h.Value)
                    {
                        var sFields = hField.Split(';');
                        var sCookieKeyValue = sFields[0].Split('=');
                        Console.Write("Adding Cookie - {0},{1}", sCookieKeyValue[0], sCookieKeyValue[1]);
                        mCookieContainer.Add(mClient.BaseAddress, new Cookie(sCookieKeyValue[0], sCookieKeyValue[1]));
                        //mClient.DefaultRequestHeaders.Add("Cookie", sFields[0] + "=" + sFields[1]);
                        addOrUpdate(mDictCookies, sCookieKeyValue[0], sCookieKeyValue[1]);
                        if (sCookieKeyValue[0] == "csrftoken")
                        {
                            mCSRF = sCookieKeyValue[1];
                        }
                    }
                    break;
                }
            }
        }
        static void addOrUpdate(Dictionary<string, string> dic, string key, string newValue)
        {
            string val;
            if (dic.TryGetValue(key, out val))
            {
                // yay, value exists!
                dic[key] = newValue;
            }
            else
            {
                // darn, lets add the value
                dic.Add(key, newValue);
            }
        }
    }

}
