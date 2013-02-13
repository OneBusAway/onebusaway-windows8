using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneBusAway.ViewModels
{
    /// <summary>
    /// Constants for the view models assembly.
    /// </summary>
    public static class ViewModelConstants
    {
        /// <summary>
        /// Default map zoom when map control is loaded on any page
        /// </summary>
        public const double DefaultMapZoom = 12;

        /// <summary>
        /// Map zoom for a closer look than the default.
        /// </summary>
        public const double ZoomedInMapZoom = 15.5;

        /// <summary>
        /// Map zoom for the default closest zoom.
        /// </summary>
        public const double DefaultZoomedInMapZoom = 15.6;

        /// <summary>
        /// The name of the route data cache file.
        /// </summary>
        public const string CacheFileName = "RouteData.xml";

        /// <summary>
        /// The latitude of Seattle's city center.
        /// </summary>
        public const double SeattleLatitude = 47.603561401367188;

        /// <summary>
        /// The longitude of Seattle's city center.
        /// </summary>
        public const double SeattleLongitude = -122.32943725585937;

    }
}
