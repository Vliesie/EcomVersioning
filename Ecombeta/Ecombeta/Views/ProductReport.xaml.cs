using System;
using System.Collections.Generic;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;

namespace Ecombeta.Views
{
    public partial class ProductReport : Rg.Plugins.Popup.Pages.PopupPage
    {
        public ProductReport(List<ProductReports> report)
        {
            InitializeComponent();

            ErrorReport.ItemsSource = report;
        }

       async void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            await Navigation.PopPopupAsync();
        }
    }
}
