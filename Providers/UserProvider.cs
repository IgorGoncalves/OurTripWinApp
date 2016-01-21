using Newtonsoft.Json.Linq;
using OurTrip.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace OurTrip.Providers
{
    class MeProvider
    {
        public UserModel MyInformation(int myId)
        {

            UserModel newUser;

            List<Model.UserModel> users = new List<Model.UserModel>();
            using (HttpClient http = new HttpClient())
            {
                var url = String.Format("http://localhost:50842/User/{0}/me", myId);
                http.DefaultRequestHeaders.Accept.Add(
                                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                var response = http.GetAsync(url).Result;
                var result = response.Content.ReadAsStringAsync().Result;

                JObject friend = JObject.Parse(result.ToString());
                newUser = new Model.UserModel();
                newUser.Name = friend["us_name"].ToString();
                newUser.Age = int.Parse(friend["us_age"].ToString());
                newUser.Bio = friend["us_Bio"].ToString();
                newUser.Avatar = new BitmapImage(new Uri("ms-appx:///Assets/avatar.png", UriKind.RelativeOrAbsolute));
                newUser.Points = new List<Model.UserModel.UserPoint>();
                string pontoString;

                foreach (JObject location in friend["Location"].Children())
                {
                    pontoString = location["Point"]["Geography"]["WellKnownText"].ToString().Substring(7).Replace(')', ' ').Trim();
                    newUser.Points.Add(new Model.UserModel.UserPoint { Latitude = float.Parse(pontoString.Split(' ')[0]), Longitude = float.Parse(pontoString.Split(' ')[1]) });
                }


                return newUser;
            }
        }


    }
}
