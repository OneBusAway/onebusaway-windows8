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
using OneBusAway.ViewModels;
using OneBusAway.ViewModels.PageControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneBusAway.PageControls
{
    /// <summary>
    /// Defines a page control object.  These objects are created when a user navigates to a new 'page',
    /// which is really just a refreshed control inside the scroll viewer on the main page with a new data binding.
    /// </summary>
    public interface IPageControl
    {
        /// <summary>
        /// Page controls should be able to return their view model.
        /// </summary>
        PageViewModelBase ViewModel
        {
            get;
        }

        /// <summary>
        /// Pages should be able to initialize.
        /// </summary>
        Task InitializeAsync(object parameter);

        /// <summary>
        /// Pages should be represent themselves as a string of parameters.
        /// </summary>
        PageInitializationParameters GetParameters();
        
        /// <summary>
        /// Pages should be able to restore themselves.
        /// </summary>
        Task RestoreAsync();

        /// <summary>
        /// Pages should be able to refresh their data on demand.
        /// </summary>
        Task RefreshAsync();
    }
}