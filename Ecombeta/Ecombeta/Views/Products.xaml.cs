using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecombeta.Models;
using Microsoft.AppCenter.Crashes;
using WooCommerceNET.WooCommerce.v3;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Ecombeta.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Products : ContentPage
    {
        public Products()
        {
            InitializeComponent();
            try
            {
                Testing.BackgroundImageSource = "https://mm-app.co.za/wp-content/uploads/2019/12/Orangepoly.jpg";
                InitAsync();
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

  
        private async Task InitAsync()
        {
            try
            {
                //I need to debug the app when I have a machine but The Variable products(More then one) arent loading on Orders wich is the Single product basicly 
                var wc = new WCObject(GlobalVariable.Init.rest);

                var products = await wc.Product.GetAll(new Dictionary<string, string>
                {
                    {"tag", SuppliersVariables.Init.TagId},
                    {"per_page", "100"}
                });
                ;

                //foreach (var item in products)
                //{
                //    if (item.status == "draft")
                //    {
                //        products.Remove(item);
                //    }
                //}

                //var y = products.Select((item) => (item.status == "draft"));
                //products.Remove(y);

               

              
                //var Flashmatch = p.Where(m => m.tags != null && m.tags.Any(u => u.name == "Flash Sale's")).ToList();
                SingleProductView.FlashSale = SuppliersVariables.Init.TagId == "1486";
                foreach (var item in products)
                {
                    if (item.stock_status == "outofstock")
                    {
                        item.images[0].src = "https://mm-app.co.za/wp-content/uploads/2020/02/Outofstock.jpg";
                    }
              
                }
                productsListView.FlowItemsSource = products.Where(p => p.status != "draft").ToList(); 
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void ProductClicked(object sender, EventArgs args)
        {
            try
            {
                var btn = (Button) sender;
                var a = btn.BindingContext;

                SingleProductView.SingleId = Convert.ToInt32(a);
                if (GlobalVariable.Tester == true)
                {
                   await DisplayAlert("Hello Tester", "You wont have access to this during the Testing Period", "Ok");
                }
                else
                {
                    await Navigation.PushAsync(new SingleProductView());
                }
              
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        protected async override void OnAppearing()
        {
            App.MakeWebRequest();
            if (App.IsConnected)
            {

            }
            else
            {
                await Navigation.PushAsync(new NoInternet());
            }
        }
    }
}