using System;
using System.Threading;
using Android.App;
using Android.OS;
using Microsoft.AppCenter.Crashes;

namespace Ecombeta.Droid
{
    [Activity(Label = "Mica Market", Theme = "@style/Splash", MainLauncher = true)]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                Thread.Sleep(200);
                base.OnCreate(savedInstanceState);
                StartActivity(typeof(MainActivity));
                Finish();
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }


            // Create your application here
        }
    }
}