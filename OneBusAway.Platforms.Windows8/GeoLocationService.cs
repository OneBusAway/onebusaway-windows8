using OneBusAway.Model;
using OneBusAway.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace OneBusAway.Platforms.Windows8
{
    public class GeoLocationService : IGeoLocationService
    {
        public async Task<Point> FindUserLocationAsync()
        {
            Geolocator locator = new Geolocator();
            Geoposition geoPosition = await locator.GetGeopositionAsync();
            BasicGeoposition position = geoPosition.Coordinate.Point.Position;
            return new Point(position.Latitude, position.Longitude);
        }
    }
}
