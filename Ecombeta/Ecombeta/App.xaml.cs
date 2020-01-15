using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Ecombeta.Services;
using Ecombeta.Views;
using Plugin.FirebasePushNotification;
using DLToolkit.Forms.Controls;
using Plugin.Toasts;

namespace Ecombeta
{
    public partial class App : Application
    {
        Ecombeta.Views.Home mPage;

     
       //I have about 10% of an idea of wtf is happening with the Firebase Notifications Syntax makes mostly sense but it's still a confusing Library for me 
        public App()
        {
            InitializeComponent();
            FlowListView.Init();
          
            mPage = new Ecombeta.Views.Home("Mica Market")
            {
              
            };

          
            MainPage = new NavigationPage(mPage);

            CrossFirebasePushNotification.Current.Subscribe("general");
            CrossFirebasePushNotification.Current.OnTokenRefresh += (s, p) =>
            {
                System.Diagnostics.Debug.WriteLine($"TOKEN REC: {p.Token}");
            };
           

            CrossFirebasePushNotification.Current.OnNotificationReceived += (s, p) =>
            {
                try
                {
                    System.Diagnostics.Debug.WriteLine("Received");
                    if (p.Data.ContainsKey("body") && p.Data.ContainsKey("title"))
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            mPage.Message = $"{p.Data["body"]}";
                            mPage.TitleMessage = $"{p.Data["title"]}";
                        });

                    }
                }
                catch (Exception ex)
                {

                }
            };
            CrossFirebasePushNotification.Current.OnNotificationOpened += (s, p) =>
            {
                //System.Diagnostics.Debug.WriteLine(p.Identifier);

                System.Diagnostics.Debug.WriteLine("Opened");
                foreach (var data in p.Data)
                {
                    System.Diagnostics.Debug.WriteLine($"{data.Key} : {data.Value}");
                }

                if (!string.IsNullOrEmpty(p.Identifier))
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {

                        mPage.Message = p.Identifier;
                    });
                }
                else if (p.Data.ContainsKey("color"))
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        mPage.Navigation.PushAsync(new ContentPage()
                        {
                            BackgroundColor = Color.FromHex($"{p.Data["color"]}")

                        });
                    });

                }
                else if (p.Data.ContainsKey("title"))
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        mPage.TitleMessage = $"{p.Data["title"]}";
                    });

                }
            };

            CrossFirebasePushNotification.Current.OnNotificationAction += (s, p) =>
            {
                System.Diagnostics.Debug.WriteLine("Action");

                if (!string.IsNullOrEmpty(p.Identifier))
                {
                    System.Diagnostics.Debug.WriteLine($"ActionId: {p.Identifier}");
                    foreach (var data in p.Data)
                    {
                        System.Diagnostics.Debug.WriteLine($"{data.Key} : {data.Value}");
                    }

                }

            };

            CrossFirebasePushNotification.Current.OnNotificationDeleted += (s, p) =>
            {
                System.Diagnostics.Debug.WriteLine("Dismissed");
            };

            MainPage = new NavigationPage(mPage);
        }

        protected override void OnStart()
        {

            mPage = new Ecombeta.Views.Home("Mica Market")
            {

            };


            MainPage = new NavigationPage(mPage);

            CrossFirebasePushNotification.Current.Subscribe("general");
            CrossFirebasePushNotification.Current.OnTokenRefresh += (s, p) =>
            {
                System.Diagnostics.Debug.WriteLine($"TOKEN REC: {p.Token}");
            };


            CrossFirebasePushNotification.Current.OnNotificationReceived += (s, p) =>
            {
                try
                {
                    System.Diagnostics.Debug.WriteLine("Received");
                    if (p.Data.ContainsKey("body") && p.Data.ContainsKey("title"))
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            mPage.Message = $"{p.Data["body"]}";
                            mPage.TitleMessage = $"{p.Data["title"]}";
                        });

                    }
                }
                catch (Exception ex)
                {

                }
            };
            CrossFirebasePushNotification.Current.OnNotificationOpened += (s, p) =>
            {
                //System.Diagnostics.Debug.WriteLine(p.Identifier);

                System.Diagnostics.Debug.WriteLine("Opened");
                foreach (var data in p.Data)
                {
                    System.Diagnostics.Debug.WriteLine($"{data.Key} : {data.Value}");
                }

                if (!string.IsNullOrEmpty(p.Identifier))
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {

                        mPage.Message = p.Identifier;
                    });
                }
                else if (p.Data.ContainsKey("color"))
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        mPage.Navigation.PushAsync(new ContentPage()
                        {
                            BackgroundColor = Color.FromHex($"{p.Data["color"]}")

                        });
                    });

                }
                else if (p.Data.ContainsKey("title"))
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        mPage.TitleMessage = $"{p.Data["title"]}";
                    });

                }
            };

            CrossFirebasePushNotification.Current.OnNotificationAction += (s, p) =>
            {
                System.Diagnostics.Debug.WriteLine("Action");

                if (!string.IsNullOrEmpty(p.Identifier))
                {
                    System.Diagnostics.Debug.WriteLine($"ActionId: {p.Identifier}");
                    foreach (var data in p.Data)
                    {
                        System.Diagnostics.Debug.WriteLine($"{data.Key} : {data.Value}");
                    }

                }

            };

            CrossFirebasePushNotification.Current.OnNotificationDeleted += (s, p) =>
            {
                System.Diagnostics.Debug.WriteLine("Dismissed");
            };

            MainPage = new NavigationPage(mPage);
        }


        protected override void OnSleep()
        {

            mPage = new Ecombeta.Views.Home("Mica Market")
            {

            };


            MainPage = new NavigationPage(mPage);

            CrossFirebasePushNotification.Current.Subscribe("general");
            CrossFirebasePushNotification.Current.OnTokenRefresh += (s, p) =>
            {
                System.Diagnostics.Debug.WriteLine($"TOKEN REC: {p.Token}");
            };


            CrossFirebasePushNotification.Current.OnNotificationReceived += (s, p) =>
            {
                try
                {
                    System.Diagnostics.Debug.WriteLine("Received");
                    if (p.Data.ContainsKey("body") && p.Data.ContainsKey("title"))
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            mPage.Message = $"{p.Data["body"]}";
                            mPage.TitleMessage = $"{p.Data["title"]}";
                        });

                    }
                }
                catch (Exception ex)
                {

                }
            };
            CrossFirebasePushNotification.Current.OnNotificationOpened += (s, p) =>
            {
                //System.Diagnostics.Debug.WriteLine(p.Identifier);

                System.Diagnostics.Debug.WriteLine("Opened");
                foreach (var data in p.Data)
                {
                    System.Diagnostics.Debug.WriteLine($"{data.Key} : {data.Value}");
                }

                if (!string.IsNullOrEmpty(p.Identifier))
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {

                        mPage.Message = p.Identifier;
                    });
                }
                else if (p.Data.ContainsKey("color"))
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        mPage.Navigation.PushAsync(new ContentPage()
                        {
                            BackgroundColor = Color.FromHex($"{p.Data["color"]}")

                        });
                    });

                }
                else if (p.Data.ContainsKey("title"))
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        mPage.TitleMessage = $"{p.Data["title"]}";
                    });

                }
            };

            CrossFirebasePushNotification.Current.OnNotificationAction += (s, p) =>
            {
                System.Diagnostics.Debug.WriteLine("Action");

                if (!string.IsNullOrEmpty(p.Identifier))
                {
                    System.Diagnostics.Debug.WriteLine($"ActionId: {p.Identifier}");
                    foreach (var data in p.Data)
                    {
                        System.Diagnostics.Debug.WriteLine($"{data.Key} : {data.Value}");
                    }

                }

            };

            CrossFirebasePushNotification.Current.OnNotificationDeleted += (s, p) =>
            {
                System.Diagnostics.Debug.WriteLine("Dismissed");
            };

            MainPage = new NavigationPage(mPage);
        }

        protected override void OnResume()
        {
            mPage = new Ecombeta.Views.Home("Mica Market")
            {

            };


            MainPage = new NavigationPage(mPage);

            CrossFirebasePushNotification.Current.Subscribe("general");
            CrossFirebasePushNotification.Current.OnTokenRefresh += (s, p) =>
            {
                System.Diagnostics.Debug.WriteLine($"TOKEN REC: {p.Token}");
            };


            CrossFirebasePushNotification.Current.OnNotificationReceived += (s, p) =>
            {
                try
                {

                    System.Diagnostics.Debug.WriteLine("Received");
                    if (p.Data.ContainsKey("body") && p.Data.ContainsKey("title"))
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            mPage.Message = $"{p.Data["body"]}";
                            mPage.TitleMessage = $"{p.Data["title"]}";
                        });

                    }
                }
                catch (Exception ex)
                {

                }
            };
            CrossFirebasePushNotification.Current.OnNotificationOpened += (s, p) =>
            {
                //System.Diagnostics.Debug.WriteLine(p.Identifier);

                System.Diagnostics.Debug.WriteLine("Opened");
                foreach (var data in p.Data)
                {
                    System.Diagnostics.Debug.WriteLine($"{data.Key} : {data.Value}");
                }

                if (!string.IsNullOrEmpty(p.Identifier))
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {

                        mPage.Message = p.Identifier;
                    });
                }
                else if (p.Data.ContainsKey("color"))
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        mPage.Navigation.PushAsync(new ContentPage()
                        {
                            BackgroundColor = Color.FromHex($"{p.Data["color"]}")

                        });
                    });

                }
                else if (p.Data.ContainsKey("title"))
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        mPage.TitleMessage = $"{p.Data["title"]}";
                    });

                }
            };

            CrossFirebasePushNotification.Current.OnNotificationAction += (s, p) =>
            {
                System.Diagnostics.Debug.WriteLine("Action");

                if (!string.IsNullOrEmpty(p.Identifier))
                {
                    System.Diagnostics.Debug.WriteLine($"ActionId: {p.Identifier}");
                    foreach (var data in p.Data)
                    {
                        System.Diagnostics.Debug.WriteLine($"{data.Key} : {data.Value}");
                    }

                }

            };

            CrossFirebasePushNotification.Current.OnNotificationDeleted += (s, p) =>
            {
                System.Diagnostics.Debug.WriteLine("Dismissed");
            };

            MainPage = new NavigationPage(mPage);

        }
    }
}
