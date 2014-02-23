/* Copyright 2014 Michael Braude and individual contributors.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OneBusAway
{
    /// <summary>
    /// A collection of constants that are used thrughout the application.
    /// </summary>
    public static class Constants    
    {
        /// <summary>
        /// This is the length of time before we time out network calls.
        /// </summary>
        public const int HttpTimeoutLength = 15000;

        /// <summary>
        /// Map zoom for a closer look than the default.
        /// </summary>
        public const double ZoomedInMapZoom = 16;
        
        /// <summary>
        /// This is the API key for the application.
        /// </summary>
        public const string ObaApiKey = "693c0a55-9ef0-4302-8bc3-f9b2db93e124";
        
        /// <summary>
        /// API Key to access Bing Maps Service
        /// </summary>
        public const string BingsMapsServiceApiKey = "AtX8n80j9qDzHLBakqiTlTwvNm0pp9I9uO1wHfu4UyAXvZosECwuYGqYoGSkxnGT";
        
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
        public const double MinBusStopVisibleZoom = 14.5;

        /// <summary>
        /// Default latitude in case user position can't be found. Defaulting to Puget Sound area
        /// </summary>
        public const double DefaultLatitude = 47.567547867111273;

        /// <summary>
        /// Default longitude in case user position can't be found. Defaulting to Puget Sound area
        /// </summary>
        public const double DefaultLongitude = -122.97295385181809;

        /// <summary>
        /// Default number of seconds that we will keep the cache result for live data. Defaulting to 5 minutes
        /// </summary>
        public const int DefaultCacheAge = 3000;

        /// <summary>
        /// This constant should be used in cases where we do not want the document to be cached.
        /// </summary>
        public const int NoCacheAge = -1;
    }
}
