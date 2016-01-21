using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml.Media.Imaging;

namespace OurTrip.Providers
{
    class FriendsProvider
    {
        public IEnumerable<Model.UserModel> FindFriends(double latitude, double longitude, int distance, int myId)
        {
            try
            {
                List<Model.UserModel> users = new List<Model.UserModel>();
                using (HttpClient http = new HttpClient())
                {
                    var url = String.Format("http://localhost:50842/api/friends/{0}/{1}/{2}/{3}/", latitude, longitude, distance, myId);
                    http.DefaultRequestHeaders.Accept.Add(
                                        new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    var response = http.GetAsync(url).Result;
                    var result = response.Content.ReadAsStringAsync().Result;

                    JArray friends = JArray.Parse(result.ToString());

                    foreach (JObject friend in friends.Children())
                    {
                        Model.UserModel newUser = new Model.UserModel();
                        newUser.Name = friend["us_name"].ToString();
                        newUser.Age = int.Parse(friend["us_age"].ToString());
                        newUser.Bio = friend["us_Bio"].ToString();
                        newUser.Avatar = new BitmapImage(new Uri("ms-appx:///Assets/avatar_Laize.jpg", UriKind.RelativeOrAbsolute));
                        newUser.Points = new List<Model.UserModel.UserPoint>();
                        string pontoString;
                        foreach (JObject location in friend["Location"].Children())
                        {
                            pontoString = location["Point"]["Geography"]["WellKnownText"].ToString().Substring(7).Replace(')', ' ').Trim();
                            newUser.Points.Add(new Model.UserModel.UserPoint { Latitude = float.Parse(pontoString.Split(' ')[0]), Longitude = float.Parse(pontoString.Split(' ')[1]) });
                        }
                       
                        users.Add(newUser);

                    }
                }

                return users;

            }
            // serialize JSON results into .NET objects            
            catch (Exception e)
            {

                throw;
            }
            return null;
        }
    }
}
