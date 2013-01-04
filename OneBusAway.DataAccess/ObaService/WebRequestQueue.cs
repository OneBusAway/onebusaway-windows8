using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
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
        /// A queue of tasks that will execute the web requests asynchronously.
        /// </summary>
        private Queue<Func<Task>> taskQueue;

        /// <summary>
        /// The queue of web requests that we are throttling.
        /// </summary>
        private Queue<HttpWebRequest> requestQueue;

        /// <summary>
        /// A queue of task completion sources that callers can await on.
        /// </summary>
        private Queue<TaskCompletionSource<XDocument>> responseQueue;

        /// <summary>
        /// Singleton ctor.
        /// </summary>
        private WebRequestQueue()
        {
            this.requestQueue = new Queue<HttpWebRequest>();
            this.responseQueue = new Queue<TaskCompletionSource<XDocument>>();
            this.taskQueue = new Queue<Func<Task>>();
        }

        /// <summary>
        /// Queues a web request into our queue and returns an awaitable task.
        /// </summary>
        public static Task<XDocument> SendAsync(HttpWebRequest request)
        {
            lock (instance)
            {
                instance.requestQueue.Enqueue(request);

                var source = new TaskCompletionSource<XDocument>();
                instance.responseQueue.Enqueue(source);

                instance.taskQueue.Enqueue(ExecuteWebRequestAsync);

                if (instance.taskQueue.Count == 1)
                {
                    Task.Run(instance.taskQueue.Dequeue());
                }

                return source.Task;
            }
        }

        /// <summary>
        /// Executes a web request asynchronously and dequeues the next task if it exists.
        /// </summary>
        private static async Task ExecuteWebRequestAsync()
        {
            HttpWebRequest request = null;
            TaskCompletionSource<XDocument> completionSource = null;

            lock (instance)
            {
                request = instance.requestQueue.Dequeue();
                completionSource = instance.responseQueue.Dequeue();
            }

            var response = await request.GetResponseAsync();
            var responseStream = response.GetResponseStream();

            XDocument doc = null;
            using (var streamReader = new StreamReader(responseStream))
            {
                string xml = await streamReader.ReadToEndAsync();
                doc = XDocument.Parse(xml);
            }

            completionSource.SetResult(doc);

            // Wait 20ms before we kick off the next request:
            await Task.Delay(20);

            // If we have another task in the queue, let it loose now:
            lock (instance)
            {
                if (instance.taskQueue.Count > 0)
                {
                    var ignored = Task.Run(instance.taskQueue.Dequeue());
                }
            }
        }
    }
}
