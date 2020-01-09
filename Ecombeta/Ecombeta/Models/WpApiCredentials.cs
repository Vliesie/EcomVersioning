using System;
using System.Collections.Generic;
using System.Text;

namespace Ecombeta.Models
{
   public class WpApiCredentials
    {
        public static string SiteUri = "http://mm-app.co.za/wp-json/";
        public static string WordPressUri = $"https://public-api.wordpress.com/wp/v2/sites/{SiteUri}/";
        public static string Username = "Name";
        public static string Password = "password";
        public static int id { get; set; }
        public static string token { get; set; }


    }
}
