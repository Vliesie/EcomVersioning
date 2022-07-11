using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Ecombeta.Models;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using WooCommerceNET;
using WooCommerceNET.WooCommerce.v3;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Ecombeta.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FlashSales : ContentPage, INotifyPropertyChanged
    {

        public List<Product> product;

        private Flashlist _items;

    

        public FlashSales()
        {
            if (Users.LoggedIn == true)
            {
                InitializeComponent();

                try
                {
                    InitAsync();
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }
            }   
            else
            {
                NotLogged();
            }
        }

            public async Task NotLogged() {


            var y = await DisplayAlert("Login to View this Page", "Please Login", "Login", "Home");

            if (y)
            {
                var masterDetailPage = new Home { Detail = new NavigationPage(new Login()) };
                Application.Current.MainPage = masterDetailPage;
               
            }
            else
            {
                var masterDetailPage = new Home { Detail = new NavigationPage(new MainPage()) };
                Application.Current.MainPage = masterDetailPage;

            }
        }



        private async Task InitAsync()
        {

            try
            {
              
                var rest = new RestAPI("http://mm-app.co.za/wp-json/wc/v3/",
                    "ck_a25f96835aabfc64b09613eb8ec4a8c9bcd5dcd0", "cs_8f247c22353f25b905c96171379b89714f8f4003");
                var wc = new WCObject(rest);

                product = await wc.Product.GetAll(new Dictionary<string, string>
                {
                    {"tag", "1486"},
                    {"per_page", "80"}
                });


                    if (product != null)
                    {
                        _items = new Flashlist(product);
                    }
                    else
                    {
                        _items.Items.Clear();
                    }
                   
                    try
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            productsListView.FlowItemsSource = _items.Items.Where(z => z.status == "publish").ToList();
                        });
                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex);
                    }
                
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                Analytics.TrackEvent(ex.ToString());
            }
        }

        protected async override void OnAppearing()
        {

            App.MakeWebRequest();
            if (App.IsConnected)
            {

              
                Device.StartTimer(TimeSpan.FromSeconds(15),  () =>
                {
                    Task.Run(async () =>
                    {
                        await InitAsync();
                    });
                    return true;
                });

               
            }
            else
            {
                await Navigation.PushAsync(new NoInternet());
            }
        }

        private async void ProductClicked(object sender, EventArgs args)
        {
            try
            {
                SingleProductView.FlashSale = true;
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
    }
}