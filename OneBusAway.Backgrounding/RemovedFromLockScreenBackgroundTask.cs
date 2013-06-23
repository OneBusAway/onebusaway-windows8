using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace OneBusAway.Backgrounding
{
    /// <summary>
    /// This task is fired when the user removes the app from the lock screen.
    /// We need to stop updating the tiles.
    /// </summary>
    public sealed class RemovedFromLockScreenBackgroundTask : IBackgroundTask
    {
        /// <summary>
        /// Runs the tile updater service.
        /// </summary>
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            BackgroundTaskRegistrar.UnregisterAllBackgroundTasks(true);
            TileUpdaterService.Instance.Abort();
        }
    }
}
