/* Copyright 2014 Michael Braude and individual contributors.
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
using System.Linq;
using System.Text;
using System.Threading;
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

            CancellationTokenSource tokenSource = new CancellationTokenSource();
            taskInstance.Canceled += (sender, reason) => tokenSource.Cancel();

            try
            {
                await TileUpdaterService.UpdateTilesAsync(tokenSource.Token);
            }
            finally
            {
                deferral.Complete();
                tokenSource.Dispose();
            }
        }
    }
}
