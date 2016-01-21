using OurTrip.Providers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Services.Maps;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;

namespace OurTrip.Views
{
    public sealed partial class Destinies : Page
    {
        IEnumerable<Model.Location> locations = new ObservableCollection<Model.Location>();

        public Destinies()
        {
            this.InitializeComponent();
            LocationProvider providerLocation = new LocationProvider();
            locations = providerLocation.MyLocations(1);
            this.listView.ItemsSource = locations;
            mapPoits(myMap, locations);
        }

        public async void mapPoits(MapControl map, IEnumerable<Model.Location> locations)
        {

            foreach (Model.Location location in locations)
            {
                MapIcon mapIcon = new MapIcon();
                mapIcon.NormalizedAnchorPoint = new Point(0.5, 1.0);
                mapIcon.Title = await GetDisplayName(location.Coordenadas);
                mapIcon.Location = location.Coordenadas;
                mapIcon.Image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/MapPin.png"));
                mapIcon.ZIndex = 0;
                myMap.MapElements.Add(mapIcon);
            }

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

        public ObservableCollection<Model.Location> Locations
        {
            get
            {
                return (ObservableCollection<Model.Location>)locations;
            }
        }

        private void listView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var item = e.ClickedItem as Model.Location;
            myMap.Center = new Geopoint(new BasicGeoposition { Longitude = item.Coordenadas.Position.Longitude, Latitude = item.Coordenadas.Position.Latitude });
            myMap.ZoomLevel = 14;

        }

        
    }
}
