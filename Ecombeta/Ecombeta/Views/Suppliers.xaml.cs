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
    public partial class Suppliers : ContentPage
    {
        public Suppliers()
        {
            if (Users.LoggedIn == true)
            {
                try
                {
                    InitializeComponent();
                    ImageBack.BackgroundImageSource = "https://mm-app.co.za/wp-content/uploads/2019/12/Bluepoly.jpg";

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
        public async Task NotLogged()
        {
            var y = await DisplayAlert("Login to View this Page", "Please Login", "Login",
                            "Home");
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

        protected override async void OnAppearing()
        {
            try
            {
                App.MakeWebRequest();
                if (App.IsConnected)
                {
                    //    WCObject wc = new WCObject(rest);
                    //    //This runs
                    //    Tags = await wc.Tag.GetAll(new Dictionary<string, string>() {
                    //    { "per_page", "100" } });
                    //    //This DOesnt
                    //    productsListView.FlowItemsSource = Tags;
                }
                else
                {
                    await Navigation.PushAsync(new NoInternet());
                }
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
                TaskLoader.IsRunning = true;
                LoadingOverlay.IsVisible = true;

                var wc = new WCObject(GlobalVariable.Init.rest);
                //TODO only Fetch once
               var networkPromise = SuppliersVariables.Init.Tags = await wc.Tag.GetAll(new Dictionary<string, string>
                {
                    {"per_page", "99"}
                });



            productsListView.FlowItemsSource =  SuppliersVariables.Init.Tags;

                LoadingOverlay.IsVisible = false;
                foreach (var x in SuppliersVariables.Init.Tags) {

                    var y = x.description;
                    Console.WriteLine(y);

                }



               


                TaskLoader.IsRunning = false;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }


        private async void SupplierClicked(object sender, EventArgs args)
        {
            try
            {
                var btn = (Button) sender;
                var product = btn.BindingContext;

                SuppliersVariables.Init.TagId = product.ToString();

             
                await Navigation.PushAsync(new Products());
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (productsListView != null)
            {
                productsListView.BeginRefresh();


                if (string.IsNullOrWhiteSpace(e.NewTextValue))
                    productsListView.FlowItemsSource = SuppliersVariables.Init.Tags;
                else
                    productsListView.FlowItemsSource = SuppliersVariables.Init.Tags
                        .Where(i => i.name.ToLower().Contains(e.NewTextValue)).ToList();


                productsListView.EndRefresh();
            }
            else
            {
                Application.Current.MainPage.DisplayAlert("Something went wrong", "We're still loading the suppliers",
                    "OK");
            }
        }

    }
}