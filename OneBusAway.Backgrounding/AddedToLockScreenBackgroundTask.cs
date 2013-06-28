using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace OneBusAway.Backgrounding
{
    /// <summary>
    /// This task is fired when the app is added to the lock screen.
    /// </summary>
    public sealed class AddedToLockScreenBackgroundTask : IBackgroundTask
    {
        /// <summary>
        /// Runs the tile updater service.
        /// </summary>
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            // Try and register the rest of our background tasks:
            BackgroundTaskRegistrar.TryRegisterAllBackgroundTasks();

            var deferral = taskInstance.GetDeferral();
            try
            {
                await TileUpdaterService.UpdateTilesAsync();
            }
            finally
            {
                deferral.Complete();
            }
        }
    }
}
