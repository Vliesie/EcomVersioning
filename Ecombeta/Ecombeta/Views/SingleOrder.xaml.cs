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
    public partial class SingleOrder : ContentPage
    {
        public static int PassOid;
        public SingleOrder()
        {
            InitializeComponent();
            
            Backimage.BackgroundImageSource = "https://mm-app.co.za/wp-content/uploads/2019/12/Bluepoly.jpg";
            Init();
        }

        public async void Init()
        {
            RestAPI rest = new RestAPI("http://mm-app.co.za/wp-json/wc/v3/", "ck_a25f96835aabfc64b09613eb8ec4a8c9bcd5dcd0", "cs_8f247c22353f25b905c96171379b89714f8f4003");
            WCObject wc = new WCObject(rest);

            var p = await wc.Order.Get(PassOid);

            SingleOrderList.ItemsSource = new Order[1] { p };

            Lineorders.ItemsSource = p.line_items;
        }
    }
}