using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DLToolkit.Forms.Controls;
using Ecombeta.Models;
using Ecombeta.Services;
using Ecombeta.Views;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Plugin.FirebasePushNotification;
using Firebase.Database;
using Firebase.Database.Query;
using WooCommerceNET;
using WooCommerceNET.WooCommerce.v3;
using Xamarin.Essentials;
using Xamarin.Forms;
using Device = Xamarin.Forms.Device;

namespace Ecombeta
{
    public partial class App : Application
    {
        public static WCObject WooObject;
        public static bool justloggedin { get; set; } 
        private Home mPage;
        public FirebaseClient firebase = new FirebaseClient("https://mica-marketapp.firebaseio.com/");
        //I have about 10% of an idea of wtf is happening with the Firebase Notifications Syntax makes mostly sense but it's still a confusing Library for me 
        public App()
        {
            InitializeComponent();
            MakeWebRequest();
            justloggedin = true;
            try
            {
                if (IsConnected)
                {
                   
                    FlowListView.Init();

                    mPage = new Home();

                   MainPage = new NavigationPage(mPage);


                   CrossFirebasePushNotification.Current.Subscribe("general");
                    CrossFirebasePushNotification.Current.OnTokenRefresh += (s, p) =>
                    {
                         Debug.WriteLine($"TOKEN REC: {p.Token}");
                    };

                 

                    CrossFirebasePushNotification.Current.OnNotificationOpened += (s, p) =>
                    {
                        //System.Diagnostics.Debug.WriteLine(p.Identifier);
                        Debug.WriteLine("Opened");
                        if (p.Data.ContainsKey("body") && p.Data.ContainsKey("title"))
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                HomeVariables.Init.Message = $"{p.Data["body"]}";
                                HomeVariables.Init.TitleMessage = $"{p.Data["title"]}";
                             
                            });
                        HomeVariables.Init.RaisePropertyChanged();


                    };



                    CrossFirebasePushNotification.Current.OnNotificationReceived += (s, p) =>
                    {
                        try
                        {
                            Debug.WriteLine("Received");
                            if (p.Data.ContainsKey("body") && p.Data.ContainsKey("title"))
                                Device.BeginInvokeOnMainThread(() =>
                                {
                                    AddNotification($"{p.Data["title"]}",$"{p.Data["body"]}",$"{p.Data["Id"]}", $"{p.Data["Image"]}");
                                    HomeVariables.Init.Message = $"{p.Data["body"]}";
                                    HomeVariables.Init.TitleMessage = $"{p.Data["title"]}";
                                });
                            
                        }
                        catch (IOException ex)
                        {
                            Crashes.TrackError(ex);
                        }
                    };


                    //CrossFirebasePushNotification.Current.OnNotificationAction += (s, p) =>
                    //{
                    //    Debug.WriteLine("Action");
                    //    if (p.Data.ContainsKey("body") && p.Data.ContainsKey("title"))
                    //        Device.BeginInvokeOnMainThread(() =>
                    //        {
                    //            HomeVariables.Init.Message = $"{p.Data["body"]}";
                    //            HomeVariables.Init.TitleMessage = $"{p.Data["title"]}";
                    //        });
                    //};

                    CrossFirebasePushNotification.Current.OnNotificationDeleted += (s, p) =>
                    {
                        Debug.WriteLine("Dismissed");
                    };
                }
                else
                {
                    MainPage = new NoInternet();
                }
            }
            catch (IOException ex)
            {
                Crashes.TrackError(ex);
            }
        }

        public async Task AddNotification(string _title, string _message, string _id, string _image)
        {
         
               await firebase
              .Child("Notifications")
              .Child(_id)
              .PutAsync(new FCM() { Title = _title, Message = _message, FCMImage = _image , Id = _id});
            
        }

       

        public static bool IsConnected { get; set; }

        public static void  MakeWebRequest()
        {
            if (!CrossConnectivity.Current.IsConnected)
                //You are offline, notify the user

                IsConnected = false;
            else
                IsConnected = true;
        }


        protected override void OnStart()
        {
            //Debug.WriteLine("OnStart");
            //try
            //{
            //    MakeWebRequest();
            //    if (IsConnected)
            //    {
            //        var rest = new RestAPI("http://mm-app.co.za/wp-json/wc/v3/",
            //            "ck_a25f96835aabfc64b09613eb8ec4a8c9bcd5dcd0", "cs_8f247c22353f25b905c96171379b89714f8f4003");

            //        WooObject = new WCObject(rest);


            //        AppCenter.Start("android=e41ba7e7-c843-43b4-977f-f71ec890ef0c;" +
            //                        "ios=5a0c8744-6679-45cb-8dfe-bba4f93befb3;",
            //            typeof(Analytics), typeof(Crashes));


            //        if (Preferences.ContainsKey("Cart"))
            //        {
            //            var FetchingCart = Preferences.Get("Cart", "default_value");
            //            FullCart.CartList = JsonConvert.DeserializeObject<List<CartList>>(FetchingCart);
            //        }

            //        if (FullCart.CartList?.Any() != true || FullCart.CartList.Any() != true ||
            //            FullCart.CartList.Count() == 0)
            //        {
            //            CartPersistance.LoadedCart = false;
            //        }
            //        else
            //        {
            //            CartPersistance.LoadedCart = true;
            //            var jsonString = JsonConvert.SerializeObject(FullCart.CartList);
            //            CartPersistance.PerCart = jsonString;
            //        }

            //        CrossFirebasePushNotification.Current.Subscribe("general");
            //        CrossFirebasePushNotification.Current.OnTokenRefresh += (s, p) =>
            //        {
            //            Debug.WriteLine($"TOKEN REC: {p.Token}");
            //        };

            CrossFirebasePushNotification.Current.OnNotificationReceived += (s, p) =>
            {
                try
                {
                    Debug.WriteLine("Received");
                    if (p.Data.ContainsKey("body") && p.Data.ContainsKey("title"))
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            HomeVariables.Init.Message = $"{p.Data["body"]}";
                            HomeVariables.Init.TitleMessage = $"{p.Data["title"]}";
                        });
                }
                catch (IOException ex)
                {
                    Crashes.TrackError(ex);
                }
            };
            //        CrossFirebasePushNotification.Current.OnNotificationOpened += (s, p) =>
            //        {
            //            //System.Diagnostics.Debug.WriteLine(p.Identifier);

            //            Debug.WriteLine("Opened");
            //            foreach (var data in p.Data) Debug.WriteLine($"{data.Key} : {data.Value}");

            //            if (!string.IsNullOrEmpty(p.Identifier))
            //                Device.BeginInvokeOnMainThread(() => { HomeVariables.Init.Message = p.Identifier; });
            //            else if (p.Data.ContainsKey("color"))
            //                Device.BeginInvokeOnMainThread(() =>
            //                {
            //                    mPage.Navigation.PushAsync(new ContentPage
            //                    {
            //                        BackgroundColor = Color.FromHex($"{p.Data["color"]}")
            //                    });
            //                });
            //            else if (p.Data.ContainsKey("title"))
            //                Device.BeginInvokeOnMainThread(() =>
            //                {
            //                    HomeVariables.Init.TitleMessage = $"{p.Data["title"]}";
            //                });
            //        };

            //        CrossFirebasePushNotification.Current.OnNotificationAction += (s, p) =>
            //        {
            //            Debug.WriteLine("Action");

            //            if (!string.IsNullOrEmpty(p.Identifier))
            //            {
            //                Debug.WriteLine($"ActionId: {p.Identifier}");
            //                foreach (var data in p.Data) Debug.WriteLine($"{data.Key} : {data.Value}");
            //            }
            //        };

            //        CrossFirebasePushNotification.Current.OnNotificationDeleted += (s, p) =>
            //        {
            //            Debug.WriteLine("Dismissed");
            //        };
            //    }
            //    else
            //    {
            //        MainPage = new NoInternet();
            //    }
            //}
            //catch (IOException ex)
            //{
            //    Crashes.TrackError(ex);
            //}
        }


        protected override void OnSleep()
        {
            //Debug.WriteLine("OnSleep");
            //try
            //{
            //        mPage = new Home();

            //        MainPage = new NavigationPage(mPage);

            //        CrossFirebasePushNotification.Current.Subscribe("general");
            //        CrossFirebasePushNotification.Current.OnTokenRefresh += (s, p) =>
            //        {
            //            Debug.WriteLine($"TOKEN REC: {p.Token}");
            //        };

            MakeWebRequest();

            if (IsConnected)
            {
                CrossFirebasePushNotification.Current.OnNotificationReceived += (s, p) =>
                {
                    try
                    {
                        Debug.WriteLine("Received");
                        if (p.Data.ContainsKey("body") && p.Data.ContainsKey("title"))
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                HomeVariables.Init.Message = $"{p.Data["body"]}";
                                HomeVariables.Init.TitleMessage = $"{p.Data["title"]}";
                            });
                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex);
                    }
                };
            }
        
            //        CrossFirebasePushNotification.Current.OnNotificationOpened += (s, p) =>
            //        {
            //            //System.Diagnostics.Debug.WriteLine(p.Identifier);
            //        };

            //        CrossFirebasePushNotification.Current.OnNotificationAction += (s, p) =>
            //        {
            //            Debug.WriteLine("Action");

            //            if (string.IsNullOrEmpty(p.Identifier)) return;
            //            Debug.WriteLine($"ActionId: {p.Identifier}");
            //            foreach (var data in p.Data) Debug.WriteLine($"{data.Key} : {data.Value}");
            //        };

            //        CrossFirebasePushNotification.Current.OnNotificationDeleted += (s, p) =>
            //        {
            //            Debug.WriteLine("Dismissed");
            //        };

            //        MainPage = new NavigationPage(mPage);
            //    }
            //    else
            //    {
            //        MainPage = new NoInternet();
            //    }
            //}
            //catch (IOException ex)
            //{
            //    Crashes.TrackError(ex);
            //}
        }

        protected override void OnResume()
        {

            MakeWebRequest();

            if (!IsConnected)
            {
                var masterDetailPage = new Home { Detail = new NavigationPage(new NoInternet()) };
                Application.Current.MainPage = masterDetailPage;
            }
            else
            {

            }
#if DEBUG

            Vibration.Vibrate();
            Debug.WriteLine("OnResume");
#endif
            }


        public static void SetDatailPage(Page page)
        {
            if (!(Current.MainPage is MasterDetailPage)) return;
            var masterPage = (MasterDetailPage) Current.MainPage;
            masterPage.Detail = new NavigationPage(page);
        }
    }
}