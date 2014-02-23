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
using OneBusAway.ViewModels.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneBusAway.ViewModels.PageControls
{
    /// <summary>
    /// View model for the time table page.
    /// </summary>
    public class TimeTablePageControlViewModel : PageViewModelBase
    {
        /// <summary>
        /// The view model for the time table page control.
        /// </summary>
        private TimeTableControlViewModel timeTableControlViewModel;
        
        /// <summary>
        /// This is the route id that we are displaying schedule data for.
        /// </summary>
        private string routeId;
        
        /// <summary>
        /// Creates the time table view model.
        /// </summary>
        public TimeTablePageControlViewModel()
        {
            this.HeaderViewModel.SubText = "TIMETABLE";
            this.TimeTableControlViewModel = new TimeTableControlViewModel();

            this.MapControlViewModel.RefreshBusStopsOnMapViewChanged = false;
            this.MapControlViewModel.StopSelected += OnStopSelectedAsync;                        
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
        public async Task SetRouteAndStopData(string stopName, string stopId, string routeName, string routeId)
        {
            this.routeId = routeId;
            this.TimeTableControlViewModel.RouteNumber = routeName;
            this.TimeTableControlViewModel.StopDescription = stopName;
            await this.TimeTableControlViewModel.FindScheduleDataAsync(stopId, routeId);
        }

        /// <summary>
        /// Ask OBA for shape & route data.
        /// </summary>
        public async Task GetRouteData(string stopId, string routeId)
        {
            var obaDataAccess = ObaDataAccess.Create();
            RouteData routeData = await obaDataAccess.GetRouteDataAsync(routeId, stopId);
            this.MapControlViewModel.BusStops = new BusStopList(routeData.Stops);
            this.MapControlViewModel.Shapes = routeData.Shapes.ToList();
            this.MapControlViewModel.SelectStop(stopId);
        }

        /// <summary>
        /// Called when user selects another bus stop on the map control.
        /// </summary>
        private async void OnStopSelectedAsync(object sender, StopSelectedEventArgs e)
        {
            this.TimeTableControlViewModel.StopDescription = e.StopName;
            await this.TimeTableControlViewModel.FindScheduleDataAsync(e.SelectedStopId, this.routeId);
            this.MapControlViewModel.SelectStop(e.SelectedStopId);            
        }        
    }
}