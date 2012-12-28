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
        private string routeNumber;
        private string routeDescription;
        private string stopDescription;

        /// <summary>
        /// Creates the time table view model.
        /// </summary>
        public TimeTablePageViewModel()
        {
            this.HeaderViewModel.SubText = "TIMETABLE";
        }

        public string RouteNumber
        {
            get
            {
                return this.routeNumber;
            }
            set
            {
                SetProperty(ref this.routeNumber, value);
            }
        }

        public string RouteDescription
        {
            get
            {
                return this.routeDescription;
            }
            set
            {
                SetProperty(ref this.routeDescription, value);
            }
        }

        public string StopDescription
        {
            get
            {
                return this.stopDescription;
            }
            set
            {
                SetProperty(ref this.stopDescription, value);
            }
        }
    }
}
