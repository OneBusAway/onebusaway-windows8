using OneBusAway.DataAccess;
using OneBusAway.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneBusAway.ViewModels
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
        /// Data access to OneBusAway.
        /// </summary>
        private ObaDataAccess obaDataAccess;

        /// <summary>
        /// The currently selected stop.
        /// </summary>
        private TripStop selectedStop;

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
            this.obaDataAccess = new ObaDataAccess();
            this.IsLoadingTripDetails = true;
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
            this.TripDetails = await this.obaDataAccess.GetTripDetailsAsync(this.trackingData.TripId);
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
