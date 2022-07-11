using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Ecombeta.Models;
using Ecombeta.Services;
using Microsoft.AppCenter.Crashes;
using WooCommerceNET.WooCommerce.v2;
using WooCommerceNET.WooCommerce.v3;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Ecombeta.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SingleProductView : ContentPage, INotifyPropertyChanged
    {
        public static WooCommerceNET.WooCommerce.v3.VariationImage Img;
        public string tempStatus;
        public static bool FlashSale;
        public string ImgSource;
        public List<CartList> OldList;
        public List<OrderLineItem> OrdersLine;
        public bool Saving;
        public WooCommerceNET.WooCommerce.v3.Product SProduct;
        public int TempId;
        public string TempIsAvb;
        public string TempStockStatus;
        public bool Unlimited;
        public bool hasQuantity;
        public List<WooCommerceNET.WooCommerce.v3.Variation> VarProduct;
        public string Titlez { get; set; }
        public string CustomEmail { set; get; }
        public int Index { get; set; }
        public bool hasInc;
        public decimal TemporaryPrice { get; set; }
        public object Increment { get; set; }
        public decimal PriceProcessing { get; set; }
        public static int SingleId { get; set; }
        public string Name { get; set; }
        public double TempStockQ { get; set; }
        public bool ProductE { get; set; }
        public int TempIncrementQ { get; set; }
        public int TempMinQ { get; set; }
        public string Time { get; set; }
        public int VarId { get; set; }
        public double DynamicPrice { get; set; }
        public decimal currency;
        public string TempCartAtribV1;
        public string TempCartAtribK1;
        public string TempCartAtribK2;
        public string TempCartAtribV2;
        public bool SetMinQ;
        public bool TempLimited;

        private double _ProductQuantity;

        public double ProductQuantity
        {
            get => _ProductQuantity;
            set
            {
                if (_ProductQuantity == value) return;
                _ProductQuantity = value;
                RaisePropertyChanged();
            }
        }

  
        string checkforeven;
        string  checkforeven2;
        public int check;
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

        public SingleProductView()
        {
            try
            {
                InitializeComponent();

                Pageback.BackgroundImageSource = "https://mm-app.co.za/wp-content/uploads/2019/12/Bluepoly.jpg";
                InitAsync();
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void Back_Clicked(object sender, EventArgs e)
        {
            ProductQuantity = 0;
            await Navigation.PopAsync();
        }

        private async Task InitAsync()
        {
            try
            {
               

                TaskLoader.IsRunning = true;
                LoadingOverlay.IsVisible = true;

                var wc = new WooCommerceNET.WooCommerce.v3.WCObject(GlobalVariable.Init.rest);
                Index = 0;
                ProductQuantity = 0;

                //Simple
                SProduct = await wc.Product.Get(SingleId);

                //Variable
                VarProduct = await wc.Product.Variations.GetAll(SingleId);
                if (VarProduct != null && (VarProduct != null) |
                    (VarProduct.Count != 0))
                    TempId = SingleId;

                var sThread = new Thread(StringReplace);
                sThread.Start();
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }



            try
            {

                if (SProduct.stock_quantity <= 0)
                {
                    TempStockQ = 2;
                    SProduct.stock_quantity = 2;
                    SProduct.meta_data[1].value = 1;
                    TempIsAvb = "Not in stock";
                }

                if (SProduct.stock_quantity == null)
                {
                    Unlimited = true;
                    //If Stock is null it means unlimited so 9999999 will be per purchase instance
                    const int i = 999999;
                    SProduct.stock_quantity = i;
                    TempIsAvb = "In Stock";
                }
                else
                {
                    SProduct._virtual = true;
                    TempStockQ = Convert.ToInt32(SProduct.stock_quantity);
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            //Variable Quantity Checks

            try
            {

                foreach (var item in SProduct.meta_data)
                {
                    if (item.key == "maximum_allowed_quantity")
                    {
                      
                        if (item.value is string stringValue && string.IsNullOrWhiteSpace(stringValue))
                        {
                            item.value = TempStockQ;
                        }
                    }
                    else if (item.key == "group_of_quantity")
                    {


                        if (item.value is string stringValue && string.IsNullOrWhiteSpace(stringValue) ||
                            item.value == null)
                        {
                            item.value = 1;
                            SProduct.download_limit = 1;
                        }
                        else
                        {
                            SProduct.download_limit = Convert.ToInt32(item.value);
                            //_ProductQuantity = Convert.ToDouble(item.value);
                            TempIncrementQ = Convert.ToInt32(item.value);
                        }

                    }
                    else if (item.key == "minimum_allowed_quantity")
                    {
                        if (item.value.ToString() == "1")
                        {
                            checkforeven = item.value.ToString();
                        }
                        if (item.value.ToString() is string stringValue && string.IsNullOrWhiteSpace(stringValue))
                        {
                            SProduct.download_expiry = 1;
                        }
                        else
                        {
                            SProduct.download_expiry = Convert.ToInt32(item.value);
                            TempMinQ = Convert.ToInt32(item.value);
                        }

                    }


                }

                if (checkforeven == "1" && SProduct.stock_quantity.ToString() == "1")
                {
                    SProduct.download_expiry = 0;
                    TempMinQ = 0;
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            try
            {
                foreach (var item in VarProduct)
                {
                    TempCartAtribK1 = item.attributes[0].name;
                    TempCartAtribV1 = item.attributes[0].option;
                    if (item.attributes.Count >= 2)
                    {
                        TempCartAtribK2 = item.attributes[1].name;
                        TempCartAtribV2 = item.attributes[1].option;
                    }


                 
                    //downloadlimit = 1 thing
                    foreach (var metaItem in item.meta_data)
                    {

                        if (metaItem.key == "group_of_quantity" || metaItem.key == "variation_group_of_quantity")
                        {
                            if (metaItem.value.ToString() is string stringCompareValue && string.IsNullOrWhiteSpace(stringCompareValue) || metaItem.value == null || Convert.ToInt32(metaItem.value) == 0)
                            {
                                item.download_limit = 1;

                                // ProductQuantity = 1;
                            }
                            else if (metaItem.value == null || Convert.ToInt32(metaItem.value) == 0 || metaItem.value.ToString() is string Check && !string.IsNullOrWhiteSpace(Check))
                            {
                                TempIncrementQ = Convert.ToInt32(metaItem.value);
                                item.download_limit = Convert.ToInt32(metaItem.value);
                                //ProductQuantity = Convert.ToDouble(metaItem.value);
                                if (SetMinQ)
                                {
                                    TempMinQ = TempIncrementQ;
                                }
                            }

                        }
                        else if (metaItem.key == "variation_minimum_allowed_quantity" || metaItem.key == "minimum_allowed_quantity")
                        {
                            if (metaItem.value.ToString() == "1")
                            {
                                checkforeven = metaItem.value.ToString();
                            }
                            if (metaItem.value.ToString() is string Checkmin && string.IsNullOrWhiteSpace(Checkmin) || metaItem.value == null)
                            {
                                metaItem.value = 1;
                                item.download_expiry = 1;
                                if (TempIncrementQ != 0)
                                {
                                    TempMinQ = TempIncrementQ;
                                }
                                else
                                {
                                    SetMinQ = true;
                                }

                            }
                          
                            else
                            {
                                item.download_expiry = Convert.ToInt32(metaItem.value);
                                TempMinQ = Convert.ToInt32(metaItem.value);
                            }
                        }
                        else if (metaItem.key == "variation_maximum_allowed_quantity" || metaItem.key == "maximum_allowed_quantity")
                        {
                           
                            if (metaItem.value.ToString() is string Checkmin && string.IsNullOrWhiteSpace(Checkmin))
                            {
                                item.stock_quantity = 99999999;
                            }
                            else if (metaItem.value == null)
                            {
                                item.stock_quantity = 99999999;
                            }
                            else
                            {
                                item.stock_quantity = Convert.ToInt32(metaItem.value);
                            }
                        }
                        if (checkforeven == "1" && item.stock_quantity.ToString() == "1")
                        {
                            item.download_expiry = 0;
                            TempMinQ = 0;
                        }

                    }

                    var StockQ = Convert.ToInt32(item.stock_quantity);
                    if (item.stock_quantity <= 0)
                    {
                        TempStockQ = 2;
                        item.stock_quantity = 2;
                        item.meta_data[1].value = 1;
                        TempIsAvb = "Not in stock";
                    }

                    if (item.stock_quantity == null)
                    {
                        Unlimited = true;
                        TempIsAvb = "In Stock";
                        //If Stock is null it means unlimited so 9999999 will be per purchase instance
                        var i = 999999;
                        item.stock_quantity = i;
                    }

                    //if (item.meta_data[2].value == null)
                    //{
                    //    var i = ProductQuantity;
                    //    item.meta_data[2].value = i;
                    //    TempIsAvb = "In Stock";
                    //}

                    //if (item.meta_data[2].value is string stringValue && string.IsNullOrWhiteSpace(stringValue))
                    //{
                    //    var i = ProductQuantity;
                    //    item.meta_data[2].value = i;
                    //}

                    //if (item.meta_data[2].value is int intValue && intValue <= 0)
                    //{
                    //    intValue = (int) item.meta_data[1].value;
                    //    item.meta_data[2].value = intValue;
                    //}

                    else
                    {
                        TempStockQ = Convert.ToInt32(item.stock_quantity);
                    }

                    var xxx = item.meta_data[2].value;
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }

            try
            {
                if (SProduct != null || VarProduct != null)
                {
                    if (SProduct.price_html != null)
                    {
                        SProduct.price_html =
                            "R" + SProduct.regular_price;
                        if (SProduct.regular_price != null)
                        {
                            SProduct.price_html =
                                "R" + SProduct.regular_price;
                            if (SProduct.sale_price != null)
                                SProduct.price_html =
                                    "From R" + SProduct.regular_price + " To " + "R" +
                                    SProduct.sale_price + "On Sale";
                        }
                    }

                    if (SProduct.type == "simple")
                    {
                        if (Users.LoggedIn == false)
                        {
                            SProduct.purchasable = false;
                            SProduct.price = 0;
                            SProduct.price_html = "Please Login to view the Prices";
                        }

                        SProduct.downloadable = true;
                        SProduct._virtual = false;
                        Title = "Single Product";
                        variablelistview.ItemsSource = new[] { SProduct };
                    }
                    else
                    {
                        foreach (var SProduct in VarProduct)
                        {
                            SProduct.downloadable = false;
                            SProduct._virtual = true;
                            //Can use SProduct instead of p[Index] Index just Increments and Then runs threw the loop and changes the details but SProduct is the easier way.
                        }

                        Index = 0;
                        if (Users.LoggedIn == false)
                            foreach (var item in VarProduct)
                            {
                                item.price = 0;
                                item.shipping_class_id = "Please Login to view the Prices";
                                // item.shipping_class_id = "From R" + item.regular_price.ToString() + " To " + "R" + item.sale_price.ToString() + "On Sale";
                            }

                        Title = "Multi Product"; //This Doest Work but w.e 
                        variablelistview.ItemsSource = VarProduct;
                    }
                }
                TaskLoader.IsRunning = false;
                LoadingOverlay.IsVisible = false;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }


        private void StepperValueChanged(object sender, ValueChangedEventArgs e)
        {
            ProductQuantity = e.NewValue;
        }


        private async void AddtoCart(object sender, EventArgs args)
        {
            try
            {
                var btn = (Button)sender;
                var a = btn.BindingContext;
                check = Convert.ToInt32(a);
               
                if (a == null) check = Convert.ToInt32(SProduct.id);
                if (SProduct != null)
                {


                    if (SProduct.id == check)
                    {
                        tempStatus = SProduct.status;
                        try
                        {

                            if (SProduct.stock_quantity <= 0)
                            {
                                TempStockQ = 2;
                                SProduct.stock_quantity = 2;
                                SProduct.meta_data[1].value = 1;
                                TempIsAvb = "Not in stock";
                            }

                            if (SProduct.stock_quantity == null)
                            {
                                Unlimited = true;
                                //If Stock is null it means unlimited so 9999999 will be per purchase instance
                                const int i = 999999;
                                SProduct.stock_quantity = i;
                                TempIsAvb = "In Stock";
                            }
                            else
                            {
                                SProduct._virtual = true;
                                TempStockQ = Convert.ToInt32(SProduct.stock_quantity);
                            }
                        }
                        catch (Exception ex)
                        {
                            Crashes.TrackError(ex);
                        }
                        //Variable Quantity Checks

                        try
                        {
                            if (SProduct.stock_status is string stringValuez && string.IsNullOrWhiteSpace(stringValuez) || SProduct.stock_status == "" || SProduct.stock_status == null)
                            {
                                TempStockStatus = "instock";
                                SProduct.stock_status = "instock";
                            }
                            else
                            {
                                TempStockStatus = SProduct.stock_status;
                            }
                            foreach (var item in SProduct.meta_data)
                            {

                                if (item.key == "maximum_allowed_quantity")
                                {
                                    if (item.value is string stringValue && string.IsNullOrWhiteSpace(stringValue))
                                    {
                                        item.value = TempStockQ;
                                    }
                                }
                                else if (item.key == "group_of_quantity")
                                {


                                    if (item.value is string stringValue && string.IsNullOrWhiteSpace(stringValue) ||
                                        item.value == null)
                                    {
                                        item.value = 1;
                                        SProduct.download_limit = 1;
                                    }
                                    else
                                    {
                                        SProduct.download_limit = Convert.ToInt32(item.value);
                                        //_ProductQuantity = Convert.ToDouble(item.value);
                                        TempIncrementQ = Convert.ToInt32(item.value);
                                    }

                                }
                                else if (item.key == "minimum_allowed_quantity")
                                {
                                    if (item.value.ToString() == "1")
                                    {
                                        checkforeven = item.value.ToString();
                                    }
                                   
                                    if (item.value.ToString() is string stringValue && string.IsNullOrWhiteSpace(stringValue))
                                    {
                                        SProduct.download_expiry = 1;
                                    }
                                    else
                                    {
                                        SProduct.download_expiry = Convert.ToInt32(item.value);
                                        TempMinQ = Convert.ToInt32(item.value);
                                    }

                                }

                            }
                            if (checkforeven == "1" && SProduct.stock_quantity.ToString() == "1")
                            {
                                SProduct.download_expiry = 0;
                                TempMinQ = 0;
                            }

                          
                        }

                        catch (Exception ex)
                        {
                            Crashes.TrackError(ex);
                        }
                    }
                    foreach (var Tag in SProduct.tags)
                    {
                        if (Tag.name == "Limited")
                        {
                            TempLimited = true;
                        }
                        else
                        {
                            TempLimited = false;
                        }

                    }
                }

                try
                {

                    foreach (var item in VarProduct)
                    {
                      
                        if (item.id == check)
                        {
                            tempStatus = item.status;
                            hasQuantity = false;
                            hasInc = false;
                            if (item.stock_status is string stringValuez && string.IsNullOrWhiteSpace(stringValuez) || item.stock_status == "" || item.stock_status == null)
                            {
                                TempStockStatus = "instock";
                                item.stock_status = "instock";
                            }
                            else
                            {
                                TempStockStatus = item.stock_status;
                            }

                            TempCartAtribK1 = item.attributes[0].name;
                            TempCartAtribV1 = item.attributes[0].option;
                            if (item.attributes.Count >= 2)
                            {
                                TempCartAtribK2 = item.attributes[1].name;
                                TempCartAtribV2 = item.attributes[1].option;
                            }

                            var mystring = "maximum_allowed_quantity";

                            //downloadlimit = 1 thing
                            foreach (var metaItem in item.meta_data)
                            {
                                if (!metaItem.key.Contains(mystring))
                                {
                                    item.stock_quantity = 99999999;
                                }


                                if (metaItem.key == "group_of_quantity" || metaItem.key == "variation_group_of_quantity")
                                {
                                    hasInc = true;
                                    if (metaItem.value.ToString() is string stringCompareValue && string.IsNullOrWhiteSpace(stringCompareValue) || metaItem.value == null || Convert.ToInt32(metaItem.value) == 0)
                                    {
                                        item.download_limit = 1;
                                        TempIncrementQ = 1;

                                    }
                                    else if (metaItem.value == null || Convert.ToInt32(metaItem.value) == 0 || metaItem.value.ToString() is string Check && !string.IsNullOrWhiteSpace(Check))
                                    {
                                        TempIncrementQ = Convert.ToInt32(metaItem.value);
                                        item.download_limit = Convert.ToInt32(metaItem.value);

                                        if (SetMinQ)
                                        {
                                            TempMinQ = TempIncrementQ;
                                        }
                                    }

                                }
                                else if (metaItem.key == "variation_minimum_allowed_quantity" || metaItem.key == "minimum_allowed_quantity")
                                {
                                    if (metaItem.value.ToString() == "1")
                                    {
                                        checkforeven = metaItem.value.ToString();
                                    }
                                    if (metaItem.value.ToString() is string Checkmin && string.IsNullOrWhiteSpace(Checkmin) || metaItem.value == null)
                                    {
                                        metaItem.value = 1;
                                        item.download_expiry = 1;
                                        if (TempIncrementQ != 0)
                                        {
                                            TempMinQ = TempIncrementQ;
                                        }
                                        else
                                        {
                                            SetMinQ = true;
                                        }

                                    }
                                    else
                                    {
                                        item.download_expiry = Convert.ToInt32(metaItem.value);
                                        TempMinQ = Convert.ToInt32(metaItem.value);
                                    }
                                }
                                else if (metaItem.key == "variation_maximum_allowed_quantity" || metaItem.key == "maximum_allowed_quantity")
                                {
                                    hasQuantity = true;
                                    if (metaItem.value.ToString() is string Checkmin && string.IsNullOrWhiteSpace(Checkmin))
                                    {
                                        item.stock_quantity = 99999999;
                                    }
                                    else if (metaItem.value == null)
                                    {
                                        item.stock_quantity = 99999999;
                                    }
                                    else
                                    {
                                        item.stock_quantity = Convert.ToInt32(metaItem.value);
                                    }
                                }

                            }
                            if (!hasQuantity)
                            {
                                item.stock_quantity = 99999999;
                                TempStockQ = 999999;
                            }
                            if (!hasInc)
                            {

                                TempIncrementQ = TempMinQ;
                            }

                            var StockQ = Convert.ToInt32(item.stock_quantity);
                            if (item.stock_quantity <= 0)
                            {
                                TempStockQ = 2;
                                item.stock_quantity = 2;
                                item.meta_data[1].value = 1;
                                TempIsAvb = "Not in stock";
                            }

                            if (item.stock_quantity == null)
                            {
                                Unlimited = true;
                                TempIsAvb = "In Stock";
                                //If Stock is null it means unlimited so 9999999 will be per purchase instance
                                var i = 999999;
                                item.stock_quantity = i;
                                TempStockQ = i;
                            }

                            
                            else
                            {
                                TempStockQ = Convert.ToInt32(item.stock_quantity);
                            }

                            var xxx = item.meta_data[2].value;
                        }
                     

                     
                        if (checkforeven == "1" && SProduct.stock_quantity.ToString() == "1")
                        {
                            SProduct.download_expiry = 0;
                            TempMinQ = 0;
                        }
                        TemporaryPrice = Convert.ToDecimal(item.price);
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }

                var j = TempMinQ;
                var h = TempIncrementQ;
                var b = ProductQuantity;
                Saving = false;
                if (TempIsAvb == "Not in stock")
                {
                    await DisplayAlert("Issue", "Product is out of stock", "Okay");
                    return;
                }

                foreach (var item in VarProduct)
                    if (TempIsAvb == "Not in stock")
                    {
                        await DisplayAlert("Issue", "Product is out of stock", "Okay");
                        return;
                    }

                PriceProcessing = SProduct.price == null
                    ? Convert.ToDecimal(SProduct.price_html)
                    : Convert.ToDecimal(SProduct.price);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }

            try
            {
                var listy = FullCart.CartList;
                var Flashlisty = FlashFullCart.CartList;


                if (SProduct.type != "simple")
                {
                    foreach (var cp in VarProduct)
                    {
                        TempStockStatus = cp.stock_status;
                        if (check == cp.id)
                        {
                            TempId = Convert.ToInt32(cp.id);
                            tempStatus = cp.status;

                            PriceProcessing = Convert.ToDecimal(cp.price);
                            ImgSource = cp.image.src;
                            if (SProduct.price == null)
                            {
                                TemporaryPrice = Convert.ToDecimal(cp.price);
                                PriceProcessing = Convert.ToDecimal(cp.price);
                            }
                        }
                    }
                }
                else
                {
                    //TempStockQ = Convert.ToInt32(SProduct.stock_quantity);
                    //TempStockStatus = SProduct.stock_status;
                    if (SProduct.price != 0 || SProduct.price != null)
                    {
                        TemporaryPrice =
                           Convert.ToDecimal(SProduct.price);
                    }
                    else
                    {
                        TemporaryPrice =
                            Convert.ToDecimal(SProduct.price);
                    }







                    ImgSource = SProduct.images[0].src;
                }


                if (OldList == null) OldList = new List<CartList>();
                 currency = PriceProcessing *
                               Convert.ToDecimal(ProductQuantity);
                var oldlitz = new List<CartList>();
                var Flasholdlitz = new List<FlashCartlist>();

                if (FlashSale && FlashFullCart.CartList != null)
                {
                    var flashfirstMatch = FlashFullCart.CartList.FirstOrDefault(i => i.PId == check);
                    var flashSecondMatch = FlashFullCart.CartList.FirstOrDefault(i => i.VariationId == check);

                    if (flashfirstMatch != null)
                    {
                        var y = await DisplayAlert("Product Already in Cart", "Change Quanity On Cart", "Go cart",
                            "Keep Shopping");
                        if (y)
                        {
                            var masterDetailPage = new Home();
                            masterDetailPage.Detail = new NavigationPage(new Flashcart());
                            Application.Current.MainPage = masterDetailPage;
                            y = false;
                            Saving = false;
                        }
                    }
                    else if (flashSecondMatch != null)
                    {
                        var y = await DisplayAlert("Product Already in Cart", "Change Quanity On Cart", "Go cart",
                            "Keep Shopping");
                        if (y)
                        {
                            var masterDetailPage = new Home { Detail = new NavigationPage(new Flashcart()) };
                            Application.Current.MainPage = masterDetailPage;
                            Saving = false;
                        }
                    }
                    else if (flashfirstMatch == null || flashSecondMatch == null)
                    {
                        if (FlashSale)
                        {
                            Flasholdlitz.Add(new FlashCartlist
                            {
                                status = tempStatus,
                                StockStatus = TempStockStatus,
                                QuantityCheck = TempIsAvb,
                                StockQuantity = Convert.ToInt32(TempStockQ),
                                Price = TemporaryPrice,
                                IncrementQ = TempIncrementQ,
                                MinQ = TempMinQ,
                                TotalPrice = currency,
                                ImgSource = ImgSource,
                                PId = Convert.ToInt32(SProduct.id),
                                ProductName = SProduct.name,
                                ProductQuantity = ProductQuantity,
                                VariationId = check,
                                VariantParentId = TempId
                            }) ;
                            Saving = true;
                            await DisplayAlert("Item Added to cart ", "Click again for a Shortcut to the Cart", "Okay");
                        }
                        else
                        {
                            oldlitz.Add(new CartList
                            {
                                StockStatus = TempStockStatus,
                                QuantityCheck = TempIsAvb,
                                StockQuantity = Convert.ToInt32(TempStockQ),
                                Price = TemporaryPrice,
                                IncrementQ = TempIncrementQ,
                                MinQ = TempMinQ,
                                TotalDynamicPrice = currency,
                                ImgSource = ImgSource,
                                PId = Convert.ToInt32(SProduct.id),
                                ProductName = SProduct.name,
                                ProductQuantity = ProductQuantity,
                                VariationId = check,
                                VariantParentId = TempId,
                                CartAtribKey = TempCartAtribK1,
                                CartAtribValue = TempCartAtribV1,
                                CartAtribKey1 = TempCartAtribK2,
                                CartAtribValue1 = TempCartAtribV2
                            });
                            ;
                            Saving = true;
                            await DisplayAlert("Item Added to cart ", "Click again for a Shortcut to the Cart", "Okay");
                        }
                    }
                }
                else if (FullCart.CartList != null)
                {
                    if (FlashSale)
                    {
                        var FlashfirstMatch = FlashFullCart.CartList.FirstOrDefault(i => i.PId == check);
                        var FlashSecondMatch = FlashFullCart.CartList.FirstOrDefault(i => i.VariationId == check);
                    }
                    else
                    {
                        var firstMatch = FullCart.CartList.FirstOrDefault(i => i.PId == check);
                        var SecondMatch = FullCart.CartList.FirstOrDefault(i => i.VariationId == check);


                        if (firstMatch != null)
                        {
                            var y = await DisplayAlert("Product Already in Cart", "Change Quanity On Cart", "Go cart",
                                "Keep Shopping");
                            if (y)
                            {
                                var masterDetailPage = new Home { Detail = new NavigationPage(new Cart()) };
                                Application.Current.MainPage = masterDetailPage;
                                y = false;
                                Saving = false;
                            }
                        }
                        else if (SecondMatch != null)
                        {
                            var y = await DisplayAlert("Product Already in Cart", "Change Quanity On Cart", "Go cart",
                                "Keep Shopping");
                            if (y)
                            {
                                var masterDetailPage = new Home { Detail = new NavigationPage(new Cart()) };
                                Application.Current.MainPage = masterDetailPage;
                                Saving = false;
                            }
                        }
                        else if (firstMatch == null || SecondMatch == null)
                        {
                            if (FlashSale)
                            {
                                Flasholdlitz.Add(new FlashCartlist
                                {
                                    status = tempStatus,
                                    StockStatus = TempStockStatus,
                                    QuantityCheck = TempIsAvb,
                                    StockQuantity = Convert.ToInt32(TempStockQ),
                                    Price = currency,
                                    IncrementQ = TempIncrementQ,
                                    MinQ = TempMinQ,
                                    TotalPrice = currency,
                                    ImgSource = ImgSource,
                                    PId = Convert.ToInt32(SProduct.id),
                                    ProductName = SProduct.name,
                                    ProductQuantity = ProductQuantity,
                                    VariationId = check,
                                    VariantParentId = TempId
                                });
                                Saving = true;
                                await DisplayAlert("Item Added to cart ", "Click again for a Shortcut to the Cart", "Okay");
                            }
                            else
                            {
                                oldlitz.Add(new CartList
                                {
                                    StockStatus = TempStockStatus,
                                    QuantityCheck = TempIsAvb,
                                    StockQuantity = Convert.ToInt32(TempStockQ),
                                    Price = currency,
                                    IncrementQ = TempIncrementQ,
                                    MinQ = TempMinQ,
                                    TotalDynamicPrice = currency,
                                    ImgSource = ImgSource,
                                    PId = Convert.ToInt32(SProduct.id),
                                    ProductName = SProduct.name,
                                    ProductQuantity = ProductQuantity,
                                    VariationId = check,
                                    VariantParentId = TempId,
                                    CartAtribKey = TempCartAtribK1,
                                    CartAtribValue = TempCartAtribV1,
                                    CartAtribKey1 = TempCartAtribK2,
                                    CartAtribValue1 = TempCartAtribV2,
                                    LimitedStock = TempLimited
                                });
                                Saving = true;
                                await DisplayAlert("Item Added to cart ", "Click again for a Shortcut to the Cart", "Okay");
                            }
                        }
                    }
                }
                else
                {
                    if (FlashSale)
                    {
                        Flasholdlitz.Add(new FlashCartlist
                        {
                            status = tempStatus,
                            StockStatus = TempStockStatus,
                            QuantityCheck = TempIsAvb,
                            StockQuantity = Convert.ToInt32(TempStockQ),
                            Price = currency,
                            IncrementQ = TempIncrementQ,
                            MinQ = TempMinQ,
                            TotalPrice = currency,
                            ImgSource = ImgSource,
                            PId = Convert.ToInt32(SProduct.id),
                            ProductName = SProduct.name,
                            ProductQuantity = ProductQuantity,
                            VariationId = check,
                            VariantParentId = TempId
                        });
                        Saving = true;
                        await DisplayAlert("Item Added to cart ", "Click again for a Shortcut to the Cart", "Okay");
                    }
                    else
                    {
                        oldlitz.Add(new CartList
                        {
                            StockStatus = TempStockStatus,
                            StockQuantity = Convert.ToInt32(TempStockQ),
                            Price = currency,
                            IncrementQ = TempIncrementQ,
                            MinQ = TempMinQ,
                            TotalDynamicPrice = currency,
                            ImgSource = ImgSource,
                            PId = Convert.ToInt32(SProduct.id),
                            ProductName = SProduct.name,
                            ProductQuantity = ProductQuantity,
                            VariationId = check,
                            VariantParentId = TempId,
                            CartAtribKey = TempCartAtribK1,
                            CartAtribValue = TempCartAtribV1,
                            CartAtribKey1 = TempCartAtribK2,
                            CartAtribValue1 = TempCartAtribV2,
                            LimitedStock = TempLimited
                        });
                        Saving = true;
                        await DisplayAlert("Item Added to cart ", "Click again for a Shortcut to the Cart", "Okay");
                    }
                }

                if (listy == null || Flashlisty == null)
                {
                    listy = oldlitz;
                    Flashlisty = Flasholdlitz;
                }
                else
                {
                    Flashlisty = FlashFullCart.CartList;
                    listy = FullCart.CartList;
                }


                if (Saving)
                {
                    FlashFullCart.CartList = Flasholdlitz.Union(Flashlisty).ToList();
                    FullCart.CartList = oldlitz.Union(listy).ToList();
                    var CartP = new CartPersistance();
                    CartP.FlashPersist(FlashFullCart.CartList);
                    CartP.Persist(FullCart.CartList);
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }

            #region AncientCode

            //oldlist.AddRange(oldlist.Select(s => new Cartlist { PId = Convert.ToInt32(z.id), Pname = z.name, Pquantity = productQuantity, variation_id = check }));
            // { $"{a}", $"{productQuantity}" , $"{finalP}" };
            //  List<WooCommerceNET.WooCommerce.v2.OrderLineItem> cart = new List<WooCommerceNET.WooCommerce.v2.OrderLineItem>();
            //  foreach (var k in z.variations)
            //  {
            //  }
            //  if (z.id == check)
            //  {
            //      cart.AddRange(new List<WooCommerceNET.WooCommerce.v2.OrderLineItem>
            //      {
            //      });
            //  }
            ////wait DisplayAlert(productQuantity.ToString(), finalprice.ToString(), "Okay");
            //  Time = DateTime.Now.ToString();
            //  WCObject wc = new WCObject(rest);
            ////  var customer = new Customer();
            // await wc.Customer.GetAll();
            //  if (Application.Current.Properties.ContainsKey("Email"))
            //  {
            //      Custemail = (Application.Current.Properties["Email"].ToString());
            //      var name = Custemail.Substring(0, Custemail.IndexOf('@')).Replace(".", " ");
            //  }
            //  Order o = new Order()
            //  {
            //      status = "pending",
            //      billing = { email = Custemail },
            //      line_items = cart,
            //  };
            //  foreach (var item in cart)
            //  {
            //      foreach (var x in p)
            //      {
            //          if (x.id == item.variation_id)
            //          {
            //              var yup = x.id;
            //              varID = Convert.ToInt32(yup);
            //          }
            //      }
            //      o.line_items.Add(new WooCommerceNET.WooCommerce.v2.OrderLineItem()
            //      {
            //          product_id = z.id,
            //          quantity = Convert.ToDecimal(productQuantity),
            //          variation_id = varID,
            //      });
            //  }
            //  

            #endregion
        }

        private async void ProductClicked(object sender, EventArgs args)
        {
            try
            {
                string check;
                var btn = (Button)sender;
                var a = btn.BindingContext;
                check = a.ToString();

                if (check == "simple") await DisplayAlert("Oops", "No Variations to Show", "Okay");
                try
                {
                    var wc = new WooCommerceNET.WooCommerce.v3.WCObject(GlobalVariable.Init.rest);
                    variablelistview.ItemsSource = VarProduct;
                }
                catch (Exception e)
                {
                    await DisplayAlert(e.ToString(), "No Variations to Show", "Okay");
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private void StringReplace()
        {
            try
            {
                //Replaces all the stupid HTML tags that come along with the Description
                foreach (var item in VarProduct)
                    item.description = item.description.Replace("<li>", "").Replace("<br />", "").Replace("<p>", "")
                        .Replace("</ol>", "").Replace("<ol>", "").Replace("<h4>", "").Replace("</h4>", "")
                        .Replace("<ul>", "").Replace("<li>'", "").Replace("<ol>", "").Replace("<strong>", "")
                        .Replace("<span>", "").Replace("<a>", "").Replace("<i>", "").Replace("</p>", "")
                        .Replace("</ul>", "").Replace("</li>", "");
                SProduct.description = SProduct.description
                    .Replace("<li>", "").Replace("<br />", "").Replace("<p>", "").Replace("</ol>", "")
                    .Replace("<ol>", "").Replace("<h4>", "").Replace("</h4>", "").Replace("<ul>", "")
                    .Replace("<li>'", "").Replace("<ol>", "").Replace("<strong>", "").Replace("<span>", "")
                    .Replace("<a>", "").Replace("<i>", "").Replace("</p>", "").Replace("</ul>", "")
                    .Replace("</li>", "");
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

        public class ObjectNullConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return !Equals(value, null);
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return value;
            }
        }


        public class InverseBoolConverter : IValueConverter, IMarkupExtension
        {
            public object ProvideValue(IServiceProvider serviceProvider)
            {
                if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));
                return this;
            }

            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return !(bool)value;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return value;
                //throw new NotImplementedException();
            }
        }
    }
}