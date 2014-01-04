using System;
using System.Threading.Tasks;
using OneBusAway.Model;
using OneBusAway.Services;
using Windows.Devices.Geolocation;

namespace OneBusAway.Platforms.WindowsPhone
{
    /// <summary>
    /// Finds the user's location.
    /// </summary>
    public class GeoLocationService : IGeoLocationService
    {
        /// <summary>
        /// This is the geo locator object that's used to find our location.
        /// </summary>
        private Geolocator geoLocator;

        /// <summary>
        /// Creates the service.
        /// </summary>
        public GeoLocationService()
        {
            this.geoLocator = new Geolocator();
        }

        /// <summary>
        /// Finds the user location.
        /// </summary>
        /// <returns></returns>
        public async Task<Point> FindUserLocationAsync()
        {
            var location = await geoLocator.GetGeopositionAsync();
            return new Point(location.Coordinate.Latitude, location.Coordinate.Longitude);
        }
    }
}
