using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Ecombeta.Views
{
    public partial class LoadingPopup : Rg.Plugins.Popup.Pages.PopupPage
    {
        public LoadingPopup()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            Loadlayout.IsVisible = true;
            Loader.IsRunning = true;
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            Loadlayout.IsVisible = false;
            Loader.IsRunning = false;
        }

    }
}
