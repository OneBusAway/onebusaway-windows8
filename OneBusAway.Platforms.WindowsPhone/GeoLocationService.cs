using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OneBusAway.Model;
using OneBusAway.Services;

namespace OneBusAway.Platforms.WindowsPhone
{
    /// <summary>
    /// Finds the user's location.
    /// </summary>
    public class GeoLocationService : IGeoLocationService
    {
        private GeoLocationService service;

        /// <summary>
        /// Creates the service.
        /// </summary>
        public GeoLocationService()
        {
            this.service = new GeoLocationService();            
        }

        /// <summary>
        /// Finds the user location.
        /// </summary>
        /// <returns></returns>
        public async Task<Point> FindUserLocationAsync()
        {
            var location = await service.FindUserLocationAsync();
            return new Point(location.Latitude, location.Longitude);
        }
    }
}
