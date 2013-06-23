using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace OneBusAway.Backgrounding
{
    /// <summary>
    /// This task is called every 15 minutes. Like a heart beat...if the task went down for 
    /// some reason this can start it back up.
    /// </summary>
    public sealed class TimerBackgroundTask : IBackgroundTask
    {
        /// <summary>
        /// Runs the tile updater service.
        /// </summary>
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            // Do nothing. We kick off the background process when the user logs in, internet becomes 
            // available, ect. The timer task is here to appease Windows 8. We don't do anything because 
            // timer tasks tend to be cancelled frequently, which can cause the process to stop running updates.
        }
    }
}
