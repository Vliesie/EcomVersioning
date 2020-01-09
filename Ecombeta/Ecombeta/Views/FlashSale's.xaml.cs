﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WooCommerceNET.WooCommerce.v3;
using WooCommerceNET.WooCommerce.v3.Extension;
using WooCommerceNET;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Ecombeta.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FlashSale_s : ContentPage
    {
        public FlashSale_s(string x)
        {
            InitializeComponent();
            InitAsync(x);
           
        }

        public async void InitAsync(string z)
        {
            RestAPI rest = new RestAPI("http://mm-app.co.za/wp-json/wc/v3/", "ck_a25f96835aabfc64b09613eb8ec4a8c9bcd5dcd0", "cs_8f247c22353f25b905c96171379b89714f8f4003");
            WCObject wc = new WCObject(rest);

            var p = await wc.Product.GetAll(new Dictionary<string, string>() {
                    {"tag", z },
                    { "per_page", "80" } }); 


            productsListView.FlowItemsSource = p;
        }
        async void ProductClicked(object sender, EventArgs args)
        {

            var btn = (Button)sender;

            var a = btn.BindingContext;

            Orders.singleID = Convert.ToInt32(a);
            await Navigation.PushAsync(new Orders());

            // var z = p.type;
            // bool checkSimple = bool.Parse(z);

            // if (checkSimple = checkSimple2)
            //{
            //  productsListView.ItemsSource = new Product[1] { p };
            //}

            //if (p.grouped_products != null)
            //{

            //  var a = await wc.Product.GetAll();
            // variablelistview.ItemsSource = a;
            //}
        }
    }
}