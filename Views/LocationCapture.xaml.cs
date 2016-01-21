using OurTrip.Providers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace OurTrip.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BasicSubPage : Page
    {
        public BasicSubPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is string)
            {
                this.Message = String.Format("Clicked on {0}", e.Parameter);
            }
            else
            {
                this.Message = "Hi!";
            }

            base.OnNavigatedTo(e);
        }

        public string Message { get; set; }

        async private void button_Click(object sender, RoutedEventArgs e)
        {
            LocationProvider providerLocation = new LocationProvider();

            var accessStatus = await Geolocator.RequestAccessAsync();

            switch (accessStatus)
            {
                case GeolocationAccessStatus.Allowed:

                    // If DesiredAccuracy or DesiredAccuracyInMeters are not set (or value is 0), DesiredAccuracy.Default is used.
                    Geolocator geolocator = new Geolocator { DesiredAccuracyInMeters = 2000 };

                    // Carry out the operation.
                    Geoposition pos = await geolocator.GetGeopositionAsync();
                    Geopoint geo = new Geopoint(new BasicGeoposition { Latitude = pos.Coordinate.Point.Position.Latitude, Longitude = pos.Coordinate.Point.Position.Longitude });

                    providerLocation.AddLocation(geo, 1);


                    this.Frame.Navigate(typeof(Friends), pos);

                    break;

                case GeolocationAccessStatus.Denied:

                    break;

                case GeolocationAccessStatus.Unspecified:

                    break;
            }

        }


    }
}
