using System;
using Xamarin.Forms;
using ZarizNavigation.Controls;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ZarizNavigation
{
    public class FormData 
    {
        private string sURL;
        public FormData(string _sURL) 
        {
            sURL = _sURL;
        }
        private struct FieldData
        {
            public string FirstName;
            public string LastName;
        }
        FieldData fieldData;
        public static Task<FormData> CreateAsync(string sURL)
        {
            var ret = new FormData(sURL);
            return ret.InitializeAsync();
        }
        private async Task<FormData> InitializeAsync()
        {

            fieldData = await GetfieldDataAsync();
            return this;
        }
        private async Task<FieldData> GetfieldDataAsync()
        {
           
            var values = new Dictionary<string, string>()
            {
                //{"localPassword", password},
                //{"localUser", username}
            };

            Dictionary<string, string> ret = await WebInterface.Instance.MakeGetRequest(sURL, values);
            fieldData.FirstName = "";
            fieldData.LastName = "";
            try
            {
                fieldData.FirstName = ret["firstName"];
                fieldData.LastName = ret["lastName"];
            }
            catch 
            {
                return fieldData; 
            }

            return fieldData;
        }
        public string GetFirstName()
        {
            return fieldData.FirstName;
        }
        public string GetLastName()
        {
            return fieldData.LastName;
        }
    }

    public partial class MainPage : ContentPage, ISwipeCallBack, IMessageSender
    {


        private static int busy = 0;
        private FormData formData;
        public async void UseFormDataAsync(string sURL)
        {
            MessagingCenter.Send<IMessageSender>(this, "StartLoading");
            formData = await FormData.CreateAsync(sURL);
            MessagingCenter.Send<IMessageSender>(this, "FinishLoading");
            MessagingCenter.Send<IMessageSender>(this, "FormDataLoaded");
        }
        public MainPage ()
        {
            InitializeComponent ();
            MessagingCenter.Subscribe<SwipeWorkerEmployerView>(this, "Employer", (sender) => 
            {
                 
            });
            MessagingCenter.Subscribe<SwipeWorkerEmployerView>(this, "Worker", (sender) =>
            {

            });
            MessagingCenter.Subscribe<ProfileBar>(this, "ImageClicked", (sender) =>
            {
                if (1 == Interlocked.Exchange(ref busy, 1))
                    return;
                Navigation.PushModalAsync(new TakePhotoOrPickModalPage());
            });
            MessagingCenter.Subscribe<ProfileBar>(this, "ImageClickedFinished", (sender) =>
            {
                Interlocked.Exchange(ref busy, 0);
            });
            MessagingCenter.Subscribe<IMessageSender>(this, "FormDataLoaded", (sender) =>
            {
                nFirstNameEntry.Text = formData.GetFirstName();
                nLastNameEntry.Text = formData.GetLastName();
            });
            SwipeListener swipeListener = new SwipeListener(nMainLayout, this);
            UseFormDataAsync("/getFieldDetails/");
        }

        public void SendMessage(string msg)
        {
            MessagingCenter.Send<IMessageSender>(this, msg);
               
        }
        public async void onBottomSwipe(View view)
        {

        }

        public async void onLeftSwipe(View view)
        {

        }

        public async void onNothingSwiped(View view)
        {

        }
        public async void onRightSwipe(View view)
        {
            Navigation.InsertPageBefore(new OccupationSelectPage(), this);
            await Navigation.PopAsync();
        }

        public async void onTopSwipe(View view)
        {

        }

        async void OnLogoutButtonClicked(object sender, EventArgs e)
        {
            new OnLogoutButtonWrapper().OnLogoutButtonClicked(this);
        }
    }
}
