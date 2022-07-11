using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Ecombeta.Views;
using Microsoft.AppCenter.Crashes;
using Rg.Plugins.Popup.Extensions;
using WooCommerceNET;
using WooCommerceNET.WooCommerce.v2;
using Xamarin.Forms;
using Customer = WooCommerceNET.WooCommerce.v3.Customer;
using Product = WooCommerceNET.WooCommerce.v3.Product;
using ProductTag = WooCommerceNET.WooCommerce.v3.ProductTag;
using VariationImage = WooCommerceNET.WooCommerce.v3.VariationImage;

namespace Ecombeta.Models
{
    public class GlobalVariable
    {
        #region Globals

        #region Singleton

        private GlobalVariable()
        {
        }

        public static bool Tester;
        public static GlobalVariable Init { get; } = new GlobalVariable();

        #endregion

        public RestAPI rest { get; } = new RestAPI("http://mm-app.co.za/wp-json/wc/v3/",
            "ck_a25f96835aabfc64b09613eb8ec4a8c9bcd5dcd0", "cs_8f247c22353f25b905c96171379b89714f8f4003");

        #endregion
    }

    public class HomeVariables : INotifyPropertyChanged
    {



        private string _message;

        private string _titleMessage;

        public string Message
        {
            get => _message;
            set
            {
                if (_message == value) return;
                _message = value;
               
            }
        }

        public string TitleMessage
        {
            get => _titleMessage;
            set
            {
                if (_titleMessage == value) return;
                _titleMessage = value;
                RaisePropertyChanged();
            }
        }

        public List<MasterPageItem> MenuList { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public void RaisePropertyChanged([CallerMemberName] string propertyName = "")
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

        public async void HandleCustomEvent(object sender, PropertyChangedEventArgs a)
        {
            try
            {
                    await Application.Current.MainPage.Navigation.PushPopupAsync(new NotificationsPlugin(TitleMessage, Message));
                    Message = null;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        #region Singleton

        private HomeVariables()
        {
        }


        public static HomeVariables Init { get; } = new HomeVariables();

        #endregion
    }

    public class SuppliersVariables : INotifyPropertyChanged
    {
        public List<Customer> CustomerList;

        public List<ProductTag> Tags;

        public ICommand PinButtonCommand => null;


        public string TagId { set; get; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Singleton

        private SuppliersVariables()
        {
        }


        public static SuppliersVariables Init { get; } = new SuppliersVariables();

        #endregion
    }

    public class ProductsVariables : INotifyPropertyChanged
    {
        public ObservableCollection<Product> Items;

        public List<Productimport> FinalProducts { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Singleton

        private ProductsVariables()
        {
        }


        public static ProductsVariables Init { get; } = new ProductsVariables();

        #endregion
    }

    public class ProductProperties : INotifyPropertyChanged
    {
       

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Singleton

        private ProductProperties()
        {
        }


        public static ProductProperties Init { get; } = new ProductProperties();

        #endregion
    }
}