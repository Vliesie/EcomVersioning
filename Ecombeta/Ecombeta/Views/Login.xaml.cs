using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ecombeta.Models;
using Microsoft.AppCenter.Crashes;
using WooCommerce.NET.WordPress.v2;
using WooCommerceNET;
using WooCommerceNET.WooCommerce.v3;
using WordPressPCL;
using WordPressPCL.Models;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Firebase.Database;
using Firebase.Database.Query;
using System.Threading.Tasks;

namespace Ecombeta.Views
{
    public class Userlogin
    {
     
        public string user_login { get; set; }
        public string passwords { get; set; }
    }
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
    
        public FirebaseClient firebase = new FirebaseClient("https://mica-marketapp.firebaseio.com/");
        public Login()
        {
            InitializeComponent();

            try
            {
                #region Ini Fetch
          
                //Initialize Fetch Method
                ExtractWooData(FetchCustomers.Customers);

                #endregion
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void ExtractWooData(List<Customer> x)
        {

            try
            {
                if (Ecombeta.Models.Users.LoggedIn == true)
                {
               
                    var yx = await DisplayAlert("Hey!!", "Your already logged in Dont worry", "Ok", "Home");

                    if (yx)
                    {
                    }
                    else
                    {
                        var masterDetailPage = new Home();
                        masterDetailPage.Detail = new NavigationPage(new Home());
                        Application.Current.MainPage = masterDetailPage;
                    }
                }
                else
                {
                    Loginbtn.IsEnabled = true;
                }
            
                #region FetchC

                var rest = new RestAPI("http://mm-app.co.za/wp-json/wc/v3/",
                    "ck_a25f96835aabfc64b09613eb8ec4a8c9bcd5dcd0", "cs_8f247c22353f25b905c96171379b89714f8f4003");
                var wc = new WCObject(rest);
                var pageNum = 1;
                const bool isNull = false;

                while (!isNull)
                {
                    var page = pageNum.ToString();
                    x = await wc.Customer.GetAll(new Dictionary<string, string>
                    {
                        {
                            "page", page
                        },
                        {
                            "per_page", "100"
                        }
                    });
                    var oldlist = FetchCustomers.Customers ?? new List<Customer>();
                    if (x.Count == 0) break;

                    //1st loop customers needs to = 100
                    //2nd loop oldist needs to = 40+
                    //If count = 0 then => Combine Customers + Oldist
                    pageNum++;

                    FetchCustomers.Customers = oldlist.Union(x).ToList();
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }

            #endregion
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
        private readonly string ChildName = "Users";
        public async Task<List<Userlogin>> GetAllUser()
        {

                 return (await firebase
                .Child(ChildName)
                .OnceAsync<Userlogin>()).Select(item =>
                new Userlogin
                {
              
                    user_login = item.Object.user_login,
                    passwords = item.Object.passwords
                }).ToList();
              
        }

        public async Task<Userlogin> GetUser(string username)
        {
            try
            {
                var allUsers = await GetAllUser();
                await firebase
                .Child(ChildName)
                .OnceAsync<Userlogin>();
                return allUsers.Where(a => a.user_login == username).FirstOrDefault();
            }
            catch (Exception e)
            {
                
                Crashes.TrackError(e);
                return null;
               
            }
        }

        private async void Login_Phase1()
        {
            #region Login Phase 1

            try
            {
                if (string.IsNullOrEmpty(Usernamelabel.Text) || string.IsNullOrEmpty(Password.Text))
                {
                    await App.Current.MainPage.DisplayAlert("Empty Values", "Please enter Username and Password", "OK");
                }
                else
                {
                    //call GetUser function which we define in Firebase helper class    
                    var user = await GetUser(Usernamelabel.Text);
                    //firebase return null valuse if user data not found in database
                    TaskLoader.IsRunning = false;
                    LoadingOverlay.IsVisible = false;
                    if (user != null)
                    {
                        if (Usernamelabel.Text == user.user_login && Password.Text == user.passwords)
                        {
                            Login_Phase2();
                        }
                        else
                            await App.Current.MainPage.DisplayAlert("Login Fail", "Please enter correct Username and Password", "OK");
                    }

                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Login Fail", "User not found, Please check your Username", "OK");
                    }
                }

            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }

            #endregion
        }
        bool userfound;
        private async void Login_Phase2()
        {
            try
            {
                #region login Phase 2

                var list = FetchCustomers.Customers.ToList();
                // find the first match

                //var found = list.Where(user => user.username == Usernamelabel.Text).FirstOrDefault();
                if (list != null || list.Any())
                {
                    foreach (var user in list)
                    {

                        if (user.username == Usernamelabel.Text)
                        {
                            if (Usernamelabel.Text == "Micamarket")
                            {
                                GlobalVariable.Tester = true;
                                Preferences.Set("CId", user.id.ToString());
                                if (WpApiCredentials.token != null) Preferences.Set("Token", WpApiCredentials.token);

                                App.justloggedin = false;
                                Preferences.Set("CUsername", user.username);
                                Preferences.Set("CEmail", user.email);
                                Ecombeta.Models.Users.LoggedIn = true;
                                await Application.Current.SavePropertiesAsync();
                            }
                            else
                            {
                                GlobalVariable.Tester = false;
                                Preferences.Set("CId", user.id.ToString());
                                if (WpApiCredentials.token != null) Preferences.Set("Token", WpApiCredentials.token);
                          
                                App.justloggedin = false;
                                Home.UsersTitle = user.username;
                                Preferences.Set("CUsername", user.username);
                                Preferences.Set("CEmail", user.email);
                                Ecombeta.Models.Users.LoggedIn = true;
                                await Application.Current.SavePropertiesAsync();
                            }   

                        }

                    }
                    if (App.justloggedin != true)
                    {
                        await ShowLoggedIn();
                    }
                }
                else
                {
                    await DisplayAlert("Wordpress Error", "We Cant find your Customer Account", "OK");
                }
              
                //if (found != null)
                //{
                //    Preferences.Set("CId", found.id.ToString());
                //    if (WpApiCredentials.token != null)
                //    {
                //        Preferences.Set("Token", WpApiCredentials.token);
                //    }

                //    Preferences.Set("CUsername", found.username);
                //    Preferences.Set("CEmail", found.email);
                //    Users.Loggedin = true;
                //    Application.Current.SavePropertiesAsync();
                //    App.Current.MainPage.DisplayAlert("Complete", "Login Process Complete, Enjoy", "OK");
                //    App.Current.MainPage = new Home("Mica Market");
                //}
            }

            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }

            #endregion
        }

        private async Task ShowLoggedIn()
        {
                App.justloggedin = true;
                Application.Current.MainPage = new Home();
                await Application.Current.MainPage.DisplayAlert("Logged in", "Login Process Complete. Welcome to Mica Market", "OK");   
        }
        #region Login Button

        private void loginbtn_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (Ecombeta.Models.Users.LoggedIn == true)
                {
                   DisplayAlert(":)", "Your Still logged in dont worry", "Ok", "Home");
                    Loginbtn.IsEnabled = false;
                }
                else
                {
                    TaskLoader.IsRunning = true;
                    LoadingOverlay.IsVisible = true;
                    ExtractWooData(FetchCustomers.Customers);
                    WpApiCredentials.Username = Usernamelabel.Text;
                    WpApiCredentials.Password = Password.Text;
                    Login_Phase1();

                }
             }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }


        #endregion
    }
}