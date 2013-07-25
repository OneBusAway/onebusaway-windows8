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
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using OneBusAway.Model;
using OneBusAway.Utilities;
using Windows.Storage;
using System.Threading;
using System.Reflection;

namespace OneBusAway.DataAccess.ObaService.ObaService
{
    /// <summary>
    /// This class wraps HttpWebRequest and makes it easier to read / write data to a REST web service.
    /// </summary>
    public class ObaServiceHelperFactory
    {
        /// <summary>
        /// This is the URL of the regions web service.
        /// </summary>
        private const string REGIONS_SERVICE_URI = "http://regions.onebusaway.org/regions.xml";

        /// <summary>
        /// This is the name of the regions XML file.
        /// </summary>
        private const string REGIONS_XML_FILE = "Regions.xml";

        /// <summary>
        /// A task that this class will wait on until we have the regions
        /// </summary>
        private static Task<Region[]> regionsLookupTask;

        /// <summary>
        /// This is the users longitude.
        /// </summary>
        private double usersLongitude;

        /// <summary>
        /// This is the user latitude.
        /// </summary>
        private double usersLatitude;

        /// <summary>
        /// Static constructor creates the regions task.
        /// </summary>
        static ObaServiceHelperFactory()
        {
            regionsLookupTask = Task.Run(async () =>
                {
                    XDocument doc = null;
                    bool existingFileHasExpired = false;

                    try
                    {
                        // Try and load the regions xml file locally:
                        try
                        {
                            // Expire the regions xml file after 7 days:                            
                            var existingFile = await ApplicationData.Current.LocalFolder.GetFileAsync(REGIONS_XML_FILE);

                            using (var stream = await existingFile.OpenStreamForReadAsync())
                            {
                                doc = XDocument.Load(stream);
                            }

                            existingFileHasExpired = (DateTime.Now - existingFile.DateCreated).TotalDays >= 7;
                        }
                        catch
                        {
                            // OK, couldn't load.
                        }

                        // If we either don't have a file, or the existing one is too old, try
                        // and get the newest version. If this fails, we can default to using 
                        // the old document until the server is back!
                        if (doc == null || existingFileHasExpired)
                        {
                            var webRequest = WebRequest.CreateHttp(REGIONS_SERVICE_URI);

                            var response = await webRequest.GetResponseAsync();
                            var responseStream = response.GetResponseStream();

                            using (var streamReader = new StreamReader(responseStream))
                            {
                                string xml = await streamReader.ReadToEndAsync();
                                doc = XDocument.Parse(xml);
                            }

                            try
                            {
                                var newFile = await ApplicationData.Current.LocalFolder.CreateFileAsync(REGIONS_XML_FILE, CreationCollisionOption.ReplaceExisting);
                                using (var stream = await newFile.OpenStreamForWriteAsync())
                                {
                                    doc.Save(stream);
                                }
                            }
                            catch
                            {
                                // OK, couldn't save.
                                // Who knows why. We have a document so let's not fail here
                            }
                        }
                    }
                    catch
                    {
                    }

                    // If we make it here, use the backup regions.xml file:
                    if (doc == null)
                    {
                        Assembly assembly = typeof(ObaServiceHelperFactory).GetTypeInfo().Assembly;
                        using (var streamReader = new StreamReader(assembly.GetManifestResourceStream("OneBusAway.DataAccess.Regions.xml")))
                        {
                            string xml = await streamReader.ReadToEndAsync();
                            doc = XDocument.Parse(xml);
                        }
                    }

                    return (from regionElement in doc.Descendants("region")
                            let region = new Region(regionElement)
                            where region.IsActive && region.SupportsObaRealtimeApis && region.SupportsObaDiscoveryApis
                            select region).ToArray();
                });
        }

        /// <summary>
        /// Creates the service helper.
        /// </summary>
        public ObaServiceHelperFactory(double usersLatitude, double usersLongitude)
        {
            this.usersLatitude = usersLatitude;
            this.usersLongitude = usersLongitude;
        }

        /// <summary>
        /// Factory method creates a service helper.
        /// </summary>
        public virtual async Task<IObaServiceHelper> CreateHelperAsync(ObaMethod obaMethod, HttpMethod httpMethod = HttpMethod.GET)
        {
            // Find the closest region to the user's location:
            var usersRegion = await this.FindClosestRegionAsync();
            return new ObaServiceHelper(usersRegion, obaMethod, httpMethod);
        }

        /// <summary>
        /// Returns the closest region to the current user.
        /// </summary>
        /// <returns></returns>
        internal async Task<Region> FindClosestRegionAsync()
        {
            // Find the closest region to the user's location:
            return (from region in await regionsLookupTask
                    let distance = region.DistanceFrom(this.usersLatitude, this.usersLongitude)
                    orderby distance ascending
                    select region).First();
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
            /// The oba method.
            /// </summary>
            private ObaMethod obaMethod;

            /// <summary>
            /// The service Url.
            /// </summary>
            private string serviceUrl;

            /// <summary>
            /// This is the region where the request is being made to.
            /// </summary>
            private Region region;

            /// <summary>
            /// If there is an id for the request we store it here.
            /// </summary>
            private string id;

            /// <summary>
            /// Maps name / value pairs to the query string.
            /// </summary>
            private Dictionary<string, string> queryStringMap;

            /// <summary>
            /// Creates the service helper.
            /// </summary>
            public ObaServiceHelper(Region region, ObaMethod obaMethod, HttpMethod httpMethod)
            {
                this.obaMethod = obaMethod;
                this.httpMethod = httpMethod;
                this.region = region;
                this.serviceUrl = this.region.RegionUrl;
                this.id = null;

                this.uriBuilder = new UriBuilder(serviceUrl);
                this.SetDefaultPath();

                this.queryStringMap = new Dictionary<string, string>();
                this.queryStringMap["key"] = UtilitiesConstants.API_KEY;
                this.queryStringMap["Version"] = "2";
            }

            /// <summary>
            /// Returns the region name that the helper will talk to.
            /// </summary>
            public string RegionName
            {
                get
                {
                    return this.region.RegionName;
                }
            }

            /// <summary>
            /// Adds a name / value pair to the query string.
            /// </summary>
            public void AddToQueryString(string name, string value)
            {
                this.queryStringMap[name] = value;
            }

            /// <summary>
            /// Sets the id for the rest query, if it exists.
            /// </summary>
            public void SetId(string id)
            {
                this.id = id;
                this.uriBuilder = new UriBuilder(serviceUrl);
                this.SetPath(id);
            }

            /// <summary>
            /// Sets the default path.
            /// </summary>
            private void SetDefaultPath()
            {
                this.SetPath(null);
            }

            /// <summary>
            /// Sets the path of the uri.
            /// </summary>
            /// <param name="id">The ID to set</param>
            private void SetPath(string id)
            {
                // If the URI we get back is missing a backslash, add it first:
                if (!String.IsNullOrEmpty(this.uriBuilder.Path) && !this.uriBuilder.Path.EndsWith("/"))
                {
                    this.uriBuilder.Path += "/";
                }

                this.uriBuilder.Path += "api/where/";

                string obaMethodString = obaMethod.ToString();
                obaMethodString = obaMethodString.Replace('_', '-');

                if (!string.IsNullOrEmpty(id))
                {
                    obaMethodString += "/";
                    obaMethodString += id;
                }

                obaMethodString += ".xml";
                this.uriBuilder.Path += obaMethodString;
            }

            /// <summary>
            /// Sends a payload to the service asynchronously.
            /// </summary>
            public async Task<XDocument> SendAndRecieveAsync(int cacheTimeout)
            {
                XDocument doc = await this.GetCachedDocument(cacheTimeout);
                if (doc == null)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        try
                        {
                            this.uriBuilder.Query = this.CreateQueryString();
                            this.request = WebRequest.CreateHttp(this.uriBuilder.Uri);
                            this.request.Method = this.httpMethod.ToString();

                            doc = await WebRequestQueue.SendAsync(request);

                            // Verify that OBA sent us a valid document and that it's status code is 200:                
                            int returnCode = doc.Root.GetFirstElementValue<int>("code");
                            if (returnCode != 200)
                            {
                                string text = doc.Root.GetFirstElementValue<string>("text");
                                throw new ObaException(returnCode, text);
                            }

                            await this.TrySaveCachedDocument(doc, cacheTimeout);
                            return doc;
                        }
                        catch (Exception e)
                        {
                            // Make sure ObaExceptions bubble up because we expect them. 
                            // Note 401 means busy
                            ObaException obaException = e as ObaException;
                            if (obaException != null && obaException.ErrorCode != 401)
                            {
                                throw;
                            }
                        }

                        // If we keep getting 401s (permission denied), then we just need to keep retrying.
                        await Task.Delay(50);
                    }
                }

                // We tried and waited as long as we could....or we had a cached copy:
                return doc;
            }

            /// <summary>
            /// Get cached query.
            /// </summary>
            /// 
            /// 
            /// <param name="cacheTimeout">Expected cache age in seconds</param>
            /// <returns>Cached query if existed and new; null otherwise.</returns>
            private async Task<XDocument> GetCachedDocument(int cacheTimeout = UtilitiesConstants.DefaultCacheAge)
            {
                if (!string.IsNullOrEmpty(this.id) && cacheTimeout != UtilitiesConstants.NoCacheAge)
                {
                    try
                    {
                        var storageFile = await ApplicationData.Current.LocalFolder.GetFileAsync(this.GetCachedFilePath());
                        var properties = await storageFile.GetBasicPropertiesAsync();

                        // Make sure the file hasn't expired yet:
                        if ((DateTime.Now - properties.DateModified).TotalSeconds < cacheTimeout)
                        {
                            using (var stream = await storageFile.OpenStreamForReadAsync())
                            {
                                return XDocument.Load(stream);
                            }
                        }
                    }
                    catch
                    {
                        // Can't get the document? Fine. We'll just have to requery.
                    }
                }

                return null;
            }

            /// <summary>
            /// Saves a cached document to disk.
            /// </summary>
            /// <param name="doc">The document to save</param>
            /// <returns>An awaitable task</returns>
            private async Task<bool> TrySaveCachedDocument(XDocument doc, int expectedCacheAge)
            {
                if (!String.IsNullOrEmpty(this.id) && expectedCacheAge != UtilitiesConstants.NoCacheAge && doc != null)
                {
                    try
                    {
                        string path = this.GetCachedFilePath();
                        var file = await ApplicationData.Current.LocalFolder.CreateFileAsync(path, CreationCollisionOption.ReplaceExisting);

                        using (var stream = await file.OpenStreamForWriteAsync())
                        {
                            doc.Save(stream);
                            return true;
                        }
                    }
                    catch
                    {
                        // Eat the exception. Not the end of the world. Just nothing to save.
                    }
                }

                return false;
            }

            /// <summary>
            /// Returns the name of the cached file.
            /// </summary>
            /// <returns></returns>
            private string GetCachedFilePath()
            {
                string fileName = Path.Combine("ObaCache", this.region.RegionName);
                return Path.Combine(fileName, string.Format("{0}_{1}.xml", this.obaMethod.ToString(), this.id));
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