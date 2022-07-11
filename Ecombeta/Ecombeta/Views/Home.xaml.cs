using System;
using System.Collections.Generic;
using System.ComponentModel;
using Ecombeta.Models;
using Microsoft.AppCenter.Crashes;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Ecombeta.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Home : MasterDetailPage, INotifyPropertyChanged
    {
        public static string UsersTitle;
        public Home()
        {
            try
            {
            
                InitializeComponent();
        
                NavigationPage.SetHasNavigationBar(this, false);
             
                HomeVariables.Init.PropertyChanged += HomeVariables.Init.HandleCustomEvent;

                HomeVariables.Init.MenuList = new List<MasterPageItem>();
                // Adding menu items to menuList and you can define title ,page and icon  
                HomeVariables.Init.MenuList.Add(new MasterPageItem
                {
                    Title = "Home",
                    Icon = "IC3.png",
                    TargetType = typeof(MainPage)
                });
                HomeVariables.Init.MenuList.Add(new MasterPageItem
                {
                    Title = "Login",
                    Icon = "IC8.png",
                    TargetType = typeof(Login)
                });
                HomeVariables.Init.MenuList.Add(new MasterPageItem
                {
                    Title = "Suppliers",
                    Icon = "IC6.png",
                    TargetType = typeof(Suppliers)
                });
                HomeVariables.Init.MenuList.Add(new MasterPageItem
                {
                    Title = "Orders",
                    Icon = "IC5.png",
                    TargetType = typeof(OrderList)
                });
                HomeVariables.Init.MenuList.Add(new MasterPageItem
                {
                    Title = "Cart",
                    Icon = "IC1.png",
                    TargetType = typeof(Cart)
                });
                HomeVariables.Init.MenuList.Add(new MasterPageItem
                {
                    Title = "Attendees",
                    Icon = "IC4.png",
                    TargetType = typeof(Attendees)
                });
                HomeVariables.Init.MenuList.Add(new MasterPageItem
                {
                    Title = "Itinerary",
                    Icon = "IC2.png",
                    TargetType = typeof(itinerary)
                });
                HomeVariables.Init.MenuList.Add(new MasterPageItem
                {
                    Title = "Gala Seating",
                    Icon = "IC9.png",
                    TargetType = typeof(Gala)
                });
                HomeVariables.Init.MenuList.Add(new MasterPageItem
                {
                    Title = "Flash Sales",
                    Icon = "IC7.png",
                    TargetType = typeof(FlashSales)
                });
                HomeVariables.Init.MenuList.Add(new MasterPageItem
                {
                    Title = "Flash Cart",
                    Icon = "IC1.png",
                    TargetType = typeof(Flashcart)
                });
                HomeVariables.Init.MenuList.Add(new MasterPageItem
                {
                    Title = "Notification",
                    Icon = "IC7.png",
                    TargetType = typeof(NotificationFeed)
                });
                HomeVariables.Init.MenuList.Add(new MasterPageItem
                {
                    Title = "Help",
                    Icon = "IC9.png",
                    TargetType = typeof(HelpPage)
                });


                // Setting our list to be ItemSource for ListView in MainPage.xaml  
                navigationDrawerList.ItemsSource = HomeVariables.Init.MenuList;
                // Initial navigation, this can be used for our home page  
                Detail = new NavigationPage((Page) Activator.CreateInstance(typeof(MainPage)));
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }

            try
            {
                NavigationPage.SetHasBackButton(this, false);
                //This just saves tot he Microsoft Prefrences Servers Its better then System Propertiers (Setting files on the phone) In my Expierence atleast

                #region Data Persistance

                if (Preferences.ContainsKey("CId"))
                {
                    //Fetch Customer ID
                    var CustomerID = Preferences.Get("CId", "default_value");
                    Users.CId = Convert.ToInt32(CustomerID);

                    Users.LoggedIn = true;

                    var CustomerName = Preferences.Get("CUsername", "default_value");
                    logoutbtn.IsVisible = true;
                    //Fetch Customer Email
                    var userToken = WpApiCredentials.token;
                    userToken = Preferences.Get("Token", "default_value");
                    //var name = Usermail.Substring(0, Usermail.IndexOf('@')).Replace(".", " ");
                    Emailnav.Text = CustomerName;
                    Users.CEmail = Preferences.Get("CEmail", "default_value");
                    var useremail = Preferences.Get("CEmail", "default_value");
                    //Fetch Customer Username
                    userEmail.Text = useremail;
                    Users.Username = Preferences.Get("CUsername", "default_value");
                   
                }
                else
                {
                    Users.LoggedIn = false;
                    Emailnav.Text = "Please log in";
                    logoutbtn.IsVisible = false;
                }

                #endregion
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private void OnMenuItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                var item = (MasterPageItem) e.SelectedItem;
                var page = item.TargetType;
                Detail = new NavigationPage((Page) Activator.CreateInstance(page));
                ((ListView)sender).SelectedItem = null;
                IsPresented = false;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private void Checkoutbtn_Clicked(object sender, EventArgs e)
        {
            try
            {
                Application.Current.MainPage.Navigation.PushAsync(new Cart());
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private void logoutbtn_Clicked(object sender, EventArgs e)
        {
            try
            {
                Preferences.Clear();
                Application.Current.MainPage.DisplayAlert("Thank you", "You've Succesfully Logged out", "OK");
                Users.LoggedIn = false;
                Users.CId = 0;
                logoutbtn.IsVisible = false;
                Emailnav.Text = "Please log in";
                var masterDetailPage = new Home { Detail = new NavigationPage(new MainPage()) };
                Application.Current.MainPage = masterDetailPage;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }
        protected async override void OnAppearing()
        {
            App.MakeWebRequest();
            if (App.IsConnected)
            {

            }
            else
            {
                await Navigation.PushAsync(new NoInternet());
            }
        }

        void ImageButton_Clicked(System.Object sender, System.EventArgs e)
        {
        }

        void CartIcon_Clicked(System.Object sender, System.EventArgs e)
        {
            var masterDetailPage = new Home { Detail = new NavigationPage(new Cart()) };
            Application.Current.MainPage = masterDetailPage;
        }
    }
}