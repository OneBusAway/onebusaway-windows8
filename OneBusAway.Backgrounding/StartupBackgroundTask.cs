/* Copyright 2013 Microsoft
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
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace OneBusAway.Backgrounding
{
    /// <summary>
    /// This task should start when the user logs in.
    /// </summary>
    public sealed class StartupBackgroundTask : IBackgroundTask
    {
        /// <summary>
        /// Runs the tile updater service.
        /// </summary>
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
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
