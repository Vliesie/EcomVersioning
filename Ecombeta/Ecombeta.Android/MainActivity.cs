using System.Globalization;
using System.IO;
using System.Net.Http;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using FFImageLoading;
using FFImageLoading.Forms.Platform;
using Microsoft.AppCenter.Crashes;
using Octane.Xamarin.Forms.VideoPlayer.Android;
using Rg.Plugins.Popup;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Application = Android.App.Application;
using CarouselViewRenderer = CarouselView.FormsPlugin.Android.CarouselViewRenderer;
using Platform = Xamarin.Essentials.Platform;


namespace Ecombeta.Droid
{
    [Activity(ScreenOrientation = ScreenOrientation.Portrait, Label = "Mica Market App", Icon = "@mipmap/icon",
        Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsAppCompatActivity
    {
        public override void OnBackPressed()
        {
            if (Popup.SendBackPressed(base.OnBackPressed))
            {
                // Do something if there are some pages in the `PopupStack`
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                Rg.Plugins.Popup.Popup.Init(this, savedInstanceState);
         
         
                    Firebase.FirebaseApp.InitializeApp(Application.Context);

                        CultureInfo myCurrency = new CultureInfo("en-ZA");
                        CultureInfo.DefaultThreadCurrentCulture = myCurrency;
                Plugin.FirebasePushNotification.FirebasePushNotificationManager.ProcessIntent(this, Intent);
                    //Set the default notification channel for your app when running Android Oreo
                    if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
                    {
                        //Change for your default notification channel id here
                        Plugin.FirebasePushNotification.FirebasePushNotificationManager.DefaultNotificationChannelId = "DefaultChannel";

                        //Change for your default notification channel name here
                        Plugin.FirebasePushNotification.FirebasePushNotificationManager.DefaultNotificationChannelName = "General";
                    }

                FFImageLoading.Forms.Platform.CachedImageRenderer.Init(enableFastRenderer : true);
                // aapt resource value: 0x7F0B0046
                ImageService.Instance.Initialize(new FFImageLoading.Config.Configuration { HttpClient = new HttpClient() });
             
                TabLayoutResource = Resource.Layout.Tabbar;
                ToolbarResource = Resource.Layout.Toolbar;
               
                base.OnCreate(savedInstanceState);
              
                Platform.Init(this, savedInstanceState);
                Forms.Init(this, savedInstanceState);
                LoadApplication(new App());
                FormsVideoPlayer.Init();
                CarouselViewRenderer.Init();
            }
            catch (IOException ex)
            {
                Crashes.TrackError(ex);
            }
        }


        public override void OnRequestPermissionsResult(int requestCode, string[] permissions,
            [GeneratedEnum] Permission[] grantResults)
        {
            try
            {
                Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

                base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            }
            catch (IOException ex)
            {
                Crashes.TrackError(ex);
            }
        }


        protected override void OnNewIntent(Intent intent)
        {
            try
            {
                base.OnNewIntent(intent);
                Plugin.FirebasePushNotification.FirebasePushNotificationManager.ProcessIntent(this, intent);
            }
            catch (IOException ex)
            {
                Crashes.TrackError(ex);
            }
        }
    }
}