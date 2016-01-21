using NotificationsExtensions.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Services.Maps;
using Windows.Storage.Streams;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Navigation;

namespace OurTrip.Views
{
    public sealed partial class Friends : Page
    {
        IEnumerable<Model.UserModel> friends;
        public Model.UserModel ViewModel { get; set; }

        public Friends()
        {
            this.InitializeComponent();
        }

        Geopoint meuLocal = new Geopoint(new BasicGeoposition() { Latitude = 47.620, Longitude = -122.349 });

        private async void mapMark(IEnumerable<Model.UserModel> friends)
        {

            foreach (Model.UserModel item in friends)
            {
                MapIcon mapIcon1 = new MapIcon();
                Geopoint snPoint = new Geopoint(new BasicGeoposition() { Latitude = item.Points.LastOrDefault().Longitude, Longitude = item.Points.LastOrDefault().Latitude });

                mapIcon1.Location = snPoint;
                mapIcon1.Image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/MapPin.png"));
                mapIcon1.NormalizedAnchorPoint = new Point(0.5, 1.0);
                mapIcon1.Title = await GetDisplayName(snPoint);
                mapIcon1.ZIndex = 0;

                // Add the MapIcon to the map.
                myMap.MapElements.Add(mapIcon1);
                // Center the map over the POI.                
                // myMap.ZoomLevel = 14;
            }
        }

        private void gridView_ItemClick(object sender, ItemClickEventArgs e)
        {

            var item = e.ClickedItem as Model.UserModel;
            myMap.Center = new Geopoint(new BasicGeoposition { Longitude = item.Points.LastOrDefault().Latitude, Latitude = item.Points.LastOrDefault().Longitude });
            myMap.ZoomLevel = 14;
            this.NotificationToast(String.Format("{0} respondeu seu Ahoy!", item.Name), item.Avatar.UriSource.LocalPath);
            this.NotificationTile(String.Format("{0} respondeu seu Ahoy!", item.Name), item.Avatar.UriSource.LocalPath);

        }



        private void NotificationTile(string text, string pathImage)
        {
            var notificationXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileSquare150x150Text01);            
            var toastElements = notificationXml.GetElementsByTagName("text");
            toastElements[0].AppendChild(notificationXml.CreateTextNode(text));            

            var notification = new TileNotification(notificationXml);
            TileUpdateManager.CreateTileUpdaterForApplication().Update(notification);
            


        }


        private void NotificationToast(string text, string pathImage)
        {
            //string mensagem = "Hey!";
            //string image = @"Assets/Logo.png";

            var notificationXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastImageAndText01);

            var toastElements = notificationXml.GetElementsByTagName("text");
            toastElements[0].AppendChild(notificationXml.CreateTextNode(text));

            var imageElements = notificationXml.GetElementsByTagName("image");
            imageElements[0].Attributes[1].NodeValue = pathImage;

            var toastNotification = new ToastNotification(notificationXml);
            ToastNotificationManager.CreateToastNotifier().Show(toastNotification);

        }

        private async Task<string> GetDisplayName(Geopoint pointToReverseGeocode)
        {
            try
            {

                MapLocationFinderResult result = await MapLocationFinder.FindLocationsAtAsync(pointToReverseGeocode);
                // If the query returns results, display the name of the town
                // contained in the address of the first result.
                if (result.Status == MapLocationFinderStatus.Success)
                {
                    return result.Locations[0].Address.Town;
                }

            }
            catch (Exception e)
            {

                throw;
            }

            return "Unknow";
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is Geoposition)
            {
                Geoposition point = (Geoposition)e.Parameter;
                this.friends = new Providers.FriendsProvider().FindFriends(point.Coordinate.Point.Position.Latitude, point.Coordinate.Point.Position.Longitude, 50, 1);
                this.gridView.ItemsSource = this.friends;
                mapMark(this.friends);
            }
            base.OnNavigatedTo(e);
        }



    }
}
