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

using OneBusAway.DataAccess;
using OneBusAway.DataAccess.ObaService;
using OneBusAway.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneBusAway.ViewModels.Controls
{
    /// <summary>
    /// View model for the trip timeline control.
    /// </summary>
    public class TripTimelineControlViewModel : ViewModelBase
    {
        /// <summary>
        /// The tracking data for the bus that we're displaying.
        /// </summary>
        private TrackingData trackingData;

        /// <summary>
        /// Trip details for this particular trip.
        /// </summary>
        private TripDetails tripDetails;
        
        /// <summary>
        /// The currently selected stop.
        /// </summary>
        private TripStop selectedStop;

        /// <summary>
        /// The selected stop id.
        /// </summary>
        private string selectedStopId;

        /// <summary>
        /// True when we are loading trip details.  Used to show the progress ring.
        /// </summary>
        private bool isLoadingTripDetails;

        /// <summary>
        /// This event is fired when the user selects a stop on the time table view model.
        /// </summary>
        public event EventHandler<StopSelectedEventArgs> StopSelected;

        /// <summary>
        /// Creates the control.
        /// </summary>
        public TripTimelineControlViewModel()
        {
            this.IsLoadingTripDetails = true;
        }

        public TripStop SelectedStop
        {
            get
            {
                return this.selectedStop;
            }
            set
            {
                SetProperty(ref this.selectedStop, value);
            }
        }

        public string SelectedStopId
        {
            get
            {
                return this.selectedStopId;
            }
            set
            {
                SetProperty(ref this.selectedStopId, value);
            }
        }

        /// <summary>
        /// Set to false once we are done loading trip details.
        /// </summary>
        public bool IsLoadingTripDetails
        {
            get
            {
                return this.isLoadingTripDetails;
            }
            set
            {
                SetProperty(ref this.isLoadingTripDetails, value);
            }
        }

        /// <summary>
        /// Gets / sets the tracking data.
        /// </summary>
        public TrackingData TrackingData
        {
            get
            {
                return this.trackingData;
            }
            set
            {
                SetProperty(ref this.trackingData, value);
            }
        }

        /// <summary>
        /// Gets / sets trip details for this trip.
        /// </summary>
        public TripDetails TripDetails
        {
            get
            {
                return this.tripDetails;
            }
            set
            {
                SetProperty(ref this.tripDetails, value);
            }
        }

        /// <summary>
        /// Gets the trip details for the tracking data.
        /// </summary>
        public async Task GetTripDetailsAsync()
        {
            var obaDataAccess = ObaDataAccess.Create();
            this.TripDetails = await obaDataAccess.GetTripDetailsAsync(this.trackingData.TripId);

            if (!string.IsNullOrEmpty(this.selectedStopId))
            {
                this.SelectStop(this.selectedStopId);
            }

            this.IsLoadingTripDetails = false;
        }

        /// <summary>
        /// Selects a stop.
        /// </summary>
        public void SelectStop(string stopId)
        {
            if (selectedStop != null)
            {
                selectedStop.IsSelectedStop = false;
            }
                        
            this.selectedStop = (from tripStop in this.tripDetails.TripStops
                                 where string.Equals(stopId, tripStop.StopId, StringComparison.OrdinalIgnoreCase)
                                 select tripStop).FirstOrDefault();

            this.SelectedStopId = stopId;
            if (this.selectedStop != null)
            {
                this.selectedStop.IsSelectedStop = true;
            }
        }

        /// <summary>
        /// Called when the user selects a new stop.
        /// </summary>
        public void SelectNewStop(TripStop stop)
        {
            // This stop is already selected:
            if (this.selectedStop != null && string.Equals(this.selectedStop.StopId, stop.StopId, StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            this.SelectStop(stop.StopId);
            var stopSelected = this.StopSelected;
            if (stopSelected != null)
            {
                stopSelected(this, new StopSelectedEventArgs(stop.Name, stop.StopId, stop.Direction, stop.Latitude, stop.Longitude));
            }
        }
    }
}