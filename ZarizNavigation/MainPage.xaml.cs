using System;
using Xamarin.Forms;

namespace ZarizNavigation
{
	public partial class MainPage : ContentPage
	{
		public MainPage ()
		{
			InitializeComponent ();
		}

		async void OnLogoutButtonClicked (object sender, EventArgs e)
		{
			App.IsUserLoggedIn = false;
			Navigation.InsertPageBefore (new LoginPage (), this);
            AccountManager.DeleteCredentials();
            await Navigation.PopAsync ();
		}
	}
}
