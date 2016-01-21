using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Services.Maps;
using Windows.UI.Xaml.Media.Imaging;

namespace OurTrip.Providers
{
    class LocationProvider
    {
        public IEnumerable<Model.Location> MyLocations(int myId)
        {
            Model.Location newLocation;
            try
            {
                List<Model.Location> locations = new List<Model.Location>();
                using (HttpClient http = new HttpClient())
                {
                    var url = String.Format("http://localhost:50842/User/{0}/locations", myId);
                    http.DefaultRequestHeaders.Accept.Add(
                                        new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    var response = http.GetAsync(url).Result;
                    var result = response.Content.ReadAsStringAsync().Result;

                    JArray locationsJson = JArray.Parse(result.ToString());
                    newLocation = new Model.Location();
                    string pontoString;



                    foreach (JObject locationJson in locationsJson.Children())
                    {
                        pontoString = locationJson["Point"]["Geography"]["WellKnownText"].ToString().Substring(7).Replace(')', ' ').Trim();
                        Geopoint point = new Geopoint(new BasicGeoposition
                        {
                            Latitude = float.Parse(pontoString.Split(' ')[1]),
                            Longitude = float.Parse(pontoString.Split(' ')[0]),
                        });

                        locations.Add(
                            new Model.Location
                            {
                                Coordenadas = point,
                                TitleName = locationJson["Title"].ToString()
                            });

                    }

                }

                return locations;

            }
            // serialize JSON results into .NET objects            
            catch (Exception e)
            {

                throw;
            }

            return null;

        }

        public async void AddLocation(Geopoint geo, int myId)
        {
            using (HttpClient http = new HttpClient())
            {
                string tower = await GetDisplayName(geo);

                var url = String.Format("http://localhost:50842/Location/add/{0}/{1}/{2}/{3}", geo.Position.Latitude, geo.Position.Longitude, tower, myId);
                http.DefaultRequestHeaders.Accept.Add(
                                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                var response = await http.GetAsync(url);
                var result = await response.Content.ReadAsStringAsync();
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
    }
}
