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
    public partial class Checkedout : ContentPage
    {
        public Checkedout()
        {
            InitializeComponent();
            //try
            //{
            //    suc.Source = "https://mm-app.co.za/wp-content/uploads/2019/12/shopping_cart_2.gif";
            //}
            //catch (Exception)
            //{
             
               
            //}
           
            imageback.Source = "https://mm-app.co.za/wp-content/uploads/2019/12/Orangepoly-e1576143689778.jpg";
        }

        private void Shopping_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Suppliers());
        }
    }
}