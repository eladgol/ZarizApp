using System;
using System.Collections.Generic;
using ZarizNavigation.Controls;
using Xamarin.Forms;
using System.Threading;
namespace ZarizNavigation
{
    public partial class TakePhotoOrPickModalPage : ContentPage
    {
        private static int busy = 0;
        private static bool bCanAcceptInput = false;
        public TakePhotoOrPickModalPage()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            bCanAcceptInput = true;
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            bCanAcceptInput = false;
        }
        async void ImageClicked(object sender, EventArgs e)
        {
            if (!bCanAcceptInput)
                return;
            if (Navigation.ModalStack.Count > 0)
                await Navigation.PopModalAsync();
            MessagingCenter.Send<TakePhotoOrPickModalPage>(this, "PickPhoto");

        }
        async void PhotoClicked(object sender, EventArgs e)
        {
            if (!bCanAcceptInput)
                return;
            if (Navigation.ModalStack.Count > 0)
                await Navigation.PopModalAsync();
            MessagingCenter.Send<TakePhotoOrPickModalPage>(this, "TakePhoto");

        }
    }

}
