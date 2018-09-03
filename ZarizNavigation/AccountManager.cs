using System;
using Xamarin.Auth;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
namespace ZarizNavigation
{
    public static class AccountManager
    {

        public static void SaveCredentials(string userName, string password)
        {
            if (!string.IsNullOrWhiteSpace(userName) && !string.IsNullOrWhiteSpace(password))
            {
                Account account = new Account
                {
                    Username = userName
                };
                account.Properties.Add("Password", password);
                AccountStore.Create().Save(account, Constants.AppName);
            }
        }
        public static void DeleteCredentials ()
        {
            var account = AccountStore.Create ().FindAccountsForService (Constants.AppName).FirstOrDefault ();
            if (account != null)
			{
                AccountStore.Create ().Delete (account, Constants.AppName);
            }
        }

        public static string Username {
            get {            
				var account =  AccountStore.Create ().FindAccountsForService (Constants.AppName).FirstOrDefault();
                return (account != null) ? account.Username : null;
			}
        }

        public static string Password {
            get {
				var account =  AccountStore.Create ().FindAccountsForService (Constants.AppName).FirstOrDefault();
				return (account != null) ? account.Properties ["Password"] : null;         
            }
        }
    }
}
