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
using OneBusAway.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OneBusAway.DataAccess.ObaService
{
    /// <summary>
    /// This interface defines an object that talks to a web service.
    /// </summary>
    public interface IObaServiceHelper
    {
        /// <summary>
        /// Adds a name / value pair to the query string.
        /// </summary>
        void AddToQueryString(string name, string value);

        /// <summary>
        /// For some rest API calls, the format will be:
        /// 
        /// obamethod/id.xml?querystring
        /// 
        /// Instead of just 
        /// 
        /// obamethod.xml?querystring.
        /// </summary>
        void SetId(string id);

        /// <summary>
        /// Returns the region name that the helper will talk to.
        /// </summary>
        string RegionName
        {
            get;
        }

        /// <summary>
        /// Sends a payload to the service asynchronously and reads the response.
        /// </summary>
        Task<XDocument> SendAndRecieveAsync(int cacheTimeout = Constants.DefaultCacheAge);
    }
}