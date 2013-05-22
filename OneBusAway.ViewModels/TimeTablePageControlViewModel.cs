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
    public class TimeTablePageControlViewModel : PageViewModelBase
    {
        /// <summary>
        /// The view model for the time table page control.
        /// </summary>
        private TimeTableControlViewModel timeTableControlViewModel;
        
        /// <summary>
        /// This is the route id that we are displaying schedule data for.
        /// </summary>
        private string routeId;
        
        /// <summary>
        /// Creates the time table view model.
        /// </summary>
        public TimeTablePageControlViewModel()
        {
            this.HeaderViewModel.SubText = "TIMETABLE";
            this.TimeTableControlViewModel = new TimeTableControlViewModel();

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
        /// Setset parameters on the time table control.
        /// </summary>
        public async Task SetRouteAndStopData(string stopName, string stopId, string routeName, string routeId)
        {
            this.routeId = routeId;
            this.TimeTableControlViewModel.RouteNumber = routeName;
            this.TimeTableControlViewModel.StopDescription = stopName;
            await this.TimeTableControlViewModel.FindScheduleDataAsync(stopId, routeId);
        }

        /// <summary>
        /// Ask OBA for shape & route data.
        /// </summary>
        public async Task GetRouteData(string stopId, string routeId)
        {
            var obaDataAccess = ObaDataAccess.Create();
            RouteData routeData = await obaDataAccess.GetRouteDataAsync(routeId, stopId);
            this.MapControlViewModel.BusStops = new BusStopList(routeData.Stops);
            this.MapControlViewModel.Shapes = routeData.Shapes.ToList();
            this.MapControlViewModel.SelectStop(stopId);
        }

        /// <summary>
        /// Called when user selects another bus stop on the map control.
        /// </summary>
        private async void OnStopSelectedAsync(object sender, StopSelectedEventArgs e)
        {
            this.TimeTableControlViewModel.StopDescription = e.StopName;
            await this.TimeTableControlViewModel.FindScheduleDataAsync(e.SelectedStopId, this.routeId);
            this.MapControlViewModel.SelectStop(e.SelectedStopId);            
        }        
    }
}
