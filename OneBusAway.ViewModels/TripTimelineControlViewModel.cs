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
        /// Creates the control.
        /// </summary>
        public TripTimelineControlViewModel()
        {
            this.obaDataAccess = new ObaDataAccess();
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
        }
    }
}
