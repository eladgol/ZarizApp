using System;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ZarizNavigation
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();

        }

        async void OnSignUpButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SignUpPage());
        }
        async void PerformLogin(User user)
        {
            var isValid = await Login(user.Username, user.Password);
            if (isValid["success"] == "true")
            {
                App.IsUserLoggedIn = true;
                AccountManager.SaveCredentials(user.Username, user.Password);
                Navigation.InsertPageBefore(new MainPage(), this);

                await Navigation.PopAsync();
            }
            else
            {
                if (isValid["error"] == "NoConnection")
                {
                    messageLabel.Text = "אין חיבור לשרת, אנא נסו מאוחר יותר";
                    passwordEntry.Text = string.Empty;
                }
                else if (isValid["error"] == "busy")
                {
                    messageLabel.Text = "עדיין מטפל בבקשה קודמת";
                    nActivityIndicator.IsRunning = true;
                    nActivityIndicator.IsVisible = true;
                }
                else
                {
                    messageLabel.Text = "הכניסה נכשלה";
                    passwordEntry.Text = string.Empty;
                }
            }
        }
        async void OnLoginButtonClicked(object sender, EventArgs e)
        {

            var user = new User
            {
                Username = usernameEntry.Text,
                Password = passwordEntry.Text
            };

            PerformLogin(user);

        }
        async void OnEntryUsernameFocused(object sender, EventArgs e)
        {
            //usernameLabel.IsVisible = true;
            usernameLabel.Text = "שם משתמש";
            usernameEntry.Placeholder = string.Empty;
        }
        async void OnEntryUsernameUnfocused(object sender, EventArgs e)
        {
            usernameEntry.Placeholder = "שם משתמש";
            usernameLabel.Text = string.Empty;
            //usernameLabel.IsVisible = false; 
        }
        async void OnEntryPasswordFocused(object sender, EventArgs e)
        {
            //passwordLabel.IsVisible = true;
            passwordLabel.Text = "סיסמא";
            passwordEntry.Placeholder = string.Empty;
        }
        async void OnEntryPasswordUnfocused(object sender, EventArgs e)
        {
            //passwordLabel.IsVisible = false;
            passwordLabel.Text = string.Empty;
            passwordEntry.Placeholder = "סיסמא";
        }
        async Task<Dictionary<string, string>> Login(string username, string password)
        {
            WebInterface wI = new WebInterface();   
            var values = new Dictionary<string, string>()
            {
                {"localPassword", password},
                {"localUser", username}
            };
            bool bSuccess = false;
            nActivityIndicator.IsRunning = true;
            nActivityIndicator.IsVisible = true;
            var res = await wI.MakeGetRequest("/localLogin/", values);
            nActivityIndicator.IsRunning = false;
            nActivityIndicator.IsVisible = false;
            try
            {
                bSuccess = (res["success"] == "true");
            }
            catch (Exception e)
            {
                var res2 = new Dictionary<string, string>()
                {
                    {"success", "false"},
                    {"error", e.Message}
                };
                nActivityIndicator.IsRunning = false;
                nActivityIndicator.IsVisible = false;

                return res2;
            }
            if (!bSuccess)
            {
                string sErr = "unknown";
                try
                {
                    sErr = res["error"];
                }
                catch (Exception e)
                {
                    sErr = e.Message;
                }
                var res2 = new Dictionary<string, string>()
                {
                    {"success", "false"},
                    {"error", sErr}
                };
                if (sErr != "busy")
                {
                    nActivityIndicator.IsRunning = false;
                    nActivityIndicator.IsVisible = false;
                }
                return res2;
            }
            res["error"] = "none";
            nActivityIndicator.IsRunning = false;
            nActivityIndicator.IsVisible = false;
            return res;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            User user = new User
            {
                Username = AccountManager.Username,
                Password = AccountManager.Password
            };
            if (user.Username != null)
            {
                usernameEntry.Text = user.Username;
                passwordEntry.Text = user.Password;
                PerformLogin(user);
            }
            usernameEntry.Focus();
        }
    }
}