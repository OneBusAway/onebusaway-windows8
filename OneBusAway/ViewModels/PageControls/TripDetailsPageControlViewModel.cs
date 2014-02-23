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

using OneBusAway.ViewModels.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneBusAway.ViewModels.PageControls
{
    /// <summary>
    /// View model for the trip details page.
    /// </summary>
    public class TripDetailsPageControlViewModel : PageViewModelBase
    {
        /// <summary>
        /// View model for the route timeline control.
        /// </summary>
        private TripTimelineControlViewModel routeTimelineControlViewModel;

        /// <summary>
        /// Creates the view model.
        /// </summary>
        public TripDetailsPageControlViewModel()
        {
            this.HeaderViewModel.SubText = "TRIP DETAILS";            
            this.TripTimelineControlViewModel = new TripTimelineControlViewModel();

            this.MapControlViewModel.RefreshBusStopsOnMapViewChanged = false;
            this.MapControlViewModel.StopSelected += OnStopSelected;
        }

        /// <summary>
        /// Returns the route timeline control view model.
        /// </summary>
        public TripTimelineControlViewModel TripTimelineControlViewModel
        {
            get
            {
                return this.routeTimelineControlViewModel;
            }
            set
            {
                SetProperty(ref this.routeTimelineControlViewModel, value);
            }
        }

        /// <summary>
        /// Called when the user selects a stop on the map.
        /// </summary>
        private void OnStopSelected(object sender, StopSelectedEventArgs e)
        {
        }
    }
}