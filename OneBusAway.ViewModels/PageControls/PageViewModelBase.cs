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
using OneBusAway.ViewModels.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneBusAway.ViewModels.PageControls
{
    public class PageViewModelBase : ViewModelBase
    {
        /// <summary>
        /// Header control for all pages
        /// </summary>
        private HeaderControlViewModel headerControlVM;

        /// <summary>
        /// Map control view model for all pages.
        /// </summary>
        private MapControlViewModel mapControlViewModel;

        /// <summary>
        /// Creates the base class.
        /// </summary>
        public PageViewModelBase()
        {
            this.HeaderViewModel = new HeaderControlViewModel();
            this.MapControlViewModel = new MapControlViewModel();
        }

        public HeaderControlViewModel HeaderViewModel
        {
            get
            {
                return this.headerControlVM;
            }
            set
            {
                SetProperty(ref this.headerControlVM, value);
            }
        }

        /// <summary>
        /// Gets / sets the map control view model.
        /// </summary>
        public virtual MapControlViewModel MapControlViewModel
        {
            get
            {
                return this.mapControlViewModel;
            }
            set
            {
                SetProperty(ref this.mapControlViewModel, value);
            }
        }
    }
}