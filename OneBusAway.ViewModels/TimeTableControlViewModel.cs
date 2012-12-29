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
    /// View model for the time table control.
    /// </summary>
    public class TimeTableControlViewModel : ViewModelBase
    {
        private string routeNumber;
        private string routeDescription;
        private string stopDescription;
        private DateTime[][] scheduleData;

        private ObaDataAccess obaDataAccess;

        /// <summary>
        /// Creates the view model.
        /// </summary>
        public TimeTableControlViewModel()
        {
            this.obaDataAccess = new ObaDataAccess();
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

        public DateTime[][] ScheduleData
        {
            get
            {
                return this.scheduleData;
            }
            set
            {
                SetProperty(ref this.scheduleData, value);
            }
        }

        public string TripHeadsign
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

        /// <summary>
        /// Queries Oba for schedule data based on the current stop id.
        /// </summary>
        public async Task FindScheduleData(string stopId, string routeId)
        {
            var scheduleData = await this.obaDataAccess.GetScheduleForStopAndRoute(stopId, routeId);

            var query = from scheduleStopTime in scheduleData.ScheduleStopTimes
                        orderby scheduleStopTime.ArrivalTime ascending
                        where scheduleStopTime.ArrivalTime.Day == DateTime.Now.Day
                        group scheduleStopTime by scheduleStopTime.ArrivalTime.Hour into groupedByHourData                        
                        select (from byHourStopTime in groupedByHourData
                                orderby byHourStopTime.ArrivalTime ascending
                                select byHourStopTime.ArrivalTime).ToArray();

            this.ScheduleData = (from arrivalsByHour in query
                                 select arrivalsByHour).ToArray();
        }
    }
}
