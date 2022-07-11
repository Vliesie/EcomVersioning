    using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Ecombeta.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Checkedout : ContentPage
    {
        public Checkedout()
        {
            InitializeComponent();

            try
            {
                imageback.Source = "https://mm-app.co.za/wp-content/uploads/2019/12/Orangepoly-e1576143689778.jpg";
            }
            catch (Exception ex)
            {
            }
        }

        private void Shopping_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Suppliers());
        }
    }
}