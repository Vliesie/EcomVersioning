using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Diagnostics;
using Xamarin.Forms;
using System.Collections.ObjectModel;

namespace Ecombeta.Models
{

    
     public class Cartlist : System.ComponentModel.INotifyPropertyChanged
    {

  


        public event PropertyChangedEventHandler PropertyChanged;
        public string Pname { get; set; }

       
        public double StockQuantity { get; set; }
      
        public int PId { get; set; }

        public  double Pquantity { get; set; }

        public int variation_id { get; set; }

        private decimal totalprice;
        public decimal Totalprice
        {
             get{ return totalprice; }

            set
            {
                if (value.CompareTo(totalprice) != 0)
                {
                    totalprice = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public  string CheckforQuantity;
        public decimal Price { get; set; }
        public  string Imagesrc { get; set; }

        public int IncrementQ { get; set; }
        public  int MinQ { get; set; }

        protected void NotifyPropertyChanged([System.Runtime.CompilerServices.CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class ItemList : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<Cartlist> _items;

        public ObservableCollection<Cartlist> Items
        {
            get { return _items; }
            set { _items = value; OnPropertyChanged("Items"); }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null)
                return;
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public ItemList(List<Cartlist> itemList)
        {

            Items = new ObservableCollection<Cartlist>();
            if (itemList != null)
            {
                foreach (Cartlist itm in itemList)
                {
                    Items.Add(itm);
                }
            }
          
        }
    }

    public class FullCart
    {
        
                                       
        public static List<Cartlist> Cartlistz;

       
    }


}
