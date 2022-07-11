using System.Collections.Generic;
using System.Linq;
using Ecombeta.Models;
using Newtonsoft.Json;
using Xamarin.Essentials;

namespace Ecombeta.Services
{
    public class CartPersistance
    {
        public static bool LoadedCart;

        public static string PerCart { get; set; }

        public static bool FlashLoadedCart;

        public static string FlashPerCart { get; set; }

        public void DePersist(string z)
        {
            if (z == null || z == "")
                FullCart.CartList = JsonConvert.DeserializeObject<List<CartList>>(PerCart);
            else
                FullCart.CartList = JsonConvert.DeserializeObject<List<CartList>>(z);
        }

        public void Persist(List<CartList> x)
        {
            var jsonString = JsonConvert.SerializeObject(x);
            if (LoadedCart)
            {
                var FetchedCart = Preferences.Get("Cart", "default_value");

                var y = JsonConvert.DeserializeObject<List<CartList>>(FetchedCart);

                var united = y.Union(FullCart.CartList).ToList();

                var jsonStringz = JsonConvert.SerializeObject(united);

                PerCart = jsonStringz;

                Preferences.Set("Cart", jsonStringz);

                LoadedCart = false;
            }
            else
            {
                Preferences.Set("Cart", jsonString);
            }
        }

        public void FlashDePersist(string z)
        {
            if (z == null || z == "")
                FlashFullCart.CartList = JsonConvert.DeserializeObject<List<FlashCartlist>>(FlashPerCart);
            else
                FlashFullCart.CartList = JsonConvert.DeserializeObject<List<FlashCartlist>>(z);
        }

        public void FlashPersist(List<FlashCartlist> x)
        {
            var jsonString = JsonConvert.SerializeObject(x);
            if (LoadedCart)
            {
                var FetchedCart = Preferences.Get("FlashCart", "default_value");

                var y = JsonConvert.DeserializeObject<List<FlashCartlist>>(FetchedCart);

                var united = y.Union(FlashFullCart.CartList).ToList();

                var jsonStringz = JsonConvert.SerializeObject(united);

                PerCart = jsonStringz;

                Preferences.Set("FlashCart", jsonStringz);

                LoadedCart = false;
            }
            else
            {
                Preferences.Set("FlashCart", jsonString);
            }
        }
    }
}