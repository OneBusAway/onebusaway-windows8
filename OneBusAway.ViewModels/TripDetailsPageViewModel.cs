using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneBusAway.ViewModels
{
    /// <summary>
    /// View model for the trip details page.
    /// </summary>
    public class TripDetailsPageViewModel : PageViewModelBase
    {
        /// <summary>
        /// View model for the route timeline control.
        /// </summary>
        private TripTimelineControlViewModel routeTimelineControlViewModel;

        /// <summary>
        /// Creates the view model.
        /// </summary>
        public TripDetailsPageViewModel()
        {
            this.HeaderViewModel.SubText = "TRIP DETAILS";            
            this.TripTimelineControlViewModel = new TripTimelineControlViewModel();

            this.MapControlViewModel = new MapControlViewModel();
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
