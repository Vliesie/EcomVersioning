using System.Net.Http;
using System.Threading.Tasks;
using Ecombeta.Models;
using Newtonsoft.Json;

namespace Ecombeta.Services
{
    internal class WoocommerceAPI
    {
        //This is my Own Pull for the WoocomAPI without the Wrapper I get Thread errors on the Desarialize Bit.

        //Sample Keys to test with 
//         private static readonly string website_url = "https://azipit.co.za/mica-market-app";
//         private static readonly string consumer_key = "ck_0112f135e2f9b32cc147f28028fd621f919bc890";
//         private static readonly string consumer_secret = "cs_38ea21f4d63eb96a801868993b66065dcb0362fa";

        public static string GetAllProductsApiUrl =
            string.Format("{0}/wp-json/wc/v3/products?tag={1}&consumer_key={2}&consumer_secret={3}", website_url,
                SuppliersVariables.Init.TagId, consumer_key, consumer_secret);
        //private static string GetAllOrdersApiUrl = string.Format("{0}/wc-api/v3/orders?consumer_key={1}&consumer_secret={2}", website_url, consumer_key, consumer_secret);


        public async Task<Productz> GetAllProducts()
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(GetAllProductsApiUrl);
            var content = response.Content;
            var json = await content.ReadAsStringAsync();
            var products = JsonConvert.DeserializeObject<Productz>(json);


            return products;
        }

        //public async Task<Orders> GetAllOrders()
        //{
        //    var httpClient = new HttpClient();
        //    var response = await httpClient.GetAsync(GetAllOrdersApiUrl);
        //    HttpContent content = response.Content;
        //    var json = await content.ReadAsStringAsync();
        //    var orders = JsonConvert.DeserializeObject<Orders>(json);
        //    return orders;
        //}
    }
}
