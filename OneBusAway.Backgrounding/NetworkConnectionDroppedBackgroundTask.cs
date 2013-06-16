using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace OneBusAway.Backgrounding
{
    /// <summary>
    /// Fired when the internet connection drops. We need to abort the loop.
    /// </summary>
    public sealed class NetworkConnectionDroppedBackgroundTask : IBackgroundTask
    {
        /// <summary>
        /// Abort the thread.
        /// </summary>
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            TileUpdaterService.Instance.Abort();
        }
    }
}
