/* Copyright 2013 Michael Braude and individual contributors.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OneBusAway.Model
{
    /// <summary>
    /// Represents a stop on a route for a particular trip.
    /// </summary>
    public class TripStop : Stop
    {
        private DateTime arrivalTime;
        private bool hasReachedStop;        
        private bool isSelectedStop;

        /// <summary>
        /// Creates the trip stop.
        /// </summary>
        public TripStop(XElement tripStopElement, DateTime serverTime)
        {
            //<tripStopTime>
            //<arrivalTime>83942</arrivalTime>
            //<departureTime>83942</departureTime>
            //<stopId>1_35324</stopId>
            //<distanceAlongTrip>13566.594707177532</distanceAlongTrip>
            //</tripStopTime>

            // Note - this time is different from all the other times :)
            // from the documentation:
            // http://developer.onebusaway.org/modules/onebusaway-application-modules/current/api/where/elements/trip-details.html
            // "•arrivalTime - time, in seconds since the start of the service date, when the trip arrives at the specified stop"
            var timeSpan = new TimeSpan(0, 0, tripStopElement.GetFirstElementValue<int>("arrivalTime"));
            this.ArrivalTime = (new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)).AddSeconds(timeSpan.TotalSeconds);

            // Try and find the stop in the owning document for this trip:
            string stopId = tripStopElement.GetFirstElementValue<string>("stopId");
            XElement stopElement = (from currentStopElement in tripStopElement.Document.Descendants("stop")
                                    where string.Equals(stopId, currentStopElement.GetFirstElementValue<string>("id"), StringComparison.OrdinalIgnoreCase)
                                    select currentStopElement).FirstOrDefault();

            if (stopElement != null)
            {
                base.ParseStopElement(stopElement);
            }
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

        public bool IsSelectedStop
        {
            get
            {
                return this.isSelectedStop;
            }
            set
            {
                SetProperty(ref this.isSelectedStop, value);
            }
        }

        public bool HasReachedStop
        {
            get
            {
                return this.hasReachedStop;
            }
            set
            {
                SetProperty(ref this.hasReachedStop, value);
            }
        }        
    }
}