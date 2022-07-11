using System;
using System.Collections.Generic;
using System.IO;
using Android.App;
using Android.OS;
using Android.Runtime;
using Microsoft.AppCenter.Crashes;
using Plugin.CurrentActivity;
using Plugin.FirebasePushNotification;
using Debug = System.Diagnostics.Debug;

namespace Ecombeta.Droid
{
    //You can specify additional application information in this attribute
    [Application]
    public class MainApplication : Application, Application.IActivityLifecycleCallbacks
    {
        public MainApplication(IntPtr handle, JniHandleOwnership transer)
            : base(handle, transer)
        {
        }

        public void OnActivityCreated(Activity activity, Bundle savedInstanceState)
        {
            try
            {
                CrossCurrentActivity.Current.Activity = activity;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        public void OnActivityDestroyed(Activity activity)
        {
        }

        public void OnActivityPaused(Activity activity)
        {
            try
            {
                CrossCurrentActivity.Current.Activity = activity;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        public void OnActivityResumed(Activity activity)
        {
            try
            {
                CrossCurrentActivity.Current.Activity = activity;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        public void OnActivitySaveInstanceState(Activity activity, Bundle outState)
        {
        }

        public void OnActivityStarted(Activity activity)
        {
            try
            {
                CrossCurrentActivity.Current.Activity = activity;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        public void OnActivityStopped(Activity activity)
        {
        }

        public override void OnCreate()
        {
            try
            {
                App.MakeWebRequest();
                base.OnCreate();
                RegisterActivityLifecycleCallbacks(this);

                if (App.IsConnected)
                    //Set the default notification channel for your app when running Android Oreo
                    if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
                    {
                        //Change for your default notification channel id here
                        FirebasePushNotificationManager.DefaultNotificationChannelId = "DefaultChannel";

                        //Change for your default notification channel name here
                        FirebasePushNotificationManager.DefaultNotificationChannelName = "General";
                    }
            }
            catch (IOException ex)
            {
                Crashes.TrackError(ex);
            }

            try
            {
                if (App.IsConnected)
                {
                    //If debug you should reset the token each time.
#if DEBUG
                    FirebasePushNotificationManager.Initialize(this, new[]
                    {
                        new NotificationUserCategory("message", new List<NotificationUserAction>
                        {
                            new NotificationUserAction("Reply", "Reply", NotificationActionType.Foreground),
                            new NotificationUserAction("Forward", "Forward", NotificationActionType.Foreground)
                        }),
                        new NotificationUserCategory("request", new List<NotificationUserAction>
                        {
                            new NotificationUserAction("Accept", "Accept", NotificationActionType.Default, "check"),
                            new NotificationUserAction("Reject", "Reject", NotificationActionType.Default, "cancel")
                        })
                    }, true);
#else
	            FirebasePushNotificationManager.Initialize(this,new NotificationUserCategory[]
		    {
			new NotificationUserCategory("message",new List<NotificationUserAction> {
			    new NotificationUserAction("Reply","Reply",NotificationActionType.Foreground),
			    new NotificationUserAction("Forward","Forward",NotificationActionType.Foreground)

			}),
			new NotificationUserCategory("request",new List<NotificationUserAction> {
			    new NotificationUserAction("Accept","Accept",NotificationActionType.Default,"check"),
			    new NotificationUserAction("Reject","Reject",NotificationActionType.Default,"cancel")
			})

		    },true);
#endif

                    CrossFirebasePushNotification.Current.OnNotificationReceived += (s, p) =>
                    {
                        Debug.WriteLine("NOTIFICATION RECEIVED", p.Data);
                    };
                }
            }
            catch (IOException ex)
            {
                Crashes.TrackError(ex);
            }
        }

        public override void OnTerminate()
        {
            try
            {
                base.OnTerminate();
                UnregisterActivityLifecycleCallbacks(this);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }
    }
}