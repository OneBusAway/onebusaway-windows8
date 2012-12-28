using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OneBusAway.Model
{
    /// <summary>
    /// A scheduled stop time.
    /// </summary>
    public class ScheduleStopTime : BindableBase
    {
        private DateTime arrivalTime;
        private string tripId;

        public ScheduleStopTime(DateTime serverTime, XElement scheduleStopTimeElement)
        {
            //<scheduleStopTime>
            //<arrivalEnabled>true</arrivalEnabled>
            //<arrivalTime>1356615606000</arrivalTime>
            //<departureEnabled>true</departureEnabled>
            //<departureTime>1356615606000</departureTime>
            //<serviceId>1_WEEKDAY</serviceId>
            //<tripId>1_21912615</tripId></scheduleStopTime>
            //</scheduleStopTime>
            this.ArrivalTime = scheduleStopTimeElement.GetFirstElementValue<long>("arrivalTime").ToDateTime();
            this.TripId = scheduleStopTimeElement.GetFirstElementValue<string>("tripId");
        }

        public DateTime ArrivalTime
        {
            get
            {
                return this.arrivalTime;
            }
            set
            {
                SetProperty(ref this.arrivalTime, value);
            }
        }

        public string TripId
        {
            get
            {
                return this.tripId;
            }
            set
            {
                SetProperty(ref this.tripId, value);
            }
        }
    }
}
