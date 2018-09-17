using System;
using System.Collections.Generic;

using Xamarin.Forms;
namespace ZarizNavigation.Controls
{
    public partial class SwipeWorkerEmployerView : ContentView
    {
        public SwipeWorkerEmployerView()
        {
            InitializeComponent();
            nWorker.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(() => {OnLabelWorkerClicked();})});
			nEmployer.GestureRecognizers.Add(new TapGestureRecognizer{ Command = new Command(() => {OnLabelEmployerClicked();})});
            if (nSwitch.IsToggled)
            {
                MessagingCenter.Send<SwipeWorkerEmployerView>(this, "Employer");
            }
            else
            {
                MessagingCenter.Send<SwipeWorkerEmployerView>(this, "Worker");
            }
        }
		public void SetWidthOfSwitch(int width)
		{
            //var metrics = DeviceDisplay.ScreenMetrics;
            //var width = (int)(metrics.Width * .25);
            nSwitch.MinimumWidthRequest = width;
		}
        public void OnLabelWorkerClicked()
		{
			nSwitch.IsToggled = false;
            MessagingCenter.Send<SwipeWorkerEmployerView>(this, "Worker");
        }
        public void OnLabelEmployerClicked()
        {
            nSwitch.IsToggled = true;
            MessagingCenter.Send<SwipeWorkerEmployerView>(this, "Employer");
        }
        async void OnToggled(object sender, ToggledEventArgs e)
        {
            if (e.Value)
            {
                MessagingCenter.Send<SwipeWorkerEmployerView>(this, "Employer");
            }
            else
            {
                MessagingCenter.Send<SwipeWorkerEmployerView>(this, "Worker");
            }
        }


    }
}
