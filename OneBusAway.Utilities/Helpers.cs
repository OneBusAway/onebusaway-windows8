using OneBusAway.Model.BingService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace OneBusAway.Utilities
{
    public static class Helpers
    {
        #region private variables

        static HttpClient client = new HttpClient();

        #endregion

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

            using (Stream responseStream = await client.GetStreamAsync(url))
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));

                return (T)serializer.ReadObject(responseStream);
            }
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

            queryParameters.Add(UtilitiesConstants.Parameter_BingServiceApiKey, UtilitiesConstants.BingsMapsServiceApiKey);

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
            if (response.ResourceSets.First().EstimatedTotal > 0)
            {
                List<T> results = new List<T>();

                var applicableResources = response.ResourceSets.First().Resources.Where(x =>
                {
                    Confidence confidence;
                    if (Enum.TryParse(x.Confidence, out confidence) && confidence >= minimumConfidence)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                });

                foreach (var resource in applicableResources)
                {
                    if (resource is T)
                    {
                        results.Add(resource as T);
                    }
                }

                return results;
            }
            else
            {
                return null;
            }
        }
    }
}
