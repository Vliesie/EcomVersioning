using Ecombeta.Models;
using Ecombeta.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WooCommerceNET.WooCommerce.v3;
using WooCommerceNET.WooCommerce.v3.Extension;
using WooCommerceNET;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net;
using System.IO;
using WordPressPCL;
using WordPressPCL.Models;
using Newtonsoft.Json;
using WordPressPCL.Utility;
using System.Net.Http;
using Xamarin.Essentials;

namespace Ecombeta.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        LoginViewModel loginViewModel;
        public Login()
        {

            InitializeComponent();

            #region Ini Fetch
            //Initialize Fetch Method
            ExtractWooData(FetchCustomers.customers);
            #endregion

            #region View Model Ini
            //Initialize View Model for Login
            loginViewModel = new LoginViewModel();
           
            BindingContext = loginViewModel;
            #endregion
        }

        private async void ExtractWooData(List<Customer> x)
        {
            #region FetchC
            RestAPI rest = new RestAPI("http://mm-app.co.za/wp-json/wc/v3/", "ck_a25f96835aabfc64b09613eb8ec4a8c9bcd5dcd0", "cs_8f247c22353f25b905c96171379b89714f8f4003");
            WCObject wc = new WCObject(rest);


            int pageNum = 1;
            var isNull = false;
            List<Customer> oldlist;
           
            while (!isNull)
            {
                
                var page = pageNum.ToString();
                x = await wc.Customer.GetAll(new Dictionary<string, string>() {
                {
                    "page", page
                }, {
                    "per_page", "100"
                }
            });
                oldlist = FetchCustomers.customers ?? new List<Customer>();
                if (x.Count == 0) {

                   
                    break;
                }
                else
                {
                    //1st loop customers needs to = 100
                    //2nd loop oldist needs to = 40+
                    //If count = 0 then => Combine Customers + Oldist
                    pageNum++;
                    
                    FetchCustomers.customers = oldlist.Union(x).ToList();


                }

            }
            #endregion
        }
    
       
        public async void Login_Phase1()
        {
            #region Login Phase 1
     
            var client = new WordPressClient("http://mm-app.co.za/wp-json/");
            client.AuthMethod = AuthMethod.JWT;
            try
            {
                await client.RequestJWToken(Usernamelabel.Text, Password.Text);
            }
            catch (Exception e)
            {
                await App.Current.MainPage.DisplayAlert("Wrong Detail's", e.ToString(), "OK");
              
            }
          

            var x = client;
            var isValidToken = await client.IsValidJWToken();

            WpApiCredentials.token = client.GetToken();

            if (isValidToken)
            {
                
                Login_Phase2();
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Empty Values", "Token not Found", "OK");
            }
            #endregion
        }

        public void Login_Phase2()
        {
            #region login Phase 2
            var list = FetchCustomers.customers.ToList();

            foreach (var user in list)
            {

                if (user.username == Usernamelabel.Text)
                {
                    Preferences.Set("CId", user.id.ToString());
                    Preferences.Set("Token", WpApiCredentials.token);
                    Preferences.Set("CUsername", user.username);
                    Preferences.Set("CEmail", user.email);
                    Users.Loggedin = true;
                    Application.Current.SavePropertiesAsync();
                    App.Current.MainPage.DisplayAlert("Complete", "Login Process Complete, Enjoy", "OK");
                    App.Current.MainPage = new Home("Mica Market");
                }
            }
            #endregion
        }

        #region Login Button
        private void loginbtn_Clicked(object sender, EventArgs e)
        {
            WpApiCredentials.Username = Usernamelabel.Text;
            WpApiCredentials.Password = Password.Text;
            Login_Phase1();
        }

        #endregion
    }
}