using System;
using Xamarin.Forms;
using ZarizNavigation.Controls;
namespace ZarizNavigation
{
    public class OnLogoutButtonWrapper
    {
        public OnLogoutButtonWrapper()
        {
        }
        public async void OnLogoutButtonClicked(object sender)
        {
            App.IsUserLoggedIn = false;
            ((Page)sender).Navigation.InsertPageBefore(new LoginPage(), (Page)sender);
            AccountManager.DeleteCredentials();
            await ((Page)sender).Navigation.PopAsync();
        }
    }
}
