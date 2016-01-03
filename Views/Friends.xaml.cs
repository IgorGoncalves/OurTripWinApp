using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace NavigationMenuSample.Views
{
    public sealed partial class Friends : Page
    {
        public Friends()
        {
            this.InitializeComponent();
            myMap.MapServiceToken = "hhMv8ajbsNkjTqZbNmIYHQ";
            myMap.Loaded += MyMap_Loaded;
        }
        private void MyMap_Loaded(object sender, RoutedEventArgs e)
        {
            myMap.Center =
               new Geopoint(new BasicGeoposition()
               {
                   //Geopoint for Seattle 
                   Latitude = 47.604,
                   Longitude = -122.329
               });
            myMap.ZoomLevel = 17;
            
        }

    }
}
