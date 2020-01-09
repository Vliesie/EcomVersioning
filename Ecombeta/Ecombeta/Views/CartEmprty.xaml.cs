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
    public partial class CartEmprty : ContentPage
    {
        public CartEmprty()
        {
            InitializeComponent();
         
            Backgroundimage.Source = "https://mm-app.co.za/wp-content/uploads/2019/12/OrangeBluepoly.jpg";
          
            NavigationPage.SetHasBackButton(this, false);
        }

       
        private void loginbtn_Clicked(object sender, EventArgs e)
        {

        }

        private void Shopping_Clicked(object sender, EventArgs e)
        {
             App.Current.MainPage = new Home("Mica Market");
        }
    }
}