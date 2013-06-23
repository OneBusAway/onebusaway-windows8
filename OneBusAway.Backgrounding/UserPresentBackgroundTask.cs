using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace OneBusAway.Backgrounding
{
    /// <summary>
    /// This task runs when the user becomes present (IE they unlock the device).
    /// </summary>
    public sealed class UserPresentBackgroundTask : IBackgroundTask
    {
        /// <summary>
        /// Runs the tile updater service.
        /// </summary>
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            taskInstance.Canceled += OnTaskInstanceCanceled;
            TileUpdaterService.Instance.CreateIfNeccessary(taskInstance.GetDeferral());
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
