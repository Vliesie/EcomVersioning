using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WooCommerceNET.WooCommerce.v3;

namespace Ecombeta.Models
{

    public class Flashlist : INotifyPropertyChanged
    {
        private ObservableCollection<Product> _items;



        public Flashlist(List<Product> itemList)
        {
            Items = new ObservableCollection<Product>();
            if (itemList == null) return;
            foreach (var itm in itemList)
                Items.Add(itm);

        }

        public ObservableCollection<Product> Items
        {
            get => _items;
            private set
            {
                _items = value;
                OnPropertyChanged("Items");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class FlashCartlist : INotifyPropertyChanged
    {
      

        private string _stockStatus;

        private decimal _totalPrice;
        public string QuantityCheck { get; set; }
        public string ProductName { get; set; }

        private string _status;
        public string status
        {
            get => _status;

            set
            {
                _status = value;
                NotifyPropertyChanged();
            }
        }

        private bool _inStock;
        public bool InStock
        {
            get => _inStock;

            set
            {
                _inStock = value;
                NotifyPropertyChanged();
            }
        }

        public string StockStatus
        {
            get => _stockStatus;

            set
            {
                _stockStatus = value;
                NotifyPropertyChanged();
            }
        }

        public double StockQuantity { get; set; }

        public int PId { get; set; }

        public int VariantParentId { get; set; }
        public double ProductQuantity { get; set; }

        public int VariationId { get; set; }

        public decimal TotalPrice
        {
            get => _totalPrice;

            set
            {
                if (value.CompareTo(_totalPrice) == 0) return;
                _totalPrice = value;
                NotifyPropertyChanged();
            }
        }

        public decimal Price { get; set; }
        public string ImgSource { get; set; }

        public int IncrementQ { get; set; }
        public int MinQ { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public sealed class FlashItemList : INotifyPropertyChanged
    {
        private ObservableCollection<FlashCartlist> _Flashitems;

        public FlashItemList(List<FlashCartlist> itemList)
        {
            FlashItems = new ObservableCollection<FlashCartlist>();
            if (itemList == null) return;
            foreach (var itm in itemList)
                FlashItems.Add(itm);
        }

        public ObservableCollection<FlashCartlist> FlashItems
        {
            get => _Flashitems;
            private set
            {
                _Flashitems = value;
                OnPropertyChanged("FlashItems");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public static class FlashFullCart
    {
        public static List<FlashCartlist> CartList;
    }
}