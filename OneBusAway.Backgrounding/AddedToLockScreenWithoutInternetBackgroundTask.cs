using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace OneBusAway.Backgrounding
{
    /// <summary>
    /// Registers all background tasks, which will be activated at a later time when the user is back on the internet.
    /// </summary>
    public sealed class AddedToLockScreenWithoutInternetBackgroundTask : IBackgroundTask
    {
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            BackgroundTaskRegistrar.TryRegisterAllBackgroundTasks();
        }
    }
}
