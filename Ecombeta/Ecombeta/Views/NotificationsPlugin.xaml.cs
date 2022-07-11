using System;
using System.Collections.Generic;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace Ecombeta.Views
{
    public partial class NotificationsPlugin : PopupPage
    {
        public NotificationsPlugin(string Title, string Body)
        {
            InitializeComponent();

            ShowNotify(Title, Body);
        }

        public void ShowNotify(string Title, string Body) {

            TitleMessage.Text = Title;
            BodyMessage.Text = Body;
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();
    
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
       
        }

        async void CloseNotification_Clicked(System.Object sender, System.EventArgs e)
        {
            await Navigation.PopPopupAsync();
        }
    }
}
