using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecombeta.Models;
using Microsoft.AppCenter.Crashes;
using WooCommerceNET.WooCommerce.v2;
using Xamarin.Forms;
using Xamarin.Essentials;
using Xamarin.Forms.Xaml;
using Product = WooCommerceNET.WooCommerce.v3.Product;
using Variation = WooCommerceNET.WooCommerce.v3.Variation;
using WooCommerceNET;
using Ecombeta.Services;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;

namespace Ecombeta.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Flashcart : ContentPage
    {
        private List<FlashCartlist> FlashCart;
        private int currentID;
        private FlashItemList items;
        private List<OrderLineItem> Lineitems;
        private bool NoMore;
        private List<string> Productname;
        private FlashItemList _items;
        public string tempStatus;
        private Product SingleProduct;
        private bool spam;
        private Variation VarProduct;


        public Flashcart()
        {
            //TO DO add max stock Q for the cart
            InitializeComponent();


            Init();
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

        public async Task Init()
        {
            try
            {
                var rest = new RestAPI("http://mm-app.co.za/wp-json/wc/v3/",
                   "ck_a25f96835aabfc64b09613eb8ec4a8c9bcd5dcd0", "cs_8f247c22353f25b905c96171379b89714f8f4003");
                var wc = new WooCommerceNET.WooCommerce.v3.WCObject(GlobalVariable.Init.rest);

                var product = await wc.Product.GetAll(new Dictionary<string, string>
                {
                    {"tag", "1486"},
                    {"per_page", "80"}
                });
                var x = FlashFullCart.CartList;

                if (x?.Any() != true || x.Any() != true || !x.Any())
                {
                    var CartP = new CartPersistance();
                    var fetchedCart = Preferences.Get("FlashCart", "default_value");
                    CartP.DePersist(fetchedCart);
                    _items = new FlashItemList(FlashFullCart.CartList);
                    if (_items.FlashItems != null)
                    {
                        cartView.ItemsSource = _items.FlashItems;
                    }
                    if (!fetchedCart.Any())
                    {
                        NavigationPage.SetHasBackButton(this, false);
                        await Navigation.PushAsync(new CartEmprty());
                    }
                }
                else
                {
                    var fetchedCart = Preferences.Get("FlashCart", "default_value");
                    FlashFullCart.CartList = JsonConvert.DeserializeObject<List<FlashCartlist>>(fetchedCart);
                    var y = JsonConvert.DeserializeObject<List<FlashCartlist>>(fetchedCart);
                    _items = new FlashItemList(y);
                    if (_items.FlashItems != null)
                    {
                        var myItems = _items.FlashItems
   .Join(product.Where(i => i.status != "publish"),
     f => f.PId,
     i => i.id,
     (f, i) => (f, i));

                        foreach ((FlashCartlist f, Product i) in myItems)
                        {
                            f.status = i.status;

                            await DisplayAlert("Sale Over!", $"Sorry the Sale for {i.name} has ended, Removing Item from you cart", "Ok");
                        }
                        //foreach (var Itemz in _items.FlashItems)
                        //    {  
                            
                        //        foreach (var item in product)
                        //        {
                        //            if (Itemz.PId == item.id && item.status != "publish")
                        //            {
                        //            //Remove Drafted Products ECT
                           
                        //                await DisplayAlert("Sale Over!", $"Sorry the Sale for {SingleProduct.name} has ended, Removing Item from you cart", "Ok");
                        //                Itemz.status = item.status;
                                
                        //            }
                        //        }
                        //    }
                       
                     
                        cartView.ItemsSource = _items.FlashItems.Where(z => z.status == "publish").ToList();

                    }

                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private void Pricevalue_Clicked(object sender, EventArgs e)
        {
        }

        private void EvetClicked(object s, SelectedItemChangedEventArgs e)
        {

            try
            {
                var obj = (CartList)e.SelectedItem;
                var ide = Convert.ToInt32(obj.PId);

                foreach (var item in _items.FlashItems)
                    if (ide == item.PId)
                    {
                        currentID = item.PId;
                        item.TotalPrice = item.Price * Convert.ToDecimal(item.ProductQuantity);
                    }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
           
        }

        private void Removevalue_Clicked(object sender, EventArgs e)
        {
            try
            {
             

                int check;
                var btn = (ImageButton) sender;
                var item = btn.BindingContext;
                check = Convert.ToInt32(item);

                var listitem = (from itm in _items.FlashItems where itm.PId == check select itm).FirstOrDefault();

                _items.FlashItems.Remove(listitem);
                var xf = (from itm in FlashFullCart.CartList where itm.PId == check select itm).FirstOrDefault();
                FlashFullCart.CartList.Remove(xf);

                cartView.BeginRefresh();
                cartView.ItemsSource = _items.FlashItems;
                cartView.EndRefresh();
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private void stepper_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            try
            {
                int check;
                int varcheck;
                int idcheck;
                var btn = (Stepper)sender;
                var Quan = btn.Value;

                var varid = btn.TabIndex;
                var id = btn.ClassId;
                idcheck = Convert.ToInt32(id);
                check = Convert.ToInt32(Quan);
                varcheck = varid;

                foreach (var listitem in _items.FlashItems)
                    if (varid != 0)
                    {
                        if (varcheck == listitem.VariationId)
                        {
                            if (listitem.IncrementQ == 0 || listitem.IncrementQ.ToString() == "") listitem.IncrementQ = 1;
                            listitem.TotalPrice = listitem.Price * Convert.ToDecimal(listitem.ProductQuantity);
                            if (check != 0)
                            {
                                listitem.ProductQuantity = check;
                            }

                        }
                    }
                    else
                    {
                        if (idcheck == listitem.PId)
                        {
                            if (listitem.IncrementQ == 0 || listitem.IncrementQ.ToString() == "") listitem.IncrementQ = 1;
                            listitem.TotalPrice = listitem.Price * Convert.ToDecimal(listitem.ProductQuantity);
                            if (check != 0)
                            {
                                listitem.ProductQuantity = check;
                            }

                        }
                    }

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private void UpdatePrice_Clicked(object sender, EventArgs e)
        {
            try
            {
                int check;
                var btn = (Button) sender;
                var a = btn.BindingContext;
                check = Convert.ToInt32(a);

                foreach (var item in FlashCart)
                    if (check == item.PId)
                        item.TotalPrice = item.Price * Convert.ToDecimal(item.ProductQuantity);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void ImageButton_Clicked(object sender, EventArgs e)
        {

            try
            {

                await Navigation.PushPopupAsync(new LoadingPopup());

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }

            if (_items.FlashItems == null || _items.FlashItems.Count <= 0)
            {
                var yx = await DisplayAlert("Whoops",
                            "Cart seem's to be empty, We cant checkout nothing", "Back to Cart", "Supplier");
                if (yx)
                {
                }
                else
                {
                    var masterDetailPage = new Home();
                    masterDetailPage.Detail = new NavigationPage(new Suppliers());
                    Application.Current.MainPage = masterDetailPage;
                }
            }
            else
            {
               
                await BeginCheckout();
            }


            try
            {
                await Navigation.PopPopupAsync();

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }
        public async Task BeginCheckout()
        {
            if (_items.FlashItems == null || _items.FlashItems.Count <= 0)
            {
                var yx = await DisplayAlert("Whoops",
                            "Cart seem's to be empty, We cant checkout nothing", "Back to Cart", "Supplier");
                if (yx)
                {
                }
                else
                {
                    var masterDetailPage = new Home();
                    masterDetailPage.Detail = new NavigationPage(new Suppliers());
                    Application.Current.MainPage = masterDetailPage;
                }
            }
            else
            {
                FlashCart = FlashFullCart.CartList;
                try
                {
                    //You cant checkout if your not logged in There are no Guest Checkouts(I can But would rather not)
                    if (Users.LoggedIn && spam == false)
                    {
                        if (Lineitems == null || Lineitems.Count <= 0) Lineitems = new List<OrderLineItem>();
                        RestAPI rest = new RestAPI("http://mm-app.co.za/wp-json/wc/v2/",
                 "ck_a25f96835aabfc64b09613eb8ec4a8c9bcd5dcd0", "cs_8f247c22353f25b905c96171379b89714f8f4003");
                        var wc = new WCObject(rest);
                        ///////////////////
                        await IsInStock();
                        ///////////////////

                        var order = new Order { status = "on-hold", customer_id = Users.CId };
                        foreach (var item in FlashCart)
                            if (FlashCart.Any(i => i.InStock == false))
                            {
                                //var yx = await DisplayAlert("Whoops", "One or more Item is out of Stock Please check and try again", "Back to Cart", "Home");
                                //if (yx)
                                //{

                                //}
                                //else
                                //{
                                //    var masterDetailPage = new Home();
                                //    masterDetailPage.Detail = new NavigationPage(new Home());
                                //    Application.Current.MainPage = masterDetailPage;
                                //}
                            }
                            else if (FlashCart.All(i => i.InStock))
                            {
                                if (item.StockStatus == "instock" || item.StockStatus == null)
                                {
                                    var a = Convert.ToInt32(item.ProductQuantity);
                                    if (item.VariationId <= 0) item.VariationId = item.PId;

                                    if (item.StockQuantity == 0)
                                    {
                                        NoMore = true;
                                        Productname.Add(item.ProductName);
                                    }

                                    order.line_items = order.line_items ?? new List<OrderLineItem>();
                                    order.line_items.Add(new OrderLineItem
                                    { product_id = item.PId, variation_id = item.VariationId, quantity = a });
                                }
                                else
                                {
                                    var yx = await DisplayAlert("Order Cant be Placed",
                                        $"{item.ProductName} is out of stock",
                                        "Back to Cart", "Home");
                                    if (yx)
                                    {
                                    }
                                    else
                                    {
                                        var masterDetailPage = new Home();
                                        masterDetailPage.Detail = new NavigationPage(new Home());
                                        Application.Current.MainPage = masterDetailPage;
                                    }
                                }
                            }

                        if (FlashCart.All(i => i.InStock))
                        {
                            if (_items.FlashItems != null && spam == false)
                            {
                                spam = true;
                                await wc.Order.Add(order);
                             
                                FlashFullCart.CartList.Clear();
                                _items.FlashItems.Clear();
                                var masterDetailPage = new Home();
                                masterDetailPage.Detail = new NavigationPage(new Checkedout());
                                Application.Current.MainPage = masterDetailPage;
                            }
                            else
                            {
                                var masterDetailPage = new Home();
                                masterDetailPage.Detail = new NavigationPage(new CartEmprty());
                                Application.Current.MainPage = masterDetailPage;
                            }
                        }
                    }
                    else
                    {
                        if (spam)
                        {
                            await DisplayAlert("Woops", "Your trying to order twice", "Ok");
                        }
                        else
                        {
                            var y = await DisplayAlert("Woops", "Please Login to check Out", "Login", "Home");
                            if (y)
                            {
                                var masterDetailPage = new Home();
                                masterDetailPage.Detail = new NavigationPage(new Login());
                                Application.Current.MainPage = masterDetailPage;
                            }
                            else
                            {
                                await Navigation.PushAsync(new Home());
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }
            }
        }
           

        private async Task IsInStock()
        {
  

            foreach (var CartItem in FlashCart)
            {
                var wc = new WooCommerceNET.WooCommerce.v3.WCObject(GlobalVariable.Init.rest);

                SingleProduct = await wc.Product.Get(CartItem.PId);

                VarProduct = await wc.Product.Variations.Get(CartItem.VariationId, CartItem.VariantParentId);

                if (SingleProduct != null)
                    await SingleCheck(CartItem.PId);
                else if (VarProduct != null) await VariableCheck(CartItem.PId);
            }

       
        }


        private async Task SingleCheck(int CurrentListItem)
        {
            try
            {
                foreach (var CartItem in FlashCart)
                    if (CurrentListItem == CartItem.PId)
                    {
                        if (SingleProduct.status != "publish")
                        {
                            //Remove Drafted Products ECT
                            CartItem.status = SingleProduct.status;
                            await DisplayAlert("Sale Over!", $"Sorry the Sale for {SingleProduct.name} has ended, Removing Item from you cart", "Ok");
                            var listitem = (from itm in _items.FlashItems where itm.PId == CurrentListItem select itm).FirstOrDefault();
                            _items.FlashItems.Remove(listitem);
                            var xf = (from itm in FlashFullCart.CartList where itm.PId == CurrentListItem select itm).FirstOrDefault();
                            FlashFullCart.CartList.Remove(xf);

                        }
                        if (SingleProduct.stock_quantity == 0 || SingleProduct.stock_status == "outofstock" ||
                            CartItem.ProductQuantity > SingleProduct.stock_quantity)
                        {
                            CartItem.InStock = false;
                            var AlertResult = await DisplayAlert("Not Enough Products",
                                $"There is this much Stock left: {SingleProduct.stock_quantity} for {SingleProduct.name}",
                                "Back to Cart", "Keep Shopping");
                            if (AlertResult)
                            {
                                CartItem.InStock = false;
                                CartItem.StockQuantity = Convert.ToInt32(SingleProduct.stock_quantity);
                                items = new FlashItemList(FlashFullCart.CartList);
                                cartView.BeginRefresh();
                                CartItem.StockStatus = SingleProduct.stock_status;
                                cartView.EndRefresh();
                                return;
                            }

                            var masterDetailPage = new Home();
                            masterDetailPage.Detail = new NavigationPage(new Home());
                            Application.Current.MainPage = masterDetailPage;
                        }
                        else if (SingleProduct.stock_quantity == null)
                        {
                            SingleProduct.stock_quantity = 999999;
                        }
                        if (SingleProduct.stock_quantity != 0 && SingleProduct.stock_status == "instock" &&
                                 CartItem.ProductQuantity <= SingleProduct.stock_quantity)
                        {
                            CartItem.InStock = true;
                        }
                    }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async Task VariableCheck(int CurrentListItem)
        {
            try
            {
                foreach (var CartItem in FlashCart)
                    if (CurrentListItem == CartItem.PId)
                    {
                        if (VarProduct.status != "publish")
                        {
                            //Remove Drafted Products ECT
                            CartItem.status = SingleProduct.status;
                            await DisplayAlert("Sale Over!", $"Sorry the Sale for {CartItem.ProductName} has ended, Removing Item from you cart", "Ok");
                            var listitem = (from itm in _items.FlashItems where itm.PId == CurrentListItem select itm).FirstOrDefault();
                            _items.FlashItems.Remove(listitem);
                            var xf = (from itm in FlashFullCart.CartList where itm.PId == CurrentListItem select itm).FirstOrDefault();
                            FlashFullCart.CartList.Remove(xf); 
                        }
                        if (VarProduct.stock_quantity == 0 || VarProduct.stock_status == "outofstock" ||
                            CartItem.ProductQuantity > VarProduct.stock_quantity)
                        {
                            CartItem.InStock = false;
                            var AlertResult = await DisplayAlert("Not Enough Products",
                                $"There is this much Stock left: {VarProduct.stock_quantity} for {CartItem.ProductName} {VarProduct.attributes[2].option}",
                                "Back to Cart", "Keep Shopping");
                            if (AlertResult)
                            {
                                CartItem.InStock = false;
                                CartItem.StockQuantity = Convert.ToInt32(VarProduct.stock_quantity);
                                items = new FlashItemList(FlashFullCart.CartList);
                                cartView.BeginRefresh();
                                CartItem.StockStatus = VarProduct.stock_status;
                                cartView.EndRefresh();
                                return;
                            }

                            var masterDetailPage = new Home();
                            masterDetailPage.Detail = new NavigationPage(new Home());
                            Application.Current.MainPage = masterDetailPage;
                        }
                        if (VarProduct.stock_quantity == null)
                        {
                            VarProduct.stock_quantity = 999999;
                        }
                        else if (VarProduct.stock_quantity != 0 && VarProduct.stock_status == "instock" &&
                                 CartItem.ProductQuantity <= VarProduct.stock_quantity)
                        {
                            CartItem.InStock = true;
                        }
                    }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        
        //Checkout Button 
        private void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                var masterDetailPage = new Home();
                masterDetailPage.Detail = new NavigationPage(new Suppliers());
                Application.Current.MainPage = masterDetailPage;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }
    }
}