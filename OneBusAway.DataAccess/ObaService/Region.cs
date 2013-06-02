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
        /// Returns the distance from this region's closest bounds.
        /// </summary>
        public double DistanceFrom(double latitude, double longitude)
        {
            double closestRegion = double.MaxValue;
            foreach (var bounds in this.RegionBounds)
            {
                double x = latitude - bounds.Latitude;
                double y = longitude - bounds.Longitude;
                double distance = Math.Sqrt(x * x + y * y);
                closestRegion = Math.Min(closestRegion, distance);
            }

            return closestRegion;
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
    }
}
