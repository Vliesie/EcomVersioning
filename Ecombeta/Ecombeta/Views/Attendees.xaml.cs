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
    public partial class Attendees : ContentPage
    {
        public Attendees()
        {
            InitializeComponent();
            Atend.Reload();
            Atend.Source = "https://mm-app.co.za/attendees-2/";
        
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}