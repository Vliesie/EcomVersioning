using System.ComponentModel;
using Ecombeta.Models;
using Ecombeta.Views;
using Xamarin.Forms;

namespace Ecombeta.ViewModel
{
    public class WelcomePageVM : INotifyPropertyChanged
    {
        private string password;

        public WelcomePageVM(string email2)
        {
            Email = email2;
        }

        public string Email { get; set; }

        public string Password
        {
            get => password;
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
                    Application.Current.MainPage.DisplayAlert("Thank you", "You've Succesfully Logged out", "OK");
                    Users.LoggedIn = false;
                    Application.Current.MainPage.Navigation.PushAsync(new Home());
                });
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public async void Goback()
        {
        }
    }
}