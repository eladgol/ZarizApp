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
namespace ZarizNavigation
{
    public class WebInterface
    {
        protected string mCSRF = String.Empty;
        private static int busy = 0;
        public class PostDataStruct
        {
            public string localUser { get; set; }
            public string localPassword { get; set; }
            public string csrfmiddlewaretoken { get; set; }
        }

        public WebInterface()
        {

        }


        public async Task<string> GetCSRF(string sUrl)
        {

            sUrl = Constants.baseIP + ":" + Constants.basePort.ToString() + sUrl;
            string csrftoken = "";
            CookieContainer c2 = new CookieContainer();
            var baseAddress = new Uri(sUrl);
            using (var handler = new HttpClientHandler() { CookieContainer = c2 })
            using (var client2 = new HttpClient(handler) { BaseAddress = baseAddress })
            {
                try
                {
                    client2.Timeout = new TimeSpan(0, 0, 0, 30);
                    var result = await client2.GetAsync(sUrl);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return "error";
                }
                try
                {
                    var responseCookies = c2.GetCookies(baseAddress);
                    csrftoken = responseCookies["csrftoken"].Value;
                }
                catch (Exception e2)
                {
                    Console.WriteLine(e2.Message);
                }
                return csrftoken;

            }
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

            if (mCSRF == string.Empty)
            {
                string sCSRFURL = "";
                mCSRF = await GetCSRF(sCSRFURL);
                if (mCSRF == "error")
                {
                    responsePayload["success"] = "false";
                    responsePayload["error"] = "NoConnection";
                    mCSRF = string.Empty;
                    Interlocked.Exchange(ref busy, 0);
                    return responsePayload;
                }
            }
            try
            {
                var baseAddress = new Uri(Constants.baseIP + ":" + Constants.basePort.ToString() + sUrl);
                var cookieContainer = new CookieContainer();
                cookieContainer.Add(baseAddress, new Cookie("csrftoken", mCSRF));
                HttpResponseMessage response;

                var postData = new FormUrlEncodedContent(data);
                postData.Headers.Add("X-CSRFToken", mCSRF);
                using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
                using (var client = new HttpClient(handler) { BaseAddress = baseAddress, Timeout = new TimeSpan(0,0,30) })
                {

                    client.DefaultRequestHeaders.Add("Accept", "application/json");
                    response = await client.PostAsync(baseAddress, postData);
                }
                var responseContent = response.Content;
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var responseString = await response.Content.ReadAsStringAsync();

                    var dictRes = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseString);
                    dictRes.ToList().ForEach(x => responsePayload[x.Key] = x.Value); // merge dictionaries

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
    }
}