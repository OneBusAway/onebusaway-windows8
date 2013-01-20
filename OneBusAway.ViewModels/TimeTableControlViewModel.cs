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
        private bool? scheduleAvailable;
        private bool isLoadingSchedule;
        private DateTime[][] scheduleData;

        private ObaDataAccess obaDataAccess;

        /// <summary>
        /// Creates the view model.
        /// </summary>
        public TimeTableControlViewModel()
        {
            this.scheduleAvailable = null;
            this.IsLoadingSchedule = true;
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

        public bool? ScheduleAvailable
        {
            get
            {
                return this.scheduleAvailable;
            }
            set
            {
                SetProperty(ref this.scheduleAvailable, value);
            }
        }

        public bool IsLoadingSchedule
        {
            get
            {
                return this.isLoadingSchedule;
            }
            set
            {
                SetProperty(ref this.isLoadingSchedule, value);
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
        public async Task FindScheduleDataAsync(string stopId, string routeId)
        {
            try
            {
                var scheduleData = await this.obaDataAccess.GetScheduleForStopAndRoute(stopId, routeId);

                // TO DO: support more than one trip headsign. 
                // Not sure what the UI for this should be.

                var query = from scheduleStopTime in scheduleData[0].ScheduleStopTimes
                            orderby scheduleStopTime.ArrivalTime ascending
                            where scheduleStopTime.ArrivalTime.Day == DateTime.Now.Day
                            group scheduleStopTime by scheduleStopTime.ArrivalTime.Hour into groupedByHourData
                            select (from byHourStopTime in groupedByHourData
                                    orderby byHourStopTime.ArrivalTime ascending
                                    select byHourStopTime.ArrivalTime).ToArray();

                this.ScheduleAvailable = true;
                this.ScheduleData = (from arrivalsByHour in query
                                     select arrivalsByHour).ToArray();

                this.TripHeadsign = scheduleData[0].TripHeadsign;                
            }
            catch (ArgumentException)
            {
                // No schedule available for this stop:   
                this.ScheduleAvailable = false;                
                this.ScheduleData = null;
            }
            finally
            {
                this.IsLoadingSchedule = false;
            }
        }
    }
}
