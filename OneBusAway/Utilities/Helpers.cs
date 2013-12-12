/* Copyright 2013 Michael Braude and individual contributors.
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
using OneBusAway.Shared.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace OneBusAway.Utilities
{
    public static class Helpers
    {
        /// <summary>
        /// Queries a REST service and returns a deserialized version of the Json response
        /// </summary>
        /// <typeparam name="T">Type of the response object</typeparam>
        /// <param name="url">Url to be queried</param>
        /// <returns>Deserialized object representing the Json response</returns>
        public static async Task<T> GetJsonResponse<T>(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException(url);
            }

            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
            return (T)serializer.ReadObject(await ServiceRepository.NetworkService.ReadAsStreamAsync(url));
        }

        /// <summary>
        /// Creates a service url given a base address and a dictionary of query parameters
        /// </summary>
        /// <param name="baseAddress">The base address for the service</param>
        /// <param name="queryParameters">A dictionary of key value pairs that are to be encoded and appended along with base address</param>
        /// <returns></returns>
        public static string CreateServiceUrl(string baseAddress, Dictionary<string, string> queryParameters)
        {
            if (string.IsNullOrEmpty(baseAddress))
            {
                throw new ArgumentNullException(baseAddress);
            }

            StringBuilder sb = new StringBuilder();
            sb.Append(baseAddress);
            sb.Append("?");

            if (queryParameters != null)
            {
                foreach (KeyValuePair<string, string> parameter in queryParameters)
                {
                    if (parameter.Key != null && parameter.Value != null)
                    {
                        sb.AppendFormat("&{0}={1}", EncodeValue(parameter.Key), EncodeValue(parameter.Value));
                    }
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Encodes a string
        /// </summary>
        /// <param name="unEncodedUrl"></param>
        /// <returns></returns>
        private static string EncodeValue(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(value);
            }

            string acceptedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._~";

            StringBuilder encodedValue = new StringBuilder();
            for (int i = 0; i < value.Length; i++)
            {
                if (acceptedChars.IndexOf(value[i]) >= 0)
                {
                    encodedValue.Append(value[i]);
                }
                else
                {
                    encodedValue.Append("%");
                    var bytes = Encoding.UTF8.GetBytes(new char[] { value[i] });

                    for (int j = 0; j < bytes.Length; j++)
                    {
                        encodedValue.Append(bytes[0].ToString("X2"));
                    }
                }
            }

            return encodedValue.ToString();
        }

        /// <summary>
        /// Gets a basic set of query parameters that are common to all requests in key, value form
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetBasicParameters()
        {
            Dictionary<string, string> queryParameters = new Dictionary<string, string>();

            queryParameters.Add(Constants.Parameter_BingServiceApiKey, Constants.BingsMapsServiceApiKey);

            return queryParameters;
        }

        /// <summary>
        /// Filters a given Response for the specified minimumConfidence and required type T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="response"></param>
        /// <param name="minimumConfidence"></param>
        /// <returns></returns>
        public static List<T> FilterResults<T>(Response response, Confidence minimumConfidence) where T : Location
        {
            List<T> results = new List<T>();
            ResourceSet resourceSet = response.ResourceSets[0];

            if (resourceSet.EstimatedTotal > 0)
            {
                foreach (T location in resourceSet.Resources.OfType<T>())
                {
                    Confidence confidence;
                    if (Enum.TryParse(location.Confidence, out confidence) && confidence >= minimumConfidence)
                    {
                        // Filter results to Washington State, USA:
                        if (string.Equals("United States", location.Address.CountryRegion, StringComparison.OrdinalIgnoreCase))
                        {
                            results.Add(location);
                        }
                    }
                }
            }

            return results;
        }
    }
}
