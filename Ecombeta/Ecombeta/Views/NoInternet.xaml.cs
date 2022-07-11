using System;
using Microsoft.AppCenter.Crashes;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;

namespace Ecombeta.Views
{
    public partial class NoInternet : ContentPage
    {
        public NoInternet()
        {
            InitializeComponent();
            try
            {
                Backgroundimage.Source = "https://mm-app.co.za/wp-content/uploads/2019/12/OrangeBluepoly.jpg";

                NavigationPage.SetHasBackButton(this, false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void Shopping_Clicked(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushPopupAsync(new TestingConnection());
                App.MakeWebRequest();
                if (App.IsConnected)
                {
                    await Navigation.PopPopupAsync();
                    Application.Current.MainPage = new Home();
                }
                else
                {
                    await DisplayAlert("No Internet", "Could not find a Internet Connection", "ok");
                }
                await Navigation.PopPopupAsync();
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
          
            //await Navigation.PushModalAsync(new NavigationPage(new Home("Mica Market")));
            // Navigation.PushModalAsync(new Home("Mica Market
        }
    }
}