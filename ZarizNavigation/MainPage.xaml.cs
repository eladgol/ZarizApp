using System;
using Xamarin.Forms;
using ZarizNavigation.Controls;
using System.Threading;
namespace ZarizNavigation
{
	public partial class MainPage : ContentPage, ISwipeCallBack
	{
        private static int busy = 0;
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
            SwipeListener swipeListener = new SwipeListener(nMainLayout, this);
            
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
