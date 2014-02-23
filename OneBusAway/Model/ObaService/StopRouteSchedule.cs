/* Copyright 2014 Michael Braude and individual contributors.
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
    /// Data structure that contains schedule data for a particular stop / route combination.
    /// </summary>
    public class StopRouteSchedule : BindableBase
    {
        /// <summary>
        /// The schedule data.
        /// </summary>
        private ScheduleStopTime[] scheduleStopTimes;

        /// <summary>
        /// The trip headsign for this route.
        /// </summary>
        private string tripHeadsign;

        /// <summary>
        /// Creates a schedule out of a scheduleStopTimesElement.
        /// </summary>
        public StopRouteSchedule(DateTime serverTime, XElement stopRouteDirectionScheduleElement)
        {
            this.TripHeadsign = stopRouteDirectionScheduleElement.GetFirstElementValue<string>("tripHeadsign");
            this.ScheduleStopTimes = (from scheduleStopTimeElement in stopRouteDirectionScheduleElement.Descendants("scheduleStopTime")
                                      select new ScheduleStopTime(serverTime, scheduleStopTimeElement)).ToArray();
        }

        /// <summary>
        /// Gets / sets the trip headsign
        /// </summary>
        public string TripHeadsign
        {
            get
            {
                return this.tripHeadsign;
            }
            set
            {
                SetProperty(ref this.tripHeadsign, value);
            }
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
