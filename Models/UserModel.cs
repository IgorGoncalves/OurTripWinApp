using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace OurTrip.Model
{
    
    public class UserModel
    {
        
        public BitmapImage Avatar { get; set; }
        public String Name { get; set; }
        public int Age { get; set; }
        public String Bio { get; set; }
        public List<UserPoint> Points { get; set; }

        //public UserModel()
        //{
        //    this.Avatar = new BitmapImage(new Uri("ms-appx:///Assets/Lrocf4 1920x1080.png", UriKind.RelativeOrAbsolute));
        //    this.Name = "Igor Gonçalves";
        //    this.Age = 20;
        //    this.Bio = "Viajante do tempo e espaço";
        //    this.Points = new List<UserPoint>();
        //    this.Points.Add(new UserPoint { Latitude = 2.8F, Longitude = 10.3F, type = 0 });

        //}

        //public UserModel(string avatarUrl, string name, int age, string bio, List<UserPoint> points = null)
        //{
        //    //this.Avatar = new BitmapImage(new Uri(avatarUrl, UriKind.RelativeOrAbsolute));
        //    this.Name = name;
        //    this.Age = age;
        //    this.Bio = bio;
        //    this.Points = points == null ? new List<UserPoint>() : points;
        //}



        public string NameAndAge
        {
            get
            {
                return $"{this.Name}, {this.Age} anos";

            }
        }

        public class UserPoint
        {
            public double Latitude { get; set; }
            public double Longitude { get; set; }            
            public int type { get; set; }
        }


    }

    public class UserModelDAO
    {
        public void GetUser()
        {

        }
        
        private static UserModelDAO _InstanceDAO = new UserModelDAO();

        public static UserModelDAO InstanceDAO()
        {
            return _InstanceDAO;
        }

    }


}
