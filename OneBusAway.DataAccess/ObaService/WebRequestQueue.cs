using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OneBusAway.DataAccess
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
        public static Task<XDocument> SendAsync(HttpWebRequest request)
        {
            lock (instance)
            {
                instance.currentTask = instance.currentTask.ContinueWith(async previousTask =>
                    {
                        var response = await request.GetResponseAsync();
                        var responseStream = response.GetResponseStream();

                        XDocument doc = null;
                        using (var streamReader = new StreamReader(responseStream))
                        {
                            string xml = await streamReader.ReadToEndAsync();
                            doc = XDocument.Parse(xml);
                        }

                        // Wait a bit to throttle the requests:
                        await Task.Delay(50);
                        return doc;

                    }).Unwrap();

                return instance.currentTask;
            }
        }
    }
}
