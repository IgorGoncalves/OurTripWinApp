using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace OurTrip.Model
{
    public class Location 
    {
        public Geopoint Coordenadas { get; set; }
        public String TitleName { get; set; }
        public String Description { get; set; }

    }
}
