using System;
using System.Collections.Generic;
using FFImageLoading.Forms.Platform;
using Firebase.CloudMessaging;
using Foundation;
using Octane.Xamarin.Forms.VideoPlayer.iOS;
using Plugin.FirebasePushNotification;
using UIKit;
using UserNotifications;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using CarouselViewRenderer = CarouselView.FormsPlugin.iOS.CarouselViewRenderer;

namespace Ecombeta.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public class AppDelegate : FormsApplicationDelegate, IUNUserNotificationCenterDelegate, IMessagingDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override void OnActivated(UIApplication uiApplication) { UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0; base.OnActivated(uiApplication); }
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Forms.SetFlags("IndicatorView_Experimental");
            Forms.SetFlags("CarouselView_Experimental");
            Forms.SetFlags("SwipeView_Experimental");
            CachedImageRenderer.Init();
            Rg.Plugins.Popup.Popup.Init();
            Forms.Init();

            CarouselViewRenderer.Init();

            LoadApplication(new App());
            if (App.IsConnected)
            {
                Firebase.Core.App.Configure();
                FirebasePushNotificationManager.Initialize(options);
                FormsVideoPlayer.Init();

                FirebasePushNotificationManager.Initialize(options, new[]
                {
                    new NotificationUserCategory("message", new List<NotificationUserAction>
                    {
                        new NotificationUserAction("Reply", "Reply", NotificationActionType.Foreground)
                    }),
                    new NotificationUserCategory("request", new List<NotificationUserAction>
                    {
                        new NotificationUserAction("Accept", "Accept"),
                        new NotificationUserAction("Reject", "Reject", NotificationActionType.Destructive)
                    })
                });

                CrossFirebasePushNotification.Current.OnNotificationReceived += (s, p) =>
                {
                    FirebasePushNotificationManager.CurrentNotificationPresentationOption =
                        UNNotificationPresentationOptions.Alert | UNNotificationPresentationOptions.Badge;
                };
            }

            return base.FinishedLaunching(app, options);
        }


        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            FirebasePushNotificationManager.DidRegisterRemoteNotifications(deviceToken);
        }

        public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        {
            FirebasePushNotificationManager.RemoteNotificationRegistrationFailed(error);
        }

        // To receive notifications in foregroung on iOS 9 and below.
        // To receive notifications in background in any iOS version
        public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo,
            Action<UIBackgroundFetchResult> completionHandler)
        {
            // If you are receiving a notification message while your app is in the background,
            // this callback will not be fired 'till the user taps on the notification launching the application.
            LoadApplication(new App());
            // If you disable method swizzling, you'll need to call this method. 
            // This lets FCM track message delivery and analytics, which is performed
            // automatically with method swizzling enabled.
            FirebasePushNotificationManager.DidReceiveMessage(userInfo);
            // Do your magic to handle the notification data
            Console.WriteLine(userInfo);

            completionHandler(UIBackgroundFetchResult.NewData);
        }
    }
}