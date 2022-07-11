using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace Ecombeta.Models
{
    [DataContract]
    public class CartList : INotifyPropertyChanged
    {
        private bool _inStock;

        private bool _isVisible;

        private int _selectedId;
        private string _stockStatus;

        private decimal _tPrice;
        public string QuantityCheck { get; set; }

        [DataMember] public string ProductName { get; set; }

        public bool InStock
        {
            get => _inStock;

            set
            {
                _inStock = value;
                NotifyPropertyChanged();
            }
        }

        [DataMember]
        public string StockStatus
        {
            get => _stockStatus;

            set
            {
                _stockStatus = value;
                NotifyPropertyChanged();
            }
        }

        [DataMember] public string CartAtribKey { get; set; }
        [DataMember] public string CartAtribValue { get; set; }

        [DataMember] public bool LimitedStock { get; set; }
        [DataMember] public string CartAtribKey1 { get; set; }
        [DataMember] public string CartAtribValue1 { get; set; }

        [DataMember] public double StockQuantity { get; set; }

        [DataMember] public int PId { get; set; }


        private double _productQuantity;

        [DataMember]
        public double ProductQuantity
        {
            get => _productQuantity;

            set
            {
                _productQuantity = value;
                NotifyPropertyChanged();
            }
        }

        private string _ErrorMsg;

        [DataMember]
        public string ErrorMsg
        {
            get => _ErrorMsg;

            set
            {
                _ErrorMsg = value;
                NotifyPropertyChanged();
            }
        }

        
        [DataMember] public int VariationId { get; set; }

        public int VariantParentId { get; set; }

        [DataMember]
        public decimal TotalDynamicPrice
        {
            get => _tPrice;

            set
            {
                if (value.CompareTo(_tPrice) != 0)
                {
                    _tPrice = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public bool IsVisible
        {
            get => _isVisible;

            set
            {
                _isVisible = value;
                NotifyPropertyChanged();
            }
        }

        public int SelectedId
        {
            get => _selectedId;

            set
            {
                _selectedId = value;
                NotifyPropertyChanged();
            }
        }

        [DataMember] public decimal Price { get; set; }

        [DataMember] public string ImgSource { get; set; }

        [DataMember] public int IncrementQ { get; set; }

        [DataMember] public int MinQ { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public sealed class ItemList : INotifyPropertyChanged
    {
        private ObservableCollection<CartList> _items;



        public ItemList(List<CartList> itemList)
        {
            Items = new ObservableCollection<CartList>();
            if (itemList == null) return;
            foreach (var itm in itemList)
                Items.Add(itm);
        }

        public ObservableCollection<CartList> Items
        {
            get => _items;
            private set
            {
                _items = value;
                OnPropertyChanged("Items");
            }
        }



        private bool _selected;
        public bool IsSelected
        {
            get => _selected;

            set
            {
                _selected = value;
                NotifyPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public static class FullCart
    {
        public static List<CartList> CartList { get; set; }
    }
}