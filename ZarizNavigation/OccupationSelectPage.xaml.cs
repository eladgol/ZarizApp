using System;
using System.Collections.Generic;
using ZarizNavigation.Controls;
using Xamarin.Forms;

namespace ZarizNavigation
{
    public partial class OccupationSelectPage : ContentPage, ISwipeCallBack
    {
        public OccupationSelectPage()
        {
            InitializeComponent();
            SwipeListener swipeListener = new SwipeListener(nMainLayout, this);

        }
        public async void onBottomSwipe(View view)
        {
        }
        public async void onLeftSwipe(View view)
        {
            Navigation.InsertPageBefore(new MainPage(), this);
            await Navigation.PopAsync();
        }
        public async void onNothingSwiped(View view)
        {
        }
        public async void onRightSwipe(View view)
        {
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
