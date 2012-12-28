using OneBusAway.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// Creates the time table view model.
        /// </summary>
        public TimeTablePageViewModel()
        {
            this.HeaderViewModel.SubText = "TIMETABLE";
            this.TimeTableControlViewModel = new TimeTableControlViewModel();
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
        public async Task SetRouteAndStopData(TrackingData trackingData)
        {
            this.TimeTableControlViewModel.RouteDescription = trackingData.Route.Description;
            this.TimeTableControlViewModel.RouteNumber = trackingData.Route.ShortName;
            this.TimeTableControlViewModel.StopDescription = trackingData.StopName;
            await this.TimeTableControlViewModel.FindScheduleData(trackingData.StopId, trackingData.RouteId);
        }
    }
}
