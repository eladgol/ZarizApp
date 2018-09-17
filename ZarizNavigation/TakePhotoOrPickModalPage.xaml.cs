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
        public TakePhotoOrPickModalPage()
        {
            InitializeComponent();
        }
        async void ImageClicked(object sender, EventArgs e)
        {
            if (Navigation.ModalStack.Count > 0)
                await Navigation.PopModalAsync();
            MessagingCenter.Send<TakePhotoOrPickModalPage>(this, "PickPhoto");

        }
        async void PhotoClicked(object sender, EventArgs e)
        {
            if (Navigation.ModalStack.Count > 0)
                await Navigation.PopModalAsync();
            MessagingCenter.Send<TakePhotoOrPickModalPage>(this, "TakePhoto");

        }
    }

}
