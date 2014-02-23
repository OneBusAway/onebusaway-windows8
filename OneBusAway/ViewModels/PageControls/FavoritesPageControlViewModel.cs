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

using OneBusAway.Model;
using OneBusAway.ViewModels.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneBusAway.ViewModels.PageControls
{
    /// <summary>
    /// View model for the favorites page.
    /// </summary>
    public class FavoritesPageControlViewModel : PageViewModelBase, ITrackingDataViewModel
    {
        /// <summary>
        /// View model for the routes and stops control.
        /// </summary>
        private RoutesAndStopsControlViewModel routesAndStopsViewModel;

        /// <summary>
        /// This event is fired when a bus stop is selected.
        /// </summary>
        public event EventHandler<StopSelectedEventArgs> StopSelected;

        /// <summary>
        /// Creates the favorites view model.
        /// </summary>
        public FavoritesPageControlViewModel()
        {
            this.RoutesAndStopsViewModel = new RoutesAndStopsControlViewModel();
            this.MapControlViewModel.StopSelected += OnMapControlViewModelStopSelected;
        }

        #region Public Properties

        /// <summary>
        /// The view model for the routes and stops page.
        /// </summary>
        public RoutesAndStopsControlViewModel RoutesAndStopsViewModel
        {
            get
            {
                return this.routesAndStopsViewModel;                
            }
            set
            {
                SetProperty(ref this.routesAndStopsViewModel, value);
            }
        }

        /// <summary>
        /// Gets / sets the real time data.
        /// </summary>
        public TrackingData[] RealTimeData
        {
            get
            {
                return this.routesAndStopsViewModel.RealTimeData;
            }
            set
            {
                this.routesAndStopsViewModel.RealTimeData = value;
            }
        }

        /// <summary>
        /// Tell any views listening that a stop was selected.
        /// </summary>
        protected void OnMapControlViewModelStopSelected(object sender, StopSelectedEventArgs e)
        {
            var stopSelected = this.StopSelected;
            if (stopSelected != null)
            {
                stopSelected(this, e);
            }
        }

        #endregion
    }
}