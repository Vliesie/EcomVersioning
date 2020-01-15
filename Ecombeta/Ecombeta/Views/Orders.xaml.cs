using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecombeta.Models;
using Ecombeta.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using WooCommerceNET.WooCommerce.v3;

using WooCommerceNET.WooCommerce.v3.Extension;
using WooCommerceNET;
using System.Net;
using Android.Content.Res;
using System.Globalization;
using System.Collections;

using WCObject = WooCommerceNET.WooCommerce.v3.WCObject;

namespace Ecombeta.Views
{



    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Orders : ContentPage
    {
        RestAPI rest = new RestAPI("http://mm-app.co.za/wp-json/wc/v3/", "ck_a25f96835aabfc64b09613eb8ec4a8c9bcd5dcd0", "cs_8f247c22353f25b905c96171379b89714f8f4003");


        #region TemparyVariables
        public List<WooCommerceNET.WooCommerce.v3.Variation> p;
      

        public static bool isUnlimted;
        public string title { get; set; }
        public static Product z;

        public string imagesrc;
        public static VariationImage img;
        public string Custemail { set; get; }
        public int index { get; set; }
        public static double productQuantity { get; set;}
        public double fluxprice { get; set; }
        public decimal Tempprice { get; set; }
        public object Increment { get; set; }
        public static decimal priceinprogress { get; set; }
        public static int singleID { get; set; }

        public List<Cartlist> oldlist;

        public string Name { get; set; }

        public double TempStockQ { get; set; }
        public bool ProductE { get; set; }
        public int TempIncrementQ { get; set; }
        public int TempMinQ { get; set; }

        public List<WooCommerceNET.WooCommerce.v2.OrderLineItem> orderline;
        public string Time { get; set; }

        string TempIsAvb;
        public int varID { get; set; }

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

        public Orders()
        {
          
            InitializeComponent();
   
            Pageback.BackgroundImageSource = "https://mm-app.co.za/wp-content/uploads/2019/12/Bluepoly.jpg";
            InitAsync();

        }

       
        private async void Back_Clicked(object sender, EventArgs e)
        {
            productQuantity = 0;
            await Navigation.PopAsync();
           
        }
        public async Task InitAsync()
        {

            WCObject wc = new WCObject(rest);
            index = 0;
            productQuantity = 0;

            //Simple
            z = await wc.Product.Get(singleID);
            z._virtual = true;
            //Variable
            p = await wc.Product.Variations.GetAll(singleID);

            var xxxx = p;
            var indexer = 0;

            // NOt to sure how Threading works but I want this to run on its own Thread as its somewhat intense || Cant Test this but Hoping this calls the Method and Starts it on a new thread then auto kills it self when its done
            Thread SThread = new Thread(Orders.StringReplace);
            SThread.Start();


            //Simple Quantity checks
            foreach (var item in z.meta_data)
            {
                if (z.meta_data[indexer].key == "group_of_quantity")
                {
                    productQuantity = Convert.ToDouble(z.meta_data[indexer].value);
                    indexer++;
                }
                else
                {
                    indexer++;
                }
            }
           
            if (z.stock_quantity <= 0)
            {
                TempStockQ = 2;
                z.stock_quantity = 2;
                z.meta_data[1].value = 1;
                TempIsAvb = "Not in stock";
                z._virtual = false;
            
            }
            if (z.stock_quantity == null)
            {
                isUnlimted = true;
                //If Stock is null it means unlimited so 9999999 will be per purchase instance
                var i = 999999;
                z.stock_quantity = i;
                z._virtual = true;
                TempIsAvb = "In Stock";
            }
            else
            {
                z._virtual = true;
                TempStockQ = Convert.ToInt32(z.stock_quantity);
            }


            //Variable Quantity Checks
          
            foreach (var item in p)
            {
                if (z.meta_data[indexer].key == "group_of_quantity")
                {
                    productQuantity = Convert.ToDouble(z.meta_data[indexer].value);
                }
                else
                {
                    indexer++;
                }
                int value = 100000;
                var StockQ = Convert.ToInt32(z.stock_quantity);
                if (item.stock_quantity <= 0)
                {
                    TempStockQ = 2;
                    item.stock_quantity = 2;
                    item.meta_data[1].value = 1;
                    TempIsAvb = "Not in stock";
                    item._virtual = false;
                }
                if (item.stock_quantity == null)
                {
                    isUnlimted = true;
                    StockQ = value;
                    TempIsAvb = "In Stock";
                    //If Stock is null it means unlimited so 9999999 will be per purchase instance
                    var i = 999999;
                    z.stock_quantity = i;
                    z._virtual = true;
                }
             
                if (item.meta_data[2].value == null)
                {
                    double i = productQuantity;
                    item.meta_data[2].value = i;
                   
                    item._virtual = true;
                    TempIsAvb = "In Stock";
                }

                if (item.meta_data[2].value is string stringValue && String.IsNullOrWhiteSpace(stringValue))
                {
                    double i = productQuantity;
                    item.meta_data[2].value = i;
                }

                if (item.meta_data[2].value is int intValue && intValue <= 0)
                {
                    intValue = (int)item.meta_data[2].value;
                    intValue = (int)item.meta_data[1].value;
                    item.meta_data[2].value = intValue;
                }
                else
                {
                    TempStockQ = Convert.ToInt32(item.stock_quantity);
                }
                var xxx = item.meta_data[2].value;
            }
            var x = z.price;
            Tempprice = Convert.ToDecimal(x);
           
            

            if (z != null || p != null)
            {
                if (z.price_html != null)
                {
                    z.price_html = "R" + z.regular_price;
                    if (z.regular_price != null)
                    {
                        z.price_html = "R" + z.regular_price;
                        if (z.sale_price != null)
                        {
                            z.price_html = "From R" + z.regular_price + " To " + "R" + z.sale_price + "On Sale";
                        }
                    }
                }
                if (z.type == "simple")
                {
                    if (Users.Loggedin == false)
                    {
                        z.price = 0;
                        z.price_html = "Please Login to view the Prices";
                    }
                    Title = "Single Product";
                    variablelistview.ItemsSource = new Product[1] { z };
                }
                else
                {
                    foreach (var z in p)
                    {
                        //Can use z instead of p[Index] Index just Increments and Then runs threw the loop and changes the details but z is the easier way.
                        p[index].shipping_class_id = "From R" + p[index].regular_price.ToString() + " To " + "R" + p[index].sale_price.ToString() + "On Sale";
                        index++;
                    }
                    index = 0;
                    if (Users.Loggedin == false)
                    {
                        foreach (var item in p)
                        {
                            item.price = 0;
                            item.shipping_class_id = "Please Login to view the Prices";
                            // item.shipping_class_id = "From R" + item.regular_price.ToString() + " To " + "R" + item.sale_price.ToString() + "On Sale";
                        }
                    }
                    Title = "Multi Product";//This Doest Work but w.e 
                    variablelistview.ItemsSource = p; 
                }

            }
           
        }

         void OnStepperValueChanged(object sender, ValueChangedEventArgs e)
         {
                productQuantity = e.NewValue;
         }


        public async void AddtoCart(object sender, EventArgs args)
        {
            if (TempIsAvb == "Not in stock")
            {
                DisplayAlert("Issue", "Product is out of stock", "Okay");
                return;
            }
            foreach (var item in p)
            {
                if (TempIsAvb == "Not in stock")
                {
                    DisplayAlert("Issue", "Product is out of stock", "Okay");
                    return;
                }
            }
            if (z.price == null)
            {
                priceinprogress = Convert.ToDecimal(z.price_html);
            }
            else
            {
                priceinprogress = Convert.ToDecimal(z.price);
            }

            int index1 = 0;
            int index2 = 1;
            var listy = FullCart.Cartlistz;
            int check;
            var btn = (Button)sender;
            var a = btn.BindingContext;
            check = Convert.ToInt32(a);
            
            if (z.type != "simple")
            {
                foreach (var cp in p)
                {
                    if (check == cp.id)
                    {
                        if (cp.stock_quantity == null)
                        {
                            //If Stock is null it means unlimited so 9999999 will be per purchase instance
                            var stockq = 9999999;
                            cp.stock_quantity = Convert.ToInt32(stockq);
                            TempStockQ = Convert.ToDouble(stockq);
                        }
                       
                        if (Convert.ToInt32(cp.meta_data[1].value) == 0)
                        {
                            TempMinQ = 1;
                        }
                        else
                        {
                            TempMinQ = Convert.ToInt32(cp.meta_data[1].value);
                        }
                       
                        TempIncrementQ = Convert.ToInt32(cp.meta_data[2].value);
                
                        priceinprogress = Convert.ToDecimal(cp.price);
                        imagesrc = cp.image.src;
                        if (z.price == null)
                        {
                            Tempprice = Convert.ToDecimal(cp.price);
                            priceinprogress = Convert.ToDecimal(cp.price);
                        }
                    }
                }
            }      
            else
            {
                TempStockQ = Convert.ToInt32(z.stock_quantity);
                
                Tempprice = Convert.ToDecimal(z.price);
                if (Convert.ToInt32(z.meta_data[1].value) == 0)
                {
                    TempMinQ = 1;
                }
                else
                {
                    if (z.meta_data[index1].key == "minimum_allowed_quantity")
                    {
                        TempMinQ = Convert.ToInt32(z.meta_data[index1].value);
                    }
                    else
                    {
                        index1++;
                        TempMinQ = Convert.ToInt32(z.meta_data[index1].value);
                    }
                }
                if (z.meta_data[index2].key == "group_of_quantity")
                {
                    TempIncrementQ = Convert.ToInt32(z.meta_data[index2].value);
                }
                else
                {
                    index2++;
                    TempIncrementQ = Convert.ToInt32(z.meta_data[index2].value);
                }
                imagesrc = z.images[0].src;
            }




       
            if (a == null)
            {
                check = Convert.ToInt32(z.id);
            }
            if (oldlist == null) 
            {
                oldlist = new List<Cartlist>();
               
            }
            var dolla = priceinprogress * Convert.ToDecimal(productQuantity);
            List<Cartlist> oldlitz = new List<Cartlist>();


            if (FullCart.Cartlistz != null)
            {
                var firstMatch = FullCart.Cartlistz.FirstOrDefault(i => i.PId == check);
                var SecondMatch = FullCart.Cartlistz.FirstOrDefault(i => i.variation_id == check);
                if (firstMatch != null)
                {
                   var y =  await DisplayAlert("Product Already in Cart", "Change Quanity On Cart", "Go cart", "Keep Shopping");
                    if (y)
                    {
                        await App.Current.MainPage.Navigation.PushAsync(new Cart());
                    }
                }
                else
                {
                    if (SecondMatch != null)
                    {
                        var y = await DisplayAlert("Product Already in Cart", "Change Quanity On Cart", "Go cart", "Keep Shopping");
                        if (y)
                        {
                            var masterDetailPage = new Home("");
                            masterDetailPage.Detail = new NavigationPage(new Cart());
                            Application.Current.MainPage = masterDetailPage;
                   
                        }
                    }
                    else
                    {
               
                        oldlitz.Add(new Cartlist {CheckforQuantity = TempIsAvb, StockQuantity = Convert.ToInt32(TempStockQ), Price = Tempprice, IncrementQ = TempIncrementQ, MinQ = TempMinQ, Totalprice = dolla, Imagesrc = imagesrc, PId = Convert.ToInt32(z.id), Pname = z.name, Pquantity = productQuantity, variation_id = check });
                        await DisplayAlert("Checkout", "Item Added to Cart", "Okay");
                    }
                } 
            }
            else
            {
                oldlitz.Add(new Cartlist { StockQuantity = Convert.ToInt32(TempStockQ), Price = Tempprice, IncrementQ = TempIncrementQ, MinQ = TempMinQ, Totalprice = dolla, Imagesrc = imagesrc, PId = Convert.ToInt32(z.id), Pname = z.name, Pquantity = productQuantity, variation_id = check });
                await DisplayAlert("Checkout1", "Item Added to Cart", "Okay");
            }

            if (listy == null)
            {
                listy = oldlitz;
            }
            else
            {
                listy = FullCart.Cartlistz;
            }
            FullCart.Cartlistz = oldlitz.Union(listy).ToList();
            InitAsync();

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
        async void ProductClicked(object sender, EventArgs args)
        {
            string check;
            var btn = (Button)sender;
            var a = btn.BindingContext;
            check = a.ToString();

            if (check == "simple")
            {
                await DisplayAlert("Oops", "No Variations to Show", "Okay");
            }
            try
            {
                RestAPI rest = new RestAPI("http://mm-app.co.za/wp-json/wc/v3/", "ck_a25f96835aabfc64b09613eb8ec4a8c9bcd5dcd0", "cs_8f247c22353f25b905c96171379b89714f8f4003");
                WCObject wc = new WCObject(rest);
                variablelistview.ItemsSource = p;
            }
            catch (Exception e)
            {
                DisplayAlert(e.ToString(), "No Variations to Show", "Okay");
            }
        }

        async void StringReplace(){
          //Replaces all the stupid HTML tags that come along with the Description
            foreach(var item in p){
      
                 item.description = item.description.Replace('<p>', '').Replace('<ul>', '').Replace('<li>', '').Replace('<ol>','').Replace('<strong>','').Replace('<span>','').Replace('<a>','').Replace('<i>',''); 
            }
            z.description = z.description.Replace('<p>', '').Replace('<ul>', '').Replace('<li>', '').Replace('<ol>','').Replace('<strong>','').Replace('<span>','').Replace('<a>','').Replace('<i>',''); 
        }
    }
}