using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Ecombeta.Models;
using Ecombeta.Services;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Services;
using WooCommerceNET;
using WooCommerceNET.WooCommerce.v2;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Product = WooCommerceNET.WooCommerce.v3.Product;
using Variation = WooCommerceNET.WooCommerce.v3.Variation;

namespace Ecombeta.Views
{

    public partial class ProductReports {
        public string ProductName { get; set; }
        public string StockQuantity { get; set; }
    }

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Cart : ContentPage, INotifyPropertyChanged
    {

        public WooCommerceNET.WooCommerce.v3.WCObject wcV3;

        public WooCommerceNET.WooCommerce.v2.WCObject wcV2;

        private readonly List<CartList> _simpleCartlist;
        private int _currentId;
        private int _currentListItem;
        private ItemList _items;
        private List<OrderLineItem> _orderlineitems;
        private bool _productBoughtOut;
        private List<string> _productnames;
        private Product _singleProduct;
        private bool _spamClick;
        private Variation _varProduct;
        public List<ProductReports> proReport;

        public Order order;
        private bool _loading;

        public bool loading
        {
            get => _loading;
            set
            {
                if (_loading == value) return;
                _loading = value;
                RaisePropertyChanged();

            }
        }


        private bool _running;

        public bool running
        {
            get => _running;
            set
            {
                if (_running == value) return;
                _running = value;
                RaisePropertyChanged();

            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        private void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            try
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        public Cart()
        {
            try
            {
                var x = FullCart.CartList;
                //TO DO add max stock Q for the cart
                InitializeComponent();
                if (x?.Any() != true || x.Any() != true || !x.Any())
                {
                    var CartP = new CartPersistance();
                    var fetchedCart = Preferences.Get("Cart", "def2ault_value");
                    CartP.DePersist(fetchedCart);
                    _items = new ItemList(FullCart.CartList);
                    cartView.ItemsSource = _items.Items;
                 
                    _simpleCartlist = FullCart.CartList;
                    if (!fetchedCart.Any())
                    {
                        NavigationPage.SetHasBackButton(this, false);
                        Navigation.PushAsync(new CartEmprty());
                    }
                }
                else
                {
                    var fetchedCart = Preferences.Get("Cart", "default_value");
                    FullCart.CartList = JsonConvert.DeserializeObject<List<CartList>>(fetchedCart);
                    //var y = JsonConvert.DeserializeObject<List<CartList>>(fetchedCart);
                    //_items = new ItemList(y);
                    _items = new ItemList(FullCart.CartList);
                    cartView.ItemsSource = _items.Items;
                
                  
                    _simpleCartlist = FullCart.CartList;
                }
                Init();
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async Task Init()
        {
            wcV3 = new WooCommerceNET.WooCommerce.v3.WCObject(GlobalVariable.Init.rest);
            RestAPI rest = new RestAPI("http://mm-app.co.za/wp-json/wc/v2/",
           "ck_a25f96835aabfc64b09613eb8ec4a8c9bcd5dcd0", "cs_8f247c22353f25b905c96171379b89714f8f4003");
            wcV2 = new WCObject(rest);
            _spamClick = false;
           // Backgroundimage.BackgroundImageSource =
                //"https://mm-app.co.za/wp-content/uploads/2019/12/OrangeBluepoly.jpg";
        }

        private void Pricevalue_Clicked(object sender, EventArgs e)
        {
        }

        private void EvetClicked(object s, SelectedItemChangedEventArgs e)
        {
            try
            {
                var obj = (CartList) e.SelectedItem;
                var ide = Convert.ToInt32(obj.PId);

                foreach (var item in _items.Items)
                    if (ide == item.PId)
                    {
                        _currentId = item.PId;
                        item.TotalDynamicPrice = item.Price * Convert.ToDecimal(item.ProductQuantity);
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

                var listitem = (from itm in _items.Items where itm.PId == check select itm).FirstOrDefault();
                _items.Items.Remove(listitem);
                var xf = (from itm in FullCart.CartList where itm.PId == check select itm).FirstOrDefault();
                FullCart.CartList.Remove(xf);
                cartView.BeginRefresh();

                cartView.EndRefresh();
                 var jsonStringz = JsonConvert.SerializeObject(FullCart.CartList);

                Preferences.Set("Cart", jsonStringz);
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

                foreach (var listitem in _items.Items)
                    if (varid != 0)
                    {
                        if (varcheck == listitem.VariationId)
                        {
                            if (listitem.IncrementQ == 0 || listitem.IncrementQ.ToString() == "") listitem.IncrementQ = 1;
                            listitem.TotalDynamicPrice = listitem.Price * Convert.ToDecimal(listitem.ProductQuantity);
                            if (check != 0)
                            {
                                listitem.ProductQuantity = check;

                                var jsonStringz = JsonConvert.SerializeObject(FullCart.CartList);

                                Preferences.Set("Cart", jsonStringz);
                            }
                        }
                    }
                    else
                    {
                        if (idcheck == listitem.PId)
                        {
                            if (listitem.IncrementQ == 0 || listitem.IncrementQ.ToString() == "") listitem.IncrementQ = 1;
                            listitem.TotalDynamicPrice = listitem.Price * Convert.ToDecimal(listitem.ProductQuantity);
                            if (check != 0)
                            {
                                listitem.ProductQuantity = check;

                                var jsonStringz = JsonConvert.SerializeObject(FullCart.CartList);

                                Preferences.Set("Cart", jsonStringz);
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

                foreach (var item in _simpleCartlist)
                    if (check == item.PId)
                        item.TotalDynamicPrice = item.Price * Convert.ToDecimal(item.ProductQuantity);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }
      
        private async void CheckoutButton_Clicked(object sender, EventArgs e)
        {
            try
            {
   
                if (FullCart.CartList == null || !FullCart.CartList.Any())
                {
                    var yx = await DisplayAlert("Whoops",
                                "Cart seem's to be empty, We cant checkout nothing", "Back to Cart", "Supplier");
                    if (yx)
                    {
                        return;
                    }
                    else
                    {
                        var masterDetailPage = new Home();
                        masterDetailPage.Detail = new NavigationPage(new Suppliers());
                        Application.Current.MainPage = masterDetailPage;
                    }
                    return;
                }

                await Navigation.PushPopupAsync(new LoadingPopup());

                await BeginCheckout();

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async Task BeginCheckout()
        {
            try
            {

             
                if (_spamClick)
                {
                    await DisplayAlert("Woops", "Your trying to order twice", "Ok");
                    return;
                }

                if (!Users.LoggedIn)
                {
                       
                    await DisplayAlert("Woops", "Please Login to check Out", "Login", "Home");
                    return;
                }

                // this is okay
                if (_orderlineitems == null) { _orderlineitems = new List<OrderLineItem>(); }

                foreach (var cartItem in _simpleCartlist)
                {
                    if (cartItem.LimitedStock)
                    {
                        await IsInStock();
                    }
                    else
                    {
                        cartItem.InStock = true;
                    
                    }
                }
                order = new Order { status = "on-hold", customer_id = Users.CId };
                foreach (var item in _simpleCartlist)
                {
                    //if out of stock
                    if (item.StockStatus != "instock" || item.InStock == false) {
                        proReport = new List<ProductReports>();
                       proReport.Add(new ProductReports
                       {
                            ProductName = item.ProductName,
                            StockQuantity = item.StockQuantity.ToString()
                        });
                        continue;
                    } // skip this and go to the next item

               
                    if (item.VariationId <= 0) item.VariationId = item.PId;
                    if (item.StockQuantity == 0)
                    {
                        _productBoughtOut = true;
                        _productnames.Add(item.ProductName);
                    }
                    double quantity = item.ProductQuantity;
                    order.line_items = order.line_items ?? new List<OrderLineItem>();
                    order.line_items.Add(new OrderLineItem
                    { product_id = item.PId, variation_id = item.VariationId, quantity = Convert.ToDecimal(quantity) });
                }

                foreach (var Cartitem in _simpleCartlist)
                {
                    if (!Cartitem.InStock)
                    {
                        await Failed();
                        return;
                    }
                    else
                    {
                        continue;
                    }
                }await Succeed();

             

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }


        private async Task Succeed()
        {
            _spamClick = true;
             wcV2.Order.Add(order);
           Preferences.Clear("Cart");
           Preferences.Remove("Cart");
           _simpleCartlist.Clear();
            FullCart.CartList.Clear();
            _items.Items.Clear();
             
            await Navigation.PopPopupAsync();
            await DisplayAlert("Success", $"Order has been Placed, Thank you!", "Ok");
        }

        private async Task Failed()
        {
            await Navigation.PopPopupAsync();
            await Navigation.PushPopupAsync(new ProductReport(proReport));
            proReport.Clear();
        }

        protected async override void OnAppearing()
        {

         
        }

        private async Task IsInStock()
        {
            if (_simpleCartlist.Any())
            {
                foreach (var CartItem in _simpleCartlist)
                {
                    if (CartItem.VariantParentId == 0)
                    {
                        _singleProduct = await wcV3.Product.Get(CartItem.PId);
                        await SingleCheck(CartItem.PId);
                    }
                    else
                    {
                        _varProduct = await wcV3.Product.Variations.Get(CartItem.VariationId, CartItem.VariantParentId);
                        await VariableCheck(CartItem.PId);
                    }
                }

            }

        }


        private async Task SingleCheck(int id)
        {
            try
            {
           
                foreach (var CartItem in _simpleCartlist)
                {
                    if (id == CartItem.PId)
                    {
                        if (_singleProduct.stock_quantity == null)
                        {
                            //CartItem.InStock = true;
                            _singleProduct.stock_quantity = 999999;
                            CartItem.InStock = true;
                            continue;
                        }
                        if (CartItem.ProductQuantity == 0)
                        {
                            CartItem.ErrorMsg = "Item requires a Quantity of atleast 1" + System.Environment.NewLine + "Quantity Cannot be 0!";
                            continue;
                        }
                        if (_singleProduct.stock_quantity == 0 || _singleProduct.stock_status == "outofstock" ||
                            CartItem.ProductQuantity > _singleProduct.stock_quantity)
                        {

                          
                            CartItem.ErrorMsg = "Product shortage " + $" There is this much Stock left: {_singleProduct.stock_quantity} for {_singleProduct.name}";

                            CartItem.InStock = false;
                            CartItem.StockQuantity = Convert.ToInt32(_singleProduct.stock_quantity);
                            _items = new ItemList(FullCart.CartList);
                            CartItem.StockStatus = _singleProduct.stock_status;
                            continue;
                        }
                        CartItem.ErrorMsg = "";
                        CartItem.InStock = true;
                    }
                }
            
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async Task VariableCheck(int id)
        {
            try
            {
                foreach (var CartItem in _simpleCartlist)
                {
                    if (id == CartItem.PId)
                    {
                        if (_varProduct.stock_quantity == null)
                        {
                            _varProduct.stock_quantity = 999999;
                        }
                        if (CartItem.ProductQuantity == 0)
                        {
                            CartItem.ErrorMsg = "Item requires a Quantity of atleast 1" + System.Environment.NewLine + "Quantity Cannot be 0!";
                            continue;
                        }
                        if (_varProduct.stock_quantity == 0 || _varProduct.stock_status == "outofstock" ||
                            CartItem.ProductQuantity > _varProduct.stock_quantity)
                        {
                            CartItem.InStock = false;
                            CartItem.StockQuantity = Convert.ToInt32(_varProduct.stock_quantity);
                            _items = new ItemList(FullCart.CartList);
                            cartView.BeginRefresh();
                            CartItem.StockStatus = _varProduct.stock_status;
                            cartView.EndRefresh();
                            continue;
                        }
                        CartItem.ErrorMsg = "";
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

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            Preferences.Remove("Cart");
            Preferences.Clear("Cart");
        }

        
    }
}