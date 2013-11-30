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
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using OneBusAway.Utilities;
using System.Net.Http;

namespace OneBusAway.DataAccess.ObaService
{
    /// <summary>
    /// This class allows us to automatically throttle requests from the client. OBA
    /// will return 401 access denied errors if we ping it too frequently (our limit is 10ms).
    /// This queue class allows clients to queue up requests and await the results, while we 
    /// throttle the requests to make sure we never send two within 20ms (just to be safe we'll
    /// add a buffer =).
    /// </summary>
    public class WebRequestQueue
    {
        /// <summary>
        /// Create the singleton instance.
        /// </summary>
        private static WebRequestQueue instance = new WebRequestQueue();

        /// <summary>
        /// This is the current task that we use as a queueing mechanism to make sure the
        /// web requests are executed serially.
        /// </summary>
        private Task<XDocument> currentTask;

        /// <summary>
        /// Singleton ctor.
        /// </summary>
        private WebRequestQueue()
        {
            this.currentTask = Task.FromResult<XDocument>(null);
        }        

        /// <summary>
        /// Queues a web request into our queue and returns an awaitable task.
        /// </summary>
        public static Task<XDocument> SendAsync(HttpClient client, string uri)
        {
            lock (instance)
            {
                instance.currentTask = instance.currentTask.ContinueWith(async previousTask =>
                    {
                        using(CancellationTokenSource source = new CancellationTokenSource(DataAccessConstants.TIMEOUT_LENGTH))
                        {
                            try
                            {
                                var response = await client.GetAsync(uri, source.Token);
                                XDocument doc = XDocument.Parse(await response.Content.ReadAsStringAsync());

                                // Wait a bit to throttle the requests:
                                await Task.Delay(50);
                                return doc;
                            }
                            catch (TaskCanceledException)
                            {
                                throw new ObaException(401, "An internal error prevented the request from completing, or the server could not be found");
                            }
                        }
                    }).Unwrap();

                return instance.currentTask;
            }
        }
    }
}