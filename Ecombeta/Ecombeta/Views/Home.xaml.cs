using Ecombeta.Models;
using Ecombeta.ViewModel;

using Plugin.FirebasePushNotification;
using WooCommerceNET.WooCommerce.v3;
using WooCommerceNET.WooCommerce.v3.Extension;
using WooCommerceNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Ecombeta.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
 
    

    public partial class Home : MasterDetailPage, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public List<MasterPageItem> menuListz { get; set; }
        private string _message;
        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                if (_message != value)
                {
                    _message = value;
                    RaisePropertyChanged();
                }
            }
        }
        //Encapsulation wich I dont understand RIP.  Hope i'm doing this right would appreciate feedback on this one. (I want it to also Show the Display alert even if the same Value gets sent in so Lets say the Id is 2 and I send another Notification with another Id of 2 5 minutes later then it should show again)
        private string _titlemessage;
        public string TitleMessage
        {
            get
            {
                return _titlemessage;
            }
            set
            {
                if (_titlemessage != value)
                {
                    _titlemessage = value;
                   
                }
            }
        }


        async void HandleCustomEvent(object sender, PropertyChangedEventArgs a)
        {
             RestAPI rest = new RestAPI("http://mm-app.co.za/wp-json/wc/v3/", "ck_a25f96835aabfc64b09613eb8ec4a8c9bcd5dcd0", "cs_8f247c22353f25b905c96171379b89714f8f4003");
             WCObject wc = new WCObject(rest);
   
                //This is my Scuff way of getting live Supplier Changes from the Firebase Network
                var y = await App.Current.MainPage.DisplayAlert("Flash Sale", Message, "Go Sale", "ok");
                if (y)
                {
                    
                    //I cant test this but writing this from my head in Notepad xD || What I expect this to do is Instead of passing a ID into the title ill give a name
                    // Then its gonna fetch every supplier then im going to check against every supplier if the title message I got from app.xaml.cs is = to one of the suppliers name's
                    // It should set flashID to the id of that supplier and give that to the Flashsale page to display the item's thus no Ugly ints in the Push notifications
                    string flashID;
                     var p = await wc.Tag.GetAll(new Dictionary<string, string>() {

                      { "per_page", "100" } });

                    foreach(var item in p){
                         if(_titlemessage == item.name){
                         flashID = item.id
                      }
                    }
                    await Navigation.PushAsync(new FlashSale_s(flashID));
                }
         
            
        }


        protected void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {              
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    
        public Home(string x)
       {
            this.PropertyChanged += HandleCustomEvent;

            //This is just the Menu
            x = "Mica Market";
            Title = x;
            InitializeComponent();

            menuListz = new List<MasterPageItem>();
            // Adding menu items to menuList and you can define title ,page and icon  
            menuListz.Add(new MasterPageItem()
            {
                Title = "Home",
                Icon = "IC3.png",
                TargetType = typeof(MainPage)
            });
            menuListz.Add(new MasterPageItem()
            {
                Title = "Login",
                Icon = "IC8.png",
                TargetType = typeof(Login)
            });
            menuListz.Add(new MasterPageItem()
            {
                Title = "Suppliers",
                Icon = "IC6.png",
                TargetType = typeof(Suppliers)
            });
            menuListz.Add(new MasterPageItem()
            {
                Title = "Orders",
                Icon = "IC5.png",
                TargetType = typeof(OrderList)
            });
            menuListz.Add(new MasterPageItem()
            {
                Title = "Checkout",
                Icon = "IC1.png",
                TargetType = typeof(Cart)
            });
            menuListz.Add(new MasterPageItem()
            {
                Title = "Attendees",
                Icon = "IC4.png",
                TargetType = typeof(Attendees)
            });
            menuListz.Add(new MasterPageItem()
            {
                Title = "Itinerary",
                Icon = "IC2.png",
                TargetType = typeof(itinerary)
            });
            menuListz.Add(new MasterPageItem()
            {
                Title = "CheckoutedTest",
                Icon = "IC7.png",
                TargetType = typeof(Checkedout)
            });

            

            // Setting our list to be ItemSource for ListView in MainPage.xaml  
            navigationDrawerList.ItemsSource = menuListz;
            // Initial navigation, this can be used for our home page  
            Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(MainPage)));

            NavigationPage.SetHasBackButton(this, false);
            //This just saves tot he Microsoft Prefrences Servers Its better then System Propertiers (Setting files on the phone) In my Expierence atleast
            #region Data Persistance
            if (Preferences.ContainsKey("CId"))
            {
                //Fetch Customer ID
                var CustomerID = Preferences.Get("CId", "default_value");
                Users.CId = Convert.ToInt32(CustomerID);

                Users.Loggedin = true;

                var CustomerName = Preferences.Get("CUsername", "default_value");
                logoutbtn.IsVisible = true;
                //Fetch Customer Email
                var userToken = WpApiCredentials.token;
                userToken = Preferences.Get("Token", "default_value");
                //var name = Usermail.Substring(0, Usermail.IndexOf('@')).Replace(".", " ");
                Emailnav.Text = "User: "+CustomerName;

                Users.CEmail = Preferences.Get("CEmail", "default_value");
                //Fetch Customer Username
                Users.Username = Preferences.Get("CUsername", "default_value");
            }
            else
            {
                Users.Loggedin = false;
                Emailnav.Text = "Please log in";
               logoutbtn.IsVisible = false;
            }
            #endregion
        }
        private void OnMenuItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = (MasterPageItem)e.SelectedItem;
            Type page = item.TargetType;
            Detail = new NavigationPage((Page)Activator.CreateInstance(page));
            IsPresented = false;
        }

        private void Checkoutbtn_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage.Navigation.PushAsync(new Cart());
        }


    
        private void logoutbtn_Clicked(object sender, EventArgs e)
        {
            string dummymail = "";
            Preferences.Clear();
            App.Current.MainPage.DisplayAlert("Thank you", "You've Succesfully Logged out", "OK");
            Users.Loggedin = false;
            App.Current.MainPage.Navigation.PushAsync(new Home("Mica Market"));
        }
    }
}