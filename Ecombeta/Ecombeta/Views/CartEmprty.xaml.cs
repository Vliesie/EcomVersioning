using System;
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
            try
            {
                Backgroundimage.Source = "https://mm-app.co.za/wp-content/uploads/2019/12/OrangeBluepoly.jpg";

                NavigationPage.SetHasBackButton(this, false);
            }
            catch (Exception ex)
            {
            }
        }


        private void loginbtn_Clicked(object sender, EventArgs e)
        {
        }

        private void Shopping_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new Home();
        }
    }
}