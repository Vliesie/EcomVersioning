//using System.Collections.Generic;
//using System.ComponentModel;
//using Ecombeta.Views;
//using WooCommerceNET;
//using WooCommerceNET.WooCommerce.v3;
//using Xamarin.Forms;

//namespace Ecombeta.ViewModel
//{
//    public class LoginViewModel : INotifyPropertyChanged
//    {
//        public List<Customer> customers;

//        //Not Using this just keeping it as a Example
//        public bool loginsuc;

//        //private string usersname;
//        //public string Usersname
//        //{
//        //    get { return Usersname; }
//        //    set
//        //    {
//        //        Usersname = value;
//        //        PropertyChanged(this, new PropertyChangedEventArgs("Usersname"));
//        //    }
//        //}
//        private string password;

//        public string Password
//        {
//            get => password;
//            set
//            {
//                password = value;
//                PropertyChanged(this, new PropertyChangedEventArgs("Password"));
//            }
//        }

//        public Command LoginCommand => new Command(Login);

       

//        public event PropertyChangedEventHandler PropertyChanged;


//        //foreach (var user in list.Where(currentUser => string.Equals(currentUser.username?? string.Empty, Usersname ?? string.Empty,
//        //            StringComparison.CurrentCultureIgnoreCase)))
//        //{

//        //}


//        private async void Login()
//        {
//            var rest = new RestAPI("http://mm-app.co.za/wp-json/wc/v3/", "ck_a25f96835aabfc64b09613eb8ec4a8c9bcd5dcd0",
//                "cs_8f247c22353f25b905c96171379b89714f8f4003");
//            var wc = new WCObject(rest);

//            //var x = await wc.Customer.GetAll(new Dictionary<string, string>() {
//            //    {
//            //        "page", "1"
//            //    }, {
//            //        "per_page", "100"
//            //    }
//            //});

//            //Checking threw 1st 100 Customers details
//            //if (Linq.Username == email && Linq.password == Password)
//            //{
//            //    await App.Current.MainPage.DisplayAlert("Login Success", "", "Ok");
//            //    //Navigate to Wellcom page after successfuly login
//            //    //pass user email to welcom page
//            //    // 
//            //    // Application.Current.Properties["Userid"] = ;
//            //  
//            //    
//            //}


//            //else
//            //{
//            //    var v = await wc.Customer.GetAll(new Dictionary<string, string>() {
//            //    {
//            //        "page", "2"
//            //    }, {
//            //        "per_page", "100"
//            //    }

//            //   });
//            //    //Loading other 48 and Then checking against them if the other one failed
//            //    //if (Linqv.username == email && Linq.password == Password)
//            //    //{
//            //    //    await App.Current.MainPage.DisplayAlert("Login Success", "", "Ok");
//            //    //    Navigate to Wellcom page after successfuly login
//            //    //    pass user email to welcom page
//            //    //     Application.Current.Properties["Email"] = email;
//            //    //    Application.Current.Properties["Userid"] = ;
//            //    //    Users.Loggedin = true;
//            //    //    await Application.Current.SavePropertiesAsync();
//            //    //   
//            //    //}

//            //}
//            //null or empty field validation, check weather email and password is null or empty

//            //if (string.IsNullOrEmpty(Usersname) || string.IsNullOrEmpty(Password))
//            //    await App.Current.MainPage.DisplayAlert("Empty Values", "Please enter Email and Password", "OK");
//            //else
//            //{
//            //    //call GetUser function which we define in Firebase helper class

//            //    //firebase return null valuse if user data not found in database
//            //  var username = Users.a.Where(c => c.username == Usersname)
//            //        .Select(c => c)
//            //        .Select(o => o.email);

//            //  var password = Users.a.Where(c => c.password == Password)
//            //        .Select(c => c)
//            //        .Select(v => v.id);

//            //        await App.Current.MainPage.DisplayAlert("Login Success", "", "Ok");
//            //        //Navigate to Wellcom page after successfuly login
//            //        //pass user email to welcom page
//            //       Application.Current.Properties["Email"] = password;
//            //       // Application.Current.Properties["Userid"] = ;
//            //        Users.Loggedin = true;
//            //        await Application.Current.SavePropertiesAsync();
//            //       await App.Current.MainPage.Navigation.PushAsync(new Home(email));

//            //}
//        }
//    }
//}