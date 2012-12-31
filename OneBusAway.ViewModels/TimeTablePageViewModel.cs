using OneBusAway.DataAccess;
using OneBusAway.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace OneBusAway.ViewModels
{
    /// <summary>
    /// View model for the time table page.
    /// </summary>
    public class TimeTablePageViewModel : PageViewModelBase
    {
        /// <summary>
        /// The view model for the time table page control.
        /// </summary>
        private TimeTableControlViewModel timeTableControlViewModel;

        /// <summary>
        /// The view model for the map control.
        /// </summary>
        private MapControlViewModel mapControlViewModel;

        /// <summary>
        /// Get shape data for the route.
        /// </summary>
        private ObaDataAccess obaDataAccess;

        /// <summary>
        /// The tracking data that we are based off of.
        /// </summary>
        private TrackingData trackingData;

        /// <summary>
        /// Creates the time table view model.
        /// </summary>
        public TimeTablePageViewModel()
        {
            this.HeaderViewModel.SubText = "TIMETABLE";
            this.obaDataAccess = new ObaDataAccess();
            this.TimeTableControlViewModel = new TimeTableControlViewModel();

            this.MapControlViewModel = new MapControlViewModel();
            this.MapControlViewModel.RefreshBusStopsOnMapViewChanged = false;
            this.MapControlViewModel.StopSelected += OnStopSelectedAsync;
        }

        /// <summary>
        /// Gets / sets the time table control view model.
        /// </summary>
        public TimeTableControlViewModel TimeTableControlViewModel
        {
            get
            {
                return this.timeTableControlViewModel;
            }
            set
            {
                SetProperty(ref this.timeTableControlViewModel, value);
            }
        }

        /// <summary>
        /// Gets / sets the map control view model.
        /// </summary>
        public MapControlViewModel MapControlViewModel
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

        /// <summary>
        /// Setset parameters on the time table control.
        /// </summary>
        public async Task SetRouteAndStopData(TrackingData trackingData)
        {
            this.trackingData = trackingData;
            this.TimeTableControlViewModel.TripHeadsign = trackingData.TripHeadsign;
            this.TimeTableControlViewModel.RouteNumber = trackingData.Route.ShortName;
            this.TimeTableControlViewModel.StopDescription = trackingData.StopName;
            await this.TimeTableControlViewModel.FindScheduleDataAsync(trackingData.StopId, trackingData.RouteId);
        }

        /// <summary>
        /// Ask OBA for shape & route data.
        /// </summary>
        public async Task GetRouteData(TrackingData trackingData)
        {
            RouteData routeData = await this.obaDataAccess.GetRouteDataAsync(trackingData.RouteId, trackingData.TripHeadsign);
            this.mapControlViewModel.BusStops = routeData.Stops.ToList();
            this.mapControlViewModel.Shapes = routeData.Shapes.ToList();
            this.mapControlViewModel.SelectStop(trackingData.StopId);            
        }

        /// <summary>
        /// Called when user selects another bus stop on the map control.
        /// </summary>
        private async void OnStopSelectedAsync(object sender, StopSelectedEventArgs e)
        {
            this.TimeTableControlViewModel.StopDescription = e.StopName;
            await this.TimeTableControlViewModel.FindScheduleDataAsync(e.SelectedStopId, this.trackingData.RouteId);
            this.mapControlViewModel.SelectStop(e.SelectedStopId);            
        }
    }
}
