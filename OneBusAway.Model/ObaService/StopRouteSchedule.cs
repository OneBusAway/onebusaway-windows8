using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OneBusAway.Model
{
    /// <summary>
    /// Data structure that contains schedule data for a particular stop / route combination.
    /// </summary>
    public class StopRouteSchedule : BindableBase
    {
        /// <summary>
        /// The schedule data.
        /// </summary>
        private ScheduleStopTime[] scheduleStopTimes;

        /// <summary>
        /// Creates a schedule out of a scheduleStopTimesElement.
        /// </summary>
        public StopRouteSchedule(DateTime serverTime, XElement scheduleStopTimesElement)
        {
            this.ScheduleStopTimes = (from scheduleStopTimeElement in scheduleStopTimesElement.Descendants("scheduleStopTime")
                                      select new ScheduleStopTime(serverTime, scheduleStopTimeElement)).ToArray();
        }

        /// <summary>
        /// Gets / sets the schedule data.
        /// </summary>
        public ScheduleStopTime[] ScheduleStopTimes
        {
            get
            {
                return this.scheduleStopTimes;
            }
            set
            {
                SetProperty(ref this.scheduleStopTimes, value);
            }
        }
    }
}
