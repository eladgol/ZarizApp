using System;
using System.Linq;
using Xamarin.Forms;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;
namespace ZarizNavigation
{
	public partial class SignUpPage : ContentPage
	{
        public static readonly BindableProperty BusyProperty = BindableProperty.Create(nameof(Busy), typeof(bool), typeof(SignUpPage), default(bool), Xamarin.Forms.BindingMode.OneWayToSource);
        public bool Busy
        {
            get
            {
                return (bool)GetValue(BusyProperty);
            }

            set
            {
                SetValue(BusyProperty, value);
            }

        }
        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == BusyProperty.PropertyName)
            {
                
            }

        }
        public SignUpPage ()
		{
            Busy = false;
			InitializeComponent ();
		}

		async void OnSignUpButtonClicked (object sender, EventArgs e)
		{
			var user = new User () {
                Username = nUser.Text,
                Password = nPassword.Text,
                Email = nMail.Text
			};

            // Sign up logic goes here
            bool bSignUpSucceeded = false;
            bool bNewUser = false;

            if (AreDetailsValid(user))
            {
                nActivityIndicator.IsRunning = true;
                nActivityIndicator.IsVisible = true;
                var payload = await SignUp(user);
                if (payload["success"] == "true")
                {
                    bSignUpSucceeded = true;
                }
                else
                {

                    if (payload["error"] != "busy")
                    {
                        nActivityIndicator.IsRunning = false;
                        nActivityIndicator.IsVisible = false;
                        if (payload["error"] == "NoConnection")
                        {
                            messageLabel.Text = "אין חיבור לשרת, אנא נסו מאוחר יותר";
                        }
                        else
                        {
                            try
                            {
                                bNewUser = payload["isNewUser"] == "true";
                                if (bNewUser)
                                {
                                    messageLabel.Text = "משתמש קיים";
                                }
                                else
                                {
                                    messageLabel.Text = "הרישום נכשל";
                                }
                            }
                            catch
                            {
                                messageLabel.Text = "הרישום נכשל";
                            }
                        }
                    }
                    else
                    {
                        messageLabel.Text = "עדיין מטפל בבקשה קודמת";
                        nActivityIndicator.IsRunning = true;
                        nActivityIndicator.IsVisible = true;
                    }

                }
            }
            if (bSignUpSucceeded) {
				var rootPage = Navigation.NavigationStack.FirstOrDefault ();
				if (rootPage != null) {
					App.IsUserLoggedIn = true;
					Navigation.InsertPageBefore (new MainPage (), Navigation.NavigationStack.First ());
					await Navigation.PopToRootAsync ();
				}
			} else {

				
			}
		}

		bool AreDetailsValid (User user)
		{
			return (!string.IsNullOrWhiteSpace (user.Username)  && !string.IsNullOrWhiteSpace (user.Email) && user.Email.Contains ("@"));
		}
        async Task<Dictionary<string, string>> SignUp(User user)
        {
            WebInterface wI = new WebInterface();
            var values = new Dictionary<string, string>()
            {
                {"localPassword", user.Password},
                {"localUser", user.Username},
                {"localEmail", user.Email}
            };
            bool bSuccess = false;
            var res = await wI.MakeGetRequest("/signUp/", values);
            try
            {
                bSuccess = (res["success"] == "true");
            }
            catch (Exception e)
            {
                var res2 = new Dictionary<string, string>
                {
                    ["success"] = "false",
                    ["error"] = e.Message
                };
                return res2;
            }
            bool bNewUser = false;
            if (bSuccess)
            {
                try
                {
                    bNewUser = (res["isNewUser"] == "true");
                    res["error"] = "none";
                }
                catch (Exception e)
                {
                    var res2 = new Dictionary<string, string>
                    {
                        ["success"] = "false",
                        ["isNewUser"] = "false",
                        ["error"] = e.Message
                    };
                    return res2;
                }
            }
            try
            {
                string err = res["error"];
            }
            catch
            {
                res["error"] = "unknown";
            }
            return res;
        }
    }
}
