using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OneBusAway.Utilities
{
    /// <summary>
    /// A collection of constants that are used thrughout the application.
    /// </summary>
    public static class UtilitiesConstants    
    {
        /// <summary>
        /// This is the API key for the application.
        /// </summary>
        public const string API_KEY = "693c0a55-9ef0-4302-8bc3-f9b2db93e124";

        /// <summary>
        /// This is the URI of the server.
        /// </summary>
        public const string SERVER_URL = "http://api.onebusaway.org/api/where/";

        /// <summary>
        /// API Key to access Bing Maps Service
        /// </summary>
        public const string BingsMapsServiceApiKey = "AvU-uxo8TYI1W27CBfXD2He_ZJ2T_yYcPbbfPgn9x-0HVVJHqbCfbB69wEYECRmt";

        /// <summary>
        /// Key to use the Bing Maps control
        /// </summary>
        public const string BingMapCredentials = "AoUHJXTeSKEqI4xCm_HWb3GEmQY_rLTAOKlhGgzZFCYpZsU-BcD_IUWE0PcjGGgr";

        /// <summary>
        /// Base address to access the REST based Bing location service
        /// </summary>
        public const string BingLocationServiceBaseAddress = "http://dev.virtualearth.net/REST/v1/Locations";

        /// <summary>
        /// key that denotes the query part of the url
        /// </summary>
        public const string Parameter_Query = "q";

        /// <summary>
        /// key that denotes the userLocation part of the url. This is optional
        /// </summary>
        public const string Parameter_UserLocation = "ul";

        /// <summary>
        /// key that denotes the apiKey when calling the Bing Maps Service
        /// </summary>
        public const string Parameter_BingServiceApiKey = "key";

        /// <summary>
        /// Minimum zoom required for bus stops to be visible
        /// </summary>
        public const double MinBusStopVisibleZoom = 15.0;

        /// <summary>
        /// Default latitude in case user position can't be found. Defaulting to Puget Sound area
        /// </summary>
        public const double DefaultLatitude = 47.567547867111273;

        /// <summary>
        /// Default longitude in case user position can't be found. Defaulting to Puget Sound area
        /// </summary>
        public const double DefaultLongitude = -122.97295385181809;
    }
}
