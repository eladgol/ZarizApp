using System;

using Xamarin.Forms;

namespace ZarizNavigation
{
    public class MainPageTake2 : ContentPage
    {
        public MainPageTake2()
        {
            Content = new StackLayout
            {
                Children = {
                    new Label { Text = "Hello ContentPage" }
                }
            };
        }
    }
}

