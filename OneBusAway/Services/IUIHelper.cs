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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneBusAway.Services
{
    /// <summary>
    /// Interface allows view models to interact with the UI in an abstacted way.
    /// </summary>
    public interface IUIHelper
    {
        /// <summary>
        /// Defines a method that will add items to an observable collection one at a time, waiting for 
        /// idle before each add.
        /// </summary>
        Task BatchAddItemsAsync<T>(ObservableCollection<T> collection, IEnumerable<T> newItems, bool clear = true);

        /// <summary>
        /// Allows view models to wait for idle asynchronously.
        /// </summary>
        Task WaitForIdleAsync();
    }
}
