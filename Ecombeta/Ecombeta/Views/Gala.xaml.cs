using System;
using Microsoft.AppCenter.Crashes;
using Xamarin.Forms;

namespace Ecombeta.Views
{
    public partial class Gala : ContentPage
    {
        public Gala()
        {
            InitializeComponent();
            try
            {
                Webview.Reload();
                Webview.Source = "https://mm-app.co.za/gala-guest/";
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}