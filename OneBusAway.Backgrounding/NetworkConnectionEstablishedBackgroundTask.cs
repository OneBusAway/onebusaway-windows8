using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace OneBusAway.Backgrounding
{
    /// <summary>
    /// This task is fired when the users internet connection becomes available.
    /// </summary>
    public sealed class NetworkConnectionEstablishedBackgroundTask : IBackgroundTask
    {
        /// <summary>
        /// Runs the tile updater service.
        /// </summary>
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            taskInstance.Canceled += OnTaskInstanceCanceled;
            await TileUpdaterService.Instance.CreateIfNeccessary(taskInstance.GetDeferral());
        }

        /// <summary>
        /// Cancels the task.
        /// </summary>
        private void OnTaskInstanceCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            TileUpdaterService.Instance.Abort();
        }
    }
}
