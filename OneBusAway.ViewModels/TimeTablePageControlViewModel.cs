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
        /// View model for hte day of the week control.
        /// </summary>
        private DayOfTheWeekControlViewModel dayOfTheWeekControlViewModel;
        
        /// <summary>
        /// Get shape data for the route.
        /// </summary>
        private ObaDataAccess obaDataAccess;

        /// <summary>
        /// This is the route id that we are displaying schedule data for.
        /// </summary>
        private string routeId;

        /// <summary>
        /// The stop id.
        /// </summary>
        private string stopId;

        /// <summary>
        /// The day of the week.
        /// </summary>
        private int dayOfTheWeek;

        /// <summary>
        /// Creates the time table view model.
        /// </summary>
        public TimeTablePageControlViewModel()
        {
            this.HeaderViewModel.SubText = "TIMETABLE";
            this.obaDataAccess = new ObaDataAccess();
            this.TimeTableControlViewModel = new TimeTableControlViewModel();

            this.MapControlViewModel.RefreshBusStopsOnMapViewChanged = false;
            this.MapControlViewModel.StopSelected += OnStopSelectedAsync;

            this.DayOfTheWeekControlViewModel = new DayOfTheWeekControlViewModel();
            this.DayOfTheWeekControlViewModel.DayOfWeekChanged += OnDayOfTheWeekControlViewModelDayChanged;

            this.dayOfTheWeek = (int)DateTime.Now.DayOfWeek;
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
        /// Returns the day of the week control view model.
        /// </summary>
        public DayOfTheWeekControlViewModel DayOfTheWeekControlViewModel
        {
            get
            {
                return this.dayOfTheWeekControlViewModel;
            }
            set
            {
                SetProperty(ref this.dayOfTheWeekControlViewModel, value);
            }
        }

        /// <summary>
        /// Setset parameters on the time table control.
        /// </summary>
        public async Task SetRouteAndStopData(string stopName, string stopId, string routeName, string routeId)
        {
            this.routeId = routeId;
            this.stopId = stopId;
            this.TimeTableControlViewModel.RouteNumber = routeName;
            this.TimeTableControlViewModel.StopDescription = stopName;
            await this.TimeTableControlViewModel.FindScheduleDataAsync(stopId, routeId, this.dayOfTheWeek);
        }

        /// <summary>
        /// Ask OBA for shape & route data.
        /// </summary>
        public async Task GetRouteData(string stopId, string routeId)
        {
            RouteData routeData = await this.obaDataAccess.GetRouteDataAsync(routeId, stopId);
            this.MapControlViewModel.BusStops = routeData.Stops.ToList();
            this.MapControlViewModel.Shapes = routeData.Shapes.ToList();
            this.MapControlViewModel.SelectStop(stopId);
            await Task.Delay(10);
        }

        /// <summary>
        /// Called when user selects another bus stop on the map control.
        /// </summary>
        private async void OnStopSelectedAsync(object sender, StopSelectedEventArgs e)
        {
            this.stopId = e.SelectedStopId;
            await this.TimeTableControlViewModel.FindScheduleDataAsync(this.stopId, this.routeId, this.dayOfTheWeek);
            this.MapControlViewModel.SelectStop(e.SelectedStopId);            
        }

        /// <summary>
        /// Called when the user selects a new day of the week.
        /// </summary>
        private async void OnDayOfTheWeekControlViewModelDayChanged(object sender, DayChangedEventArgs e)
        {
            this.dayOfTheWeek = e.DayOfWeek;
            await this.TimeTableControlViewModel.FindScheduleDataAsync(this.stopId, this.routeId, this.dayOfTheWeek);
        }
    }
}
