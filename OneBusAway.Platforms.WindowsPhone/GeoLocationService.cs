using System;
using System.Device.Location;
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
        private GeoCoordinateWatcher geoLocator;

        /// <summary>
        /// Creates the service.
        /// </summary>
        public GeoLocationService()
        {
            this.geoLocator = new GeoCoordinateWatcher(GeoPositionAccuracy.High);
            this.geoLocator.Start();
        }

        /// <summary>
        /// Finds the user location.
        /// </summary>
        /// <returns></returns>
        public Task<Point> FindUserLocationAsync()
        {
            var location = this.geoLocator.Position.Location;
            return Task.FromResult<Point>(new Point(location.Latitude, location.Longitude));
        }
    }
}
