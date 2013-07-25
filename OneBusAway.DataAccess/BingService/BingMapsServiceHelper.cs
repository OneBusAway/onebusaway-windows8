/* Copyright 2013 Microsoft
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
using OneBusAway.Model.BingService;
using OneBusAway.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace OneBusAway.DataAccess.BingService
{
    /// <summary>
    /// Class that provides helper methods to interface with the Bing Maps Service
    /// </summary>
    public class BingMapsServiceHelper
    {
        /// <summary>
        /// Returns a static image from Bing for a specific lat / long and with a specific height.
        /// </summary>
        /// <param name="latitude">The latitude</param>
        /// <param name="longitude">The longitude</param>
        /// <param name="imageWidth">The image width</param>
        /// <param name="imageHeight">The image height</param>
        /// <returns></returns>
        public async Task<MemoryStream> GetStaticImageBytesAsync(double latitude, double longitude, int imageWidth, int imageHeight)
        {
            // Get the image from Bing and save to disk.
            string url = string.Format("http://dev.virtualearth.net/REST/v1/Imagery/Map/Road/{0},{1}/16?mapSize={2},{3}&pushpin={0},{1};39&format=png&key={4}",
                latitude,
                longitude,
                imageWidth,
                imageHeight,
                UtilitiesConstants.BingsMapsServiceApiKey);

            WebRequest request = HttpWebRequest.Create(url);
            request.Method = "GET";
            var response = await request.GetResponseAsync();

            MemoryStream memoryStream = new MemoryStream();
            using (var stream = response.GetResponseStream())
            {
                await stream.CopyToAsync(memoryStream);
            }

            return memoryStream;
        }

        /// <summary>
        /// Queries the Bing Maps Service with a given search query, confidence and userLocation and returns a list of locations that match the query
        /// </summary>
        /// <param name="query">Search query</param>
        /// <param name="minimumConfidence">Minimum confidence by which to filter the search results</param>
        /// <param name="userPosition">User's location that is used to improve search results</param>
        /// <returns></returns>
        public static async Task<List<Location>> GetLocationByQuery(string query, Confidence minimumConfidence, OneBusAway.Model.Point userPosition)
        {
            if (string.IsNullOrEmpty(query))
            {
                throw new ArgumentNullException(query);
            }

            Dictionary<string, string> queryParameters = Helpers.GetBasicParameters();
            queryParameters.Add(UtilitiesConstants.Parameter_Query, query);

            if (userPosition != null)
            {
                queryParameters.Add(UtilitiesConstants.Parameter_UserLocation, userPosition.Latitude + "," + userPosition.Longitude);
            }

            string url = Helpers.CreateServiceUrl(UtilitiesConstants.BingLocationServiceBaseAddress, queryParameters);

            Response response = await Helpers.GetJsonResponse<Response>(url);

            return Helpers.FilterResults<Location>(response, minimumConfidence);
        }        
    }
}
