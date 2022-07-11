using System;
using System.IO;
using Microsoft.AppCenter.Crashes;
using UIKit;

namespace Ecombeta.iOS
{
    public class Application
    {
        // This is the main entry point of the application.
        private static void Main(string[] args)
        {
            // if you want to use a different Application Delegate class from "AppDelegate"
            // you can specify it here.
            try
            {
                UIApplication.Main(args, null, typeof(AppDelegate));


            }
            catch (IOException ex)
            {
                Crashes.TrackError(ex);
            }
          
        }
    }
}