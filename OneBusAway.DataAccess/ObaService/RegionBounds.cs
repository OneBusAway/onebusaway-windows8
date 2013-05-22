using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using OneBusAway.Model;

namespace OneBusAway.DataAccess.ObaService
{
    /// <summary>
    /// Represents a regions bounds.
    /// </summary>
    public class RegionBounds : BindableBase
    {
        private double regionLatitude;
        private double regionLatitudeSpan;
        private double regionLongitude;
        private double regionLongitudeSpan;

        /// <summary>
        /// Creates the region bounds.
        /// </summary>
        /// <param name="boundsElement">The bounds element</param>
        public RegionBounds(XElement boundsElement)
        {
            this.regionLatitude = boundsElement.GetFirstElementValue<double>("lat");
            this.regionLatitudeSpan = boundsElement.GetFirstElementValue<double>("latSpan");
            this.regionLongitude = boundsElement.GetFirstElementValue<double>("lon");
            this.regionLongitudeSpan = boundsElement.GetFirstElementValue<double>("lonSpan");
        }

        /// <summary>
        /// Returns true if the lat / lon falls inside this region's bounds.
        /// </summary>
        /// <param name="latitude">The lat</param>
        /// <param name="longitude">The lon</param>
        /// <returns>True if it falls inside the bounds</returns>
        public bool FallsInside(double latitude, double longitude)
        {
            return (regionLatitude <= latitude && latitude <= regionLatitude + regionLatitudeSpan)
                && (regionLongitude <= longitude && longitude <= regionLongitude + regionLongitudeSpan);
        }
    }
}
