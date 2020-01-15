using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using WooCommerceNET.WooCommerce.v3;
using WooCommerceNET.WooCommerce.v3.Extension;
using WooCommerceNET;
using System.Net;
using System.Windows.Input;
using Ecombeta.Models;

namespace Ecombeta.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Suppliers : ContentPage
    {   
        public static string tagid { set; get; }
        public ICommand PinButtonCommand { get; private set; }


        public  Suppliers()
        {
            
            InitializeComponent();
 
            ImageBack.BackgroundImageSource = "https://mm-app.co.za/wp-content/uploads/2019/12/Bluepoly.jpg";
            InitAsync();
          
        }

        public List<Customer> a;
        private async Task InitAsync()
        {
            //There is about 98 Suppliers currentley its just a Image with a Button that passes a ID to use to get all Products under that Supplier Its basiclly just a Categorie 
            RestAPI rest = new RestAPI("http://mm-app.co.za/wp-json/wc/v3/", "ck_a25f96835aabfc64b09613eb8ec4a8c9bcd5dcd0", "cs_8f247c22353f25b905c96171379b89714f8f4003");
            WCObject wc = new WCObject(rest);
           // var products = await wc.Tag.GetAll();
            var p = await wc.Tag.GetAll(new Dictionary<string, string>() {

                   { "per_page", "100" } });
            productsListView.FlowItemsSource = p;
        }

        async void SupplierClicked(object sender, EventArgs args)
        {  
            var btn = (Button)sender;
            var product = btn.BindingContext;

            tagid = product.ToString();
            await Navigation.PushAsync(new Products());

        }
    }
}