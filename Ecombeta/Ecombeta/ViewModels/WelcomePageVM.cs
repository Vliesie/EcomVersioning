
using Ecombeta.Models;
using Ecombeta.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using Xamarin.Forms;

namespace Ecombeta.ViewModel
{
    public class WelcomePageVM : INotifyPropertyChanged
    {
        public WelcomePageVM(string email2)
        {
            Email = email2;
        }
        private string email;

        public string Email
        {
            get { return email; }
            set { email = value; }
        }
        private string password;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Password"));
            }
        }
      
        //For Logout
        public Command LogoutCommand
        {
           

            get
            {
                return new Command(() =>
                {
                    
                    Application.Current.Properties.Remove("CId");
                    Application.Current.Properties.Remove("Token");
                    Application.Current.Properties.Remove("CUsername");
             

                    App.Current.MainPage.DisplayAlert("Thank you", "You've Succesfully Logged out", "OK");
                    Users.Loggedin = false;
                    App.Current.MainPage.Navigation.PushAsync(new Home("Mica Market"));

                });
            }
        }

        public async void  Goback()
        {
          
          
        }

      


    }
}
