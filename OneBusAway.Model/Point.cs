using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneBusAway.Model
{
    /// <summary>
    /// A point is a latitude / longitude pair that's displayed as a path on the map.
    /// </summary>
    public class Point : BindableBase
    {
        private double latitude;
        private double longitude;

        public Point()
        {
        }

        public Point(double _latitude, double _longitude)
        {
            latitude = _latitude;
            longitude = _longitude;
        }

        public double Latitude
        {
            get
            {
                return this.latitude;
            }
            set
            {
                SetProperty(ref this.latitude, value);
            }
        }

        public double Longitude
        {
            get
            {
                return this.longitude;
            }
            set
            {
                SetProperty(ref this.longitude, value);
            }
        }
    }
}
