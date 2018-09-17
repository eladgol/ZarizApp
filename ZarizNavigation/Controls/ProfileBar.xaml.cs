using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Plugin.Media;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System.Threading;

namespace ZarizNavigation.Controls
{
    public partial class ProfileBar : ContentView
    {
        static int bBusyTakingPhoto = 0;
        static int bBusyPickingPhoto = 0;

        bool bCMInitialized = false;
        public ProfileBar()
        {
            InitializeComponent();
            
            ImageSource imageSource = ImageSource.FromFile("no_portrait.png");
            nImage.Source = imageSource;
            nImage.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(() => { OnImageClicked(); }) });
            MessagingCenter.Subscribe<TakePhotoOrPickModalPage>(this, "TakePhoto", (sender) =>
            {
                if (0 == Interlocked.Exchange(ref bBusyTakingPhoto, 1))
                {
                    try
                    {
                        TakeCameraPhoto();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    Interlocked.Exchange(ref bBusyTakingPhoto, 0);
                }
                MessagingCenter.Send<ProfileBar>(this, "ImageClickedFinished");
            });
            MessagingCenter.Subscribe<TakePhotoOrPickModalPage>(this, "PickPhoto", (sender) =>
            {
                if (0 == Interlocked.Exchange(ref bBusyPickingPhoto, 1))
                {
                    try
                    {
                        PickPhoto();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    Interlocked.Exchange(ref bBusyPickingPhoto, 0);
                }
                MessagingCenter.Send<ProfileBar>(this, "ImageClickedFinished");
            });
        }
        public async void OnImageClicked()
        {
            MessagingCenter.Send<ProfileBar>(this, "ImageClicked");
        }

        public async void TakeCameraPhoto()
        {

            if (bCMInitialized == false)
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
                if (status != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Camera))
                    {
                        //await DisplayAlert("Camera Permission", "Allow SavR to access your camera", "OK");
                    }
                    var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Camera });
                    status = results[Permission.Camera];
                    if (status != PermissionStatus.Granted)
                    {
                        MessagingCenter.Send<ProfileBar>(this, "Error_Camera_Permissions_Not_Granted");
                        return;
                    }
                }
                await CrossMedia.Current.Initialize();

                bCMInitialized = true;
            }
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                MessagingCenter.Send<ProfileBar>(this, "Error_No Camera");
                return;
            }
            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory = "Sample",
                Name = "ProfilePic.jpg"
            });
            if (file == null)
                return;

            //await DisplayAlert("File Location", file.Path, "OK");

            nImage.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                return stream;
            });
        }

        public async void PickPhoto()
        {

            if (bCMInitialized == false)
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Photos);
                if (status != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Photos))
                    {
                        //await DisplayAlert("Camera Permission", "Allow SavR to access your camera", "OK");
                    }
                    var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Photos });
                    status = results[Permission.Camera];
                    if (status != PermissionStatus.Granted)
                    {
                        MessagingCenter.Send<ProfileBar>(this, "Error_Photo_Permissions_Not_Granted");
                        return;
                    }
                }
                await CrossMedia.Current.Initialize();

                bCMInitialized = true;
            }
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                MessagingCenter.Send<ProfileBar>(this, "Error_No Pick photo");
                return;
            }
            var file = await CrossMedia.Current.PickPhotoAsync();
            if (file == null)
                return;

            //await DisplayAlert("File Location", file.Path, "OK");

            nImage.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                return stream;
            });
        }
    }
}
