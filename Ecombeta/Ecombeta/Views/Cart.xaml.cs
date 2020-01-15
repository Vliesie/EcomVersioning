using Ecombeta.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WooCommerceNET.WooCommerce.v3;
using WooCommerceNET.WooCommerce.v3.Extension;
using WooCommerceNET;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using WooCommerceNET.WooCommerce.v2;
using System.Collections.ObjectModel;

namespace Ecombeta.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Cart : ContentPage
    {
        List<OrderLineItem> Lineitems;
        List<Cartlist> z;
        ItemList items;
        bool NoMore;
        List<String> Productname;
        public int currentID;

        public Cart()
        {
            //TO DO add max stock Q for the cart
            InitializeComponent();

            Backgroundimage.BackgroundImageSource = "https://mm-app.co.za/wp-content/uploads/2019/12/OrangeBluepoly.jpg";
            var x = FullCart.Cartlistz;
            z = x;
            if (x == null)
            {
                NavigationPage.SetHasBackButton(this, false);
                Navigation.PushAsync(new CartEmprty());
            }
            items = new ItemList(FullCart.Cartlistz);
            cartView.ItemsSource = items.Items; 
        }
        private void Pricevalue_Clicked(object sender, EventArgs e)
        {

        }
        private void EvetClicked(object s, SelectedItemChangedEventArgs e)
        {
            var obj = (Cartlist)e.SelectedItem;
            var ide = Convert.ToInt32(obj.PId);

            foreach (var item in z)
            {
                if (ide == item.PId)
                {
                    currentID = item.PId;
                }
            }
        }  
         private void Removevalue_Clicked(object sender, EventArgs e)
         {
            int check;
            var btn = (ImageButton)sender;
            var item = btn.BindingContext;
            check = Convert.ToInt32(item);
          
            Cartlist listitem = (from itm in items.Items where itm.PId == check select itm).FirstOrDefault<Cartlist>();
            items.Items.Remove(listitem);
            Cartlist xf = (from itm in FullCart.Cartlistz where itm.PId == check select itm).FirstOrDefault<Cartlist>();
            FullCart.Cartlistz.Remove(xf);

          
        }
            
        private void stepper_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            foreach (var listitem in z)
            {
                if (currentID == listitem.PId)
                {
                    listitem.Totalprice = listitem.Price * Convert.ToDecimal(listitem.Pquantity);
                }
            }
        }

        private void UpdatePrice_Clicked(object sender, EventArgs e)
        {
            int check;
            var btn = (Button)sender;
            var a = btn.BindingContext;
            check = Convert.ToInt32(a);

            foreach (var item in z)
            {
                if (check == item.PId)
                {
                    item.Totalprice = item.Price * Convert.ToDecimal(item.Pquantity);
                }
            }
        }

        private async void ImageButton_Clicked(object sender, EventArgs e)
        {

            //You cant checkout if your not logged in There are no Guest Checkouts(I can But would rather not)
          if (Users.Loggedin == true)
          {

            if (Lineitems == null)
            {
                Lineitems = new List<OrderLineItem>();
            }

            RestAPI rest = new RestAPI("http://mm-app.co.za/wp-json/wc/v2/", "ck_a25f96835aabfc64b09613eb8ec4a8c9bcd5dcd0", "cs_8f247c22353f25b905c96171379b89714f8f4003");
            WooCommerceNET.WooCommerce.v2.WCObject wc = new WooCommerceNET.WooCommerce.v2.WCObject(rest);
            Check();
            var order = new WooCommerceNET.WooCommerce.v2.Order() { status = "on-hold", customer_id = Users.CId };
            foreach (var item in z)
            {
                var a = Convert.ToInt32(item.Pquantity);
                if (item.variation_id <= 0)
                {
                    item.variation_id = item.PId;
                }

                if (item.StockQuantity == 0)
                {
                    NoMore = true;
                    Productname.Add(item.Pname);
                }
                order.line_items = order.line_items ?? new List<OrderLineItem>();
                order.line_items.Add(new OrderLineItem() { product_id = item.PId, variation_id = item.variation_id, quantity = a });
            }

                if (NoMore)
                {
                    var yx = await DisplayAlert("Order Cant be Placed", $"Not enough stock for {Productname}", "Back to Cart", "Home");
                    if (yx)
                    {

                    }
                    else
                    {
                        await Navigation.PushAsync(new Home("Mica Market"));
                    }
                }
                else
                {
                    if (items != null)
                    {
                        await wc.Order.Add(order);
                      
                        var masterDetailPage = new Home("");
                        masterDetailPage.Detail = new NavigationPage(new Checkedout());
                        Application.Current.MainPage = masterDetailPage;
                    }
                    else
                    {
                        var masterDetailPage = new Home("");
                        masterDetailPage.Detail = new NavigationPage(new CartEmprty());
                        Application.Current.MainPage = masterDetailPage;
                  
                    }
                }
                   
          }
          else
          {
                var y =  await DisplayAlert("Woops", "Please Login to check Out", "Login", "Home");
                if (y)
                {
                    
                    var masterDetailPage = new Home("");
                    masterDetailPage.Detail = new NavigationPage(new Login());
                    Application.Current.MainPage = masterDetailPage;
                }
                else
                {
                    await Navigation.PushAsync(new Home("Mica Market"));
                }
            }
        }

        //Checks for stock quantity for variable's and singles incase theres no more stocks left this needs IMMENSE testing.
        public async void Check()
        {
            RestAPI rest = new RestAPI("http://mm-app.co.za/wp-json/wc/v3/", "ck_a25f96835aabfc64b09613eb8ec4a8c9bcd5dcd0", "cs_8f247c22353f25b905c96171379b89714f8f4003");
            WooCommerceNET.WooCommerce.v3.WCObject wc = new WooCommerceNET.WooCommerce.v3.WCObject(rest);
            foreach (var item in z)
            {
                var a = await wc.Product.Get(item.PId);
                //Variable
                var y = await wc.Product.Variations.GetAll(item.PId);

                if (a.variations == null)
                {
                    if (item.StockQuantity == Convert.ToInt32(a.stock_quantity))
                    {
                        if (a.stock_quantity == 0 || item.Pquantity > a.stock_quantity)
                        {
                            var yx = await DisplayAlert("Not Enough Products", $"There is this much Stock left:{a.stock_quantity}for{a.name}", "Back to Cart", "Keep Shopping");
                            if (yx)
                            {
                                item.StockQuantity = Convert.ToInt32(a.stock_quantity);
                                items = new ItemList(FullCart.Cartlistz);
                                cartView.ItemsSource = items.Items;
                            }
                            else
                            {
                                await Navigation.PushAsync(new Home("Mica Market"));
                            }
                        }
                    } //Is Single
                }
                if (Orders.isUnlimted)
                {
                  var x = item.StockQuantity - 1;
                    item.StockQuantity = x;
                }
                else
                {
                    foreach (var itemz in y)
                    {
                        if (itemz.stock_quantity == 0 || item.Pquantity > itemz.stock_quantity)
                        {
                            var yx = await DisplayAlert("Not Enough Products", $"There is this much Stock left:{a.stock_quantity}for{itemz.id}", "Back to Cart", "Home");
                            if (yx)
                            {
                                item.StockQuantity = Convert.ToInt32(itemz.stock_quantity);
                                items = new ItemList(FullCart.Cartlistz);
                                cartView.ItemsSource = items.Items;
                            }
                            else
                            {
                                await Navigation.PushAsync(new Home("Mica Market"));
                            }
                        }
                        if (Orders.isUnlimted)
                        {
                            var x = itemz.stock_quantity - 1;
                            itemz.stock_quantity = x;
                        }
                    }
                }
            }
        }

        //Checkout Button 
        private void Button_Clicked(object sender, EventArgs e)
        {
            var masterDetailPage = new Home("");

            masterDetailPage.Detail = new NavigationPage(new Suppliers());
            Application.Current.MainPage = masterDetailPage;
        }
    }
}