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

using OneBusAway.DataAccess;
using OneBusAway.DataAccess.ObaService;
using OneBusAway.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneBusAway.ViewModels.Controls
{
    /// <summary>
    /// View model for the time table control.
    /// </summary>
    public class TimeTableControlViewModel : ViewModelBase
    {
        private string routeNumber;
        private string tripHeadsign;
        private string stopDescription;
        private bool? scheduleAvailable;
        private bool isLoadingSchedule;
        private DateTime[][] scheduleData;
        private int dayOfTheWeek;
        private string stopId;
        private string routeId;

        private bool hasSecondTripHeadsign;
        private string secondTripHeadsign;
        private DateTime[][] secondScheduleData;

        private DayOfTheWeekControlViewModel dayOfTheWeekControlViewModel;

        /// <summary>
        /// Creates the view model.
        /// </summary>
        public TimeTableControlViewModel()
        {
            this.scheduleAvailable = null;
            this.IsLoadingSchedule = true;

            this.dayOfTheWeek = (int)DateTime.Now.DayOfWeek;

            this.DayOfTheWeekControlViewModel = new DayOfTheWeekControlViewModel();
            this.DayOfTheWeekControlViewModel.DayOfWeekChanged += OnDayOfTheWeekControlViewModelDayChanged;
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
                return this.tripHeadsign;
            }
            set
            {
                SetProperty(ref this.tripHeadsign, value);
            }
        }

        public DateTime[][] SecondScheduleData
        {
            get
            {
                return this.secondScheduleData;
            }
            set
            {
                SetProperty(ref this.secondScheduleData, value);
            }
        }

        public string SecondTripHeadsign
        {
            get
            {
                return this.secondTripHeadsign;
            }
            set
            {
                SetProperty(ref this.secondTripHeadsign, value);
            }
        }

        public bool HasSecondTripHeadsign
        {
            get
            {
                return this.hasSecondTripHeadsign;
            }
            set
            {
                SetProperty(ref this.hasSecondTripHeadsign, value);
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
            this.stopId = stopId;
            this.routeId = routeId;

            // First we need to find the day of the week to use:
            DateTime date = DateTime.Now;            

            if (this.dayOfTheWeek != (int)date.DayOfWeek)
            {
                int daysFromNow = this.dayOfTheWeek - (int)date.DayOfWeek;

                // If we're talking about a day in the past, roll it forward to next week:
                if (daysFromNow < 0)
                {
                    daysFromNow += 7;
                }

                date = date.AddDays(daysFromNow);
            }

            try
            {
                var obaDataAccess = ObaDataAccess.Create();
                var scheduleData = await obaDataAccess.GetScheduleForStopAndRouteAsync(stopId, routeId, date);

                if (scheduleData != null && scheduleData.Length > 0)
                {
                    this.ScheduleAvailable = true;
                    this.ScheduleData = this.GetDateTimesFromScheduleStopTimes(scheduleData[0].ScheduleStopTimes);
                    this.TripHeadsign = scheduleData[0].TripHeadsign;

                    if (scheduleData.Length == 2)
                    {
                        this.HasSecondTripHeadsign = true;
                        this.SecondScheduleData = this.GetDateTimesFromScheduleStopTimes(scheduleData[1].ScheduleStopTimes);
                        this.SecondTripHeadsign = scheduleData[1].TripHeadsign;
                    }
                    else
                    {
                        this.HasSecondTripHeadsign = false;
                    }
                }
                else
                {
                    this.ScheduleAvailable = false;
                    this.ScheduleData = null;
                    this.HasSecondTripHeadsign = false;
                }
            }
            catch (ArgumentException)
            {
                // No schedule available for this stop:
                this.ScheduleAvailable = false;
                this.ScheduleData = null;
                this.HasSecondTripHeadsign = false;
            }
            finally
            {
                this.IsLoadingSchedule = false;
            }
        }

        /// <summary>
        /// Sorts the schedule stop times and turns it into an array of date times.
        /// </summary>
        private DateTime[][] GetDateTimesFromScheduleStopTimes(ScheduleStopTime[] stopTimes)
        {
            var query = from scheduleStopTime in stopTimes
                        orderby scheduleStopTime.ArrivalTime ascending
                        group scheduleStopTime by scheduleStopTime.ArrivalTime.Hour into groupedByHourData
                        select (from byHourStopTime in groupedByHourData
                                orderby byHourStopTime.ArrivalTime ascending
                                select byHourStopTime.ArrivalTime).ToArray();

            return (from arrivalsByHour in query 
                    select arrivalsByHour).ToArray();
        }

        /// <summary>
        /// Called when the user selects a new day of the week.
        /// </summary>
        private async void OnDayOfTheWeekControlViewModelDayChanged(object sender, DayChangedEventArgs e)
        {
            this.dayOfTheWeek = e.DayOfWeek;
            await this.FindScheduleDataAsync(this.stopId, this.routeId);
        }
    }
}