using Ecombeta.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WooCommerceNET;
using WooCommerceNET.WooCommerce.v3;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Ecombeta.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrderList : ContentPage
    {
        public class Bought {
            public string name { get; set; }
        }

        //List<Bought> BoughtI;
        public OrderList()
        {
            InitializeComponent();
     
            Imageback.Source = "https://mm-app.co.za/wp-content/uploads/2019/12/Bluepoly.jpg";
            Init();
        }
          
        public async void Init()
        {
            RestAPI rest = new RestAPI("http://mm-app.co.za/wp-json/wc/v3/", "ck_a25f96835aabfc64b09613eb8ec4a8c9bcd5dcd0", "cs_8f247c22353f25b905c96171379b89714f8f4003");
            WCObject wc = new WCObject(rest);

           var p = await wc.Order.GetAll(new Dictionary<string, string>()
            {
                {"customer", Users.CId.ToString() },
                { "per_page", "100" }
            });
            Orderslist.ItemsSource = p;
            //foreach (var item in p)
            //{
            //    foreach (var z in item.line_items)
            //    {
            //        BoughtI.Add(new Bought { name = z.name });
            //    }
            //
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            int check;

            var btn = (Button)sender;

            var a = btn.BindingContext;

            check = Convert.ToInt32(a);

            SingleOrder.PassOid = check;

             Navigation.PushAsync(new SingleOrder());

        }
    }
}