/* Copyright 2013 Michael Braude and individual contributors.
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
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.UI.Core;
using OneBusAway.Services;

namespace OneBusAway.Platforms.Windows8
{
    /// <summary>
    /// Passed into some view models to help them interact with the UI.
    /// </summary>
    public class DefaultUIHelper : IUIHelper
    {
        private CoreDispatcher dispatcher;

        /// <summary>
        /// Crteates the default UI helper.
        /// </summary>
        public DefaultUIHelper(CoreDispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
        }

        /// <summary>
        /// Batch-adds new items to the search result list in a way that prevents LayoutCycleExceptions. If 
        /// we add too many at once, WinRT thinks we've entered an infinite loop.
        /// </summary>
        public async Task BatchAddItemsAsync<T>(ObservableCollection<T> collection, IEnumerable<T> newItems, bool clear = true)
        {
            if (clear)
            {
                collection.Clear();
            }

            foreach (var item in newItems)
            {
                collection.Add(item);
                await WaitForIdleAsync();
                await Task.Delay(20);
            }
        }

        /// <summary>
        /// Allows view models to wait for the UI to idle.
        /// </summary>
        public async Task WaitForIdleAsync()
        {
            await this.dispatcher.RunIdleAsync(args => { });
        }
    }
}
