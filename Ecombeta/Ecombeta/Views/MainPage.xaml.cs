using System;
using System.Collections.Generic;
using Ecombeta.Models;
using Microsoft.AppCenter.Crashes;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Ecombeta.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        private bool _hide;


        public List<CarouselViewModel> CarouselSource { get; set; }
        public MainPage()
        {

            try
            {
                InitializeComponent();

                //var imagez = new List<string>
                //{
                //    "",
                //    "https://mm-app.co.za/wp-content/uploads/2020/01/MicaPic2.jpg",
                //    "https://mm-app.co.za/wp-content/uploads/2020/01/MicaPic3.jpg",
                //    "https://mm-app.co.za/wp-content/uploads/2020/01/MicaPic4.jpg",
                //    "https://mm-app.co.za/wp-content/uploads/2020/01/MicaPic5.jpg"
                //};

                CarouselSource = new List<CarouselViewModel>
                {//https://images.squarespace-cdn.com/content/v1/5ade9263620b8564e0cad4ce/1573504242014-YS0DSH2AIGKICCTOD29H/ke17ZwdGBToddI8pDm48kNiEM88mrzHRsd1mQ3bxVct7gQa3H78H3Y0txjaiv_0fDoOvxcdMmMKkDsyUqMSsMWxHk725yiiHCCLfrh8O1z4YTzHvnKhyp6Da-NYroOW3ZGjoBKy3azqku80C789l0s0XaMNjCqAzRibjnE_wBlkZ2axuMlPfqFLWy-3Tjp4nKScCHg1XF4aLsQJlo6oYbA/NightMarketSheds.jpeg
                
                  new CarouselViewModel {
                    imagesrc = "https://mm-app.co.za/wp-content/uploads/2020/02/PW3C4257-scaled.jpg",
                    SubImage = "https://mm-app.co.za/wp-content/uploads/2020/02/MicaMarketCarouselLogo.png",
                    Title = "Welcome to",
                    SubTitle = "Here's how it works",
                    content = "On the top left you'll find the Menu Icon ☰ | Or MenuText called" + " Menu " + "this is how we get around",
                    footer = "SWIPE ⏎ TO CONTINUE"
                  },
                  new CarouselViewModel
                  {
                      imagesrc = "https://mm-app.co.za/wp-content/uploads/2020/02/shutterstock_251928148.jpg",
                      Title = "Step 1:" + System.Environment.NewLine + "Login",
                      SubTitle = "Begin by logging in!",
                      content = "The full mica market application cannot be expierenced untill you've logged in! So use the ☰ || or" +"Menu"+ "navigation icon & tap on" + $" 'Login' " + "& use provided details",
                       footer = "SWIPE ⏎ TO CONTINUE"
                  },
                  new CarouselViewModel
                  {
                      imagesrc = "https://mm-app.co.za/wp-content/uploads/2020/02/shutterstock_627578354-1.png",
                      Title = "Step 2: Products",
                      SubTitle = "Suppliers/Products/Product",
                      content = "Products can be found under Suppliers, Select a Supplier, Select a Product, Add to Cart or keep Shopping",
                      footer = "SWIPE ⏎ TO CONTINUE"
                  },
                  new CarouselViewModel
                  {
                      imagesrc = "https://mm-app.co.za/wp-content/uploads/2020/02/shutterstock_1017676501.jpg",
                      Title  = "Step 3:" + System.Environment.NewLine + "Cart",
                      SubTitle = "The Cart is where we place orders",
                      content = "Inside of the Cart if we tap on a item we can see the total price when we increase the quantity. This is also where we'll place our orders",
                      footer = "SWIPE ⏎ TO CONTINUE"
                  },
                  new CarouselViewModel
                  {
                      imagesrc = "https://mm-app.co.za/wp-content/uploads/2020/02/shutterstock_1579838815.png",
                      Title = "Step 4:" + System.Environment.NewLine + "Orders",
                      SubTitle = "We can view any orders we've placed",
                      content = "If we use the navigation icon ☰ and tap on orders we can see all the orders we've placed. We can also tap on an order to view everything that was in the basket"
                  }


                };

                NavigationPage.SetHasBackButton(this, false);

                carousel.ItemsSource = CarouselSource;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Crashes.TrackError(ex);
            }

            if (Users.LoggedIn) _hide = false;
            if (Users.LoggedIn == false) _hide = true;
        }


        private void loginbtn_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage.Navigation.PushAsync(new Login());
        }

        void CartIcon_Clicked(System.Object sender, System.EventArgs e)
        {
            var masterDetailPage = new Home { Detail = new NavigationPage(new Cart()) };
            Application.Current.MainPage = masterDetailPage;
        }
    }
    public class CarouselViewModel
    {
        public string imagesrc { get; set; }

        public string SubImage { get; set; }
        public string Title { get; set; }

        public string SubTitle { get; set; }

        public string content { get; set; }

        public string footer { get; set; }


    }
}