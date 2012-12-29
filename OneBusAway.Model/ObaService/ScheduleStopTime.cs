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

            // IMPORTANT: the schedule time that we get from OBA is
            // relative to the time on the server. So for instance, the arrival
            // time may be 5:00pm, but that is relative to the server
            // time which may be 3:00pm. What that means is that the 
            // bus' arrival time is actually 2 hours from now, so to 
            // make the time relative to the client we take the difference
            // in minutes between the arrival time and the server time
            // and then add it to the current time.
            var arrivalTime = scheduleStopTimeElement.GetFirstElementValue<long>("arrivalTime").ToDateTime();
            var arrivalTimeInMinutes = (arrivalTime - serverTime).TotalMinutes;

            this.ArrivalTime = DateTime.Now.AddMinutes(arrivalTimeInMinutes);
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
