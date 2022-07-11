using System.ComponentModel;
using Xamarin.Forms;

namespace Ecombeta.ViewModel
{
    public class SignUpVM : INotifyPropertyChanged
    {
        private string confirmpassword;
        private string email;
        private string password;

        public string Email
        {
            get => email;
            set
            {
                email = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Email"));
            }
        }

        public string Password
        {
            get => password;
            set
            {
                password = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Password"));
            }
        }

        public string ConfirmPassword
        {
            get => confirmpassword;
            set
            {
                confirmpassword = value;
                PropertyChanged(this, new PropertyChangedEventArgs("ConfirmPassword"));
            }
        }

        public Command SignUpCommand
        {
            get
            {
                return new Command(() =>
                {
                    if (Password == ConfirmPassword)
                        SignUp();
                    else
                        Application.Current.MainPage.DisplayAlert("", "Password must be same as above!", "OK");
                });
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private async void SignUp()
        {
            //null or empty field validation, check weather email and password is null or empty

            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
                await Application.Current.MainPage.DisplayAlert("Empty Values", "Please enter Email and Password",
                    "OK");
        }
    }
}