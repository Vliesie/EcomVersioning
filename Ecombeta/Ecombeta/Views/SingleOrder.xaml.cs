using System;
using Ecombeta.Models;
using Microsoft.AppCenter.Crashes;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Ecombeta.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SingleOrder : ContentPage
    {
        public static int PassOid;

        public SingleOrder()
        {
            InitializeComponent();
            try
            {
                Backimage.BackgroundImageSource = "https://mm-app.co.za/wp-content/uploads/2019/12/Bluepoly.jpg";
                Init();
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void Init()
        {
            try
            {
                var wc = new WooCommerceNET.WooCommerce.v3.WCObject(GlobalVariable.Init.rest);
                var p = await wc.Order.Get(PassOid);
                SingleOrderList.ItemsSource = new[] {p};

                Lineorders.ItemsSource = p.line_items;
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