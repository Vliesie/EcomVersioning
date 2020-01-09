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
    public partial class itinerary : ContentPage
    {
        public itinerary()
        {
            InitializeComponent();
            Webview.Reload();
            Webview.Source = "https://mm-app.co.za/itinerary/";
        }

        protected override bool OnBackButtonPressed()
        {
            return true;

        }
    }
}