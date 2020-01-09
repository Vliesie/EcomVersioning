using Ecombeta.Models;
using Ecombeta.ViewModel;
using Octane.Xamarin.Forms.VideoPlayer.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Ecombeta.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
       
        public bool Hide;
        WelcomePageVM welcomePageVM;
        LoginViewModel loginViewModel;
        public MainPage()
        {
            NavigationPage.SetHasBackButton(this, false);

            string year = DateTime.Now.Year.ToString();

            InitializeComponent();
            if (Users.Loggedin == true)
            {
                Hide = false;
            }
            if (Users.Loggedin == false)
            {
                Hide = true;
            }

            loginViewModel = new LoginViewModel();

            BindingContext = loginViewModel;
        }

        private void VideoPlayer_OnPlaying(object sender, VideoPlayerEventArgs e)
        {
         
            Zmain.IsVisible = false;

        }
      

         private void VideoPlayer_OnStopped(object sender, VideoPlayerEventArgs e)
        {
            Zmain.IsVisible = true;
        }

        private void VideoPlayer_OnCompleted(object sender, VideoPlayerEventArgs e)
        {
            Zmain.IsVisible = true;
        }
 
        private void loginbtn_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage.Navigation.PushAsync(new Login());
        }
    }
}