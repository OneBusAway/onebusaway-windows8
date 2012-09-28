using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OneBusAway.DataAccess
{
    /// <summary>
    /// This class wraps HttpWebRequest and makes it easier to read / write data to a REST web service.
    /// </summary>
    public class ObaServiceHelperFactory
    {
        /// <summary>
        /// This is the URL of the service that we're talking to.
        /// </summary>
        private string serviceUrl;

        /// <summary>
        /// Creates the service helper.
        /// </summary>
        public ObaServiceHelperFactory(string serviceUrl)
        {
            this.serviceUrl = serviceUrl;
        }

        /// <summary>
        /// Factory method creates a service helper.
        /// </summary>
        public virtual IObaServiceHelper CreateHelper(ObaMethod obaMethod, HttpMethod httpMethod)
        {
            return new ObaServiceHelper(this.serviceUrl, obaMethod, httpMethod);
        }

        /// <summary>
        /// Private implementation so that clients are forced to use the create method to talk to a OBA web service.
        /// </summary>
        private class ObaServiceHelper : IObaServiceHelper
        {
            /// <summary>
            /// This Uri builder is used to create the URI of the OBA REST service.
            /// </summary>
            private UriBuilder uriBuilder;

            /// <summary>
            /// Creates the web request.
            /// </summary>
            private HttpWebRequest request;

            /// <summary>
            /// The http method.
            /// </summary>
            private HttpMethod httpMethod;

            /// <summary>
            /// Maps name / value pairs to the query string.
            /// </summary>
            private Dictionary<string, string> queryStringMap;

            /// <summary>
            /// Creates the service helper.
            /// </summary>
            public ObaServiceHelper(string serviceUrl, ObaMethod obaMethod, HttpMethod httpMethod)
            {
                this.httpMethod = httpMethod;
                this.uriBuilder = new UriBuilder(serviceUrl);

                string obaMethodString = obaMethod.ToString();
                obaMethodString = obaMethodString.Replace('_', '-');
                obaMethodString += ".xml";
                this.uriBuilder.Path += obaMethodString;

                this.queryStringMap = new Dictionary<string, string>();
                this.queryStringMap["key"] = Constants.API_KEY;
            }
            
            /// <summary>
            /// Adds a name / value pair to the query string.
            /// </summary>
            public void AddToQueryString(string name, string value)
            {
                this.queryStringMap[name] = value;
            }

            /// <summary>
            /// Sends a payload to the service asynchronously.
            /// </summary>
            public async Task<string> SendAndRecieveAsync(string payload)
            {
                this.uriBuilder.Query = this.CreateQueryString();
                this.request = WebRequest.CreateHttp(this.uriBuilder.Uri);
                this.request.Method = this.httpMethod.ToString();

                if (this.httpMethod == HttpMethod.POST)
                {
                    Stream requestStream = await this.request.GetRequestStreamAsync();
                    using (var streamWriter = new StreamWriter(requestStream))
                    {
                        await streamWriter.WriteAsync(payload);
                    }
                }

                var response = await this.request.GetResponseAsync();
                var responseStream = response.GetResponseStream();

                using (var streamReader = new StreamReader(responseStream))
                {
                    return await streamReader.ReadToEndAsync();
                }
            }

            /// <summary>
            /// Creates the query string out of the current queryStringMap object.
            /// </summary>
            private string CreateQueryString()
            {
                return string.Join("&", from keyValuePair in this.queryStringMap
                                        select string.Format(CultureInfo.CurrentCulture, "{0}={1}", keyValuePair.Key, keyValuePair.Value));

            }
        }
    }
}
