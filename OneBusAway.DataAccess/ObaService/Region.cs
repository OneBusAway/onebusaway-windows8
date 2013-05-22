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
            this.IsActive = regionElement.GetFirstElementValue<bool>("active");
            this.RegionBounds = new List<RegionBounds>(from boundsElement in regionElement.Descendants("bound")
                                                       select new RegionBounds(boundsElement));
        }

        /// <summary>
        /// Returns true if any of the regions fall inside the lat / lon.
        /// </summary>
        /// <param name="latitude">The latitide</param>
        /// <param name="longitude">The longitude</param>
        /// <returns>True if it falls inside</returns>
        public bool FallsInside(double latitude, double longitude)
        {
            return this.RegionBounds.Any(regionBounds => regionBounds.FallsInside(latitude, longitude));
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
