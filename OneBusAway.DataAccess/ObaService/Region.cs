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
    /// Represents a region that OBA supports.
    /// </summary>
    public class Region : BindableBase
    {
        private const double EarthsRadiusInKM = 6371;

        private string regionName;
        private string regionUrl;
        private bool supportsObaRealtimeApis;
        private bool supportsObaDiscoveryApis;
        private bool isActive;
        private List<RegionBounds> regionBounds;

        /// <summary>
        /// Creates the region.
        /// </summary>
        /// <param name="regionElement">The region element.</param>
        public Region(XElement regionElement)
        {
            this.RegionName = regionElement.GetFirstElementValue<string>("regionName");
            this.RegionUrl = regionElement.GetFirstElementValue<string>("obaBaseUrl");
            this.SupportsObaRealtimeApis = regionElement.GetFirstElementValue<bool>("supportsObaRealtimeApis");
            this.SupportsObaDiscoveryApis = regionElement.GetFirstElementValue<bool>("supportsObaDiscoveryApis");
            this.IsActive = regionElement.GetFirstElementValue<bool>("active");
            this.RegionBounds = new List<RegionBounds>(from boundsElement in regionElement.Descendants("bound")
                                                       select new RegionBounds(boundsElement));
        }        
        
        /// <summary>
        /// Gets / sets the region URI.
        /// </summary>
        public string RegionName
        {
            get
            {
                return this.regionName;
            }
            set
            {
                SetProperty(ref this.regionName, value);
            }
        }

        /// <summary>
        /// Gets / sets the region URI.
        /// </summary>
        public string RegionUrl
        {
            get
            {
                return this.regionUrl;
            }
            set
            {
                SetProperty(ref this.regionUrl, value);
            }
        }

        /// <summary>
        /// True when we support real time Apis
        /// </summary>
        public bool SupportsObaRealtimeApis
        {
            get
            {
                return this.supportsObaRealtimeApis;
            }
            set
            {
                SetProperty(ref this.supportsObaRealtimeApis, value);
            }
        }

        /// <summary>
        /// Returns true if the region supports discovery APIs
        /// </summary>
        public bool SupportsObaDiscoveryApis
        {
            get
            {
                return this.supportsObaDiscoveryApis;
            }
            set
            {
                SetProperty(ref this.supportsObaDiscoveryApis, value);
            }
        }

        /// <summary>
        /// True when the region is active.
        /// </summary>
        public bool IsActive
        {
            get
            {
                return this.isActive;
            }
            set
            {
                SetProperty(ref this.isActive, value);
            }
        }

        /// <summary>
        /// Gets / sets the regions bounds.
        /// </summary>
        public List<RegionBounds> RegionBounds
        {
            get
            {
                return this.regionBounds;
            }
            set
            {
                SetProperty(ref this.regionBounds, value);
            }
        }

        /// <summary>
        /// Returns the distance from this region's closest bounds.
        /// </summary>
        public double DistanceFrom(double latitude, double longitude)
        {
            double closestRegion = double.MaxValue;
            foreach (var bounds in this.RegionBounds)
            {
                closestRegion = Math.Min(closestRegion, DistanceFromBoundsInKM(latitude, longitude, bounds));
            }

            return closestRegion;
        }

        /// <summary>
        /// Returns the great circle distance between two coordinates on a sphere. From
        /// http://en.wikipedia.org/wiki/Great-circle_distance
        /// and
        /// http://stackoverflow.com/questions/6544286/calculate-distance-of-two-geo-points-in-km-c-sharp
        /// </summary>
        private double DistanceFromBoundsInKM(double latitude, double longitude, RegionBounds bounds)
        {
            double sLat1 = Math.Sin(DegreesToRadians(latitude));
            double sLat2 = Math.Sin(DegreesToRadians(bounds.Latitude));
            double cLat1 = Math.Cos(DegreesToRadians(latitude));
            double cLat2 = Math.Cos(DegreesToRadians(bounds.Latitude));
            double cLon = Math.Cos(DegreesToRadians(longitude) - DegreesToRadians(bounds.Longitude));

            return EarthsRadiusInKM * Math.Acos(sLat1 * sLat2 + cLat1 * cLat2 * cLon);
        }

        /// <summary>
        /// Converts degrees to radians.
        /// </summary>
        private static double DegreesToRadians(double degrees)
        {
            return Math.PI * (degrees / 360.0);
        }
    }
}
