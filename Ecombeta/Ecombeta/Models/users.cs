using System;
using System.Collections.Generic;
using System.Text;
using WooCommerceNET.WooCommerce.v3;
using WooCommerceNET.WooCommerce.v3.Extension;
using WooCommerceNET;

namespace Ecombeta.Models
{
    public class Users
    {
        public static bool Loggedin { get; set; }
        public static string Username { get; set; }
        public  string Password { get; set; }
        public static string Store { get; set; }
        public static int CId { get; set; }
        public static string CEmail { get; set; }

        public int id { get; set; }
       
    }
}
