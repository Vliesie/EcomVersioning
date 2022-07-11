using System;
using Microsoft.AppCenter.Crashes;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Ecombeta.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Attendees : ContentPage
    {
        public Attendees()
        {
            InitializeComponent();
            try
            {
                Atend.Reload();
                Atend.Source = "https://mm-app.co.za/attendees-2/";
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