using System;

using Xamarin.Forms;

namespace Ecombeta.Models
{
    public class Flashcart : ContentPage
    {
        public Flashcart()
        {
            Content = new StackLayout
            {
                Children = {
                    new Label { Text = "Hello ContentPage" }
                }
            };
        }
    }
}

