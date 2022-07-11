using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Firebase.Database;
using Firebase.Database.Query;
using System.Linq;

namespace Ecombeta.Views
{
    public class NotifyFeed {
        public string FCMImage { get; set; }
        public string Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
    }

    public partial class NotificationFeed : ContentPage
    {
        public FirebaseClient firebase = new FirebaseClient("https://mica-marketapp.firebaseio.com/");
        public NotificationFeed()
        {
            InitializeComponent();
            Init();
        }

        public async Task Init()
        {
            var _feed = await GetAllFeed();
            Feedlist.ItemsSource = _feed;
        }

        public async Task<List<NotifyFeed>> GetAllFeed()
        {
           return (await firebase
              .Child("Notifications")
              .OnceAsync<NotifyFeed>()).Select(item => new NotifyFeed
              {
                  Message = item.Object.Message,
                  Id = item.Object.Id,
                  FCMImage = item.Object.FCMImage,
                  Title = item.Object.Title,
              }).ToList();
        }
    }
}
