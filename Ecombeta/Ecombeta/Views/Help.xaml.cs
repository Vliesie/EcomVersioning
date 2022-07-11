using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using Ecombeta.Models;
using Xamarin.Forms;

namespace Ecombeta.Views
{
    public partial class HelpPage : ContentPage, INotifyPropertyChanged
    {
        protected async override void OnAppearing()
        {
            App.MakeWebRequest();
            if (App.IsConnected)
            {

            }
            else
            {
                await Navigation.PushAsync(new NoInternet());
            }
        }
        public HelpPage()
        {
            InitializeComponent();
            helppage.Title = "Help";
            HelpText.Text =

     "- Mica Market app requires a internet connection as it uses an API." + System.Environment.NewLine + System.Environment.NewLine +
     "- What is a API? It lets one piece of software talk to another." + System.Environment.NewLine + System.Environment.NewLine +
     "- If you see the " + $"'No Intenet' " + "page just check your connection and press" + $"'try again'." + System.Environment.NewLine + System.Environment.NewLine +
     "- To" + $"'view' " + "or " + $"'purchase' " + "any products please ensure you are logged in as this is how the orders are tracked." + System.Environment.NewLine + System.Environment.NewLine +
     "- It is recommended that you read all pop - ups as these contain either issues or information regarding the application." + System.Environment.NewLine + System.Environment.NewLine +
     "- The application has a search feature for suppliers simply type in lower case inside of the [Search] Box on the suppliers place E.G dulux and dulux will show." + System.Environment.NewLine + System.Environment.NewLine +
     "- On the main page you can find a short descriptive carousel about the application." + System.Environment.NewLine + System.Environment.NewLine +
     "- You can " + $"'view' " + "any orders you've placed by going to the = icon and tapping on the Orders then press view Orders to look at a single Order." + System.Environment.NewLine + System.Environment.NewLine +
     "- There are notifications to notify you of any and all Flash sales." + System.Environment.NewLine + System.Environment.NewLine +
     "- Flash Sales will be removed form your cart if the time expires." + System.Environment.NewLine + System.Environment.NewLine +
     "- Pressing the back button repeatidly will exit the application. This is not the application crashing." + System.Environment.NewLine + System.Environment.NewLine +
     "- Quantities can be adjusted on the product and the cart" + System.Environment.NewLine + System.Environment.NewLine +
     "- If ever there is a crash and your asked to send a report, please do." + System.Environment.NewLine + System.Environment.NewLine;
        
        }
        bool canExecute = true;

        public bool CanExecute
        {
            get
            {
                return canExecute;
            }
            set
            {
                canExecute = value;
                NotifyPropertyChanged(nameof(CanExecute));
            }
        }

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public new event PropertyChangedEventHandler PropertyChanged;
        void Button_Clicked(System.Object sender, System.EventArgs e)
        {
          

          
        }
    }
}
