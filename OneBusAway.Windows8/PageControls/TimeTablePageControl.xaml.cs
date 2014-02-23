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
using System.Threading.Tasks;
using OneBusAway.Model;
using OneBusAway.ViewModels;
using OneBusAway.ViewModels.Controls;
using OneBusAway.ViewModels.PageControls;
using Windows.UI.Xaml.Controls;

namespace OneBusAway.PageControls
{
    /// <summary>
    /// This class contains schedule data for a particular route / stop combination.
    /// </summary>
    public sealed partial class TimeTablePageControl : UserControl, IPageControl
    {
        /// <summary>
        /// The view model for the time table page.
        /// </summary>
        private TimeTablePageControlViewModel viewModel;

        /// <summary>
        /// Creates the page control.
        /// </summary>
        public TimeTablePageControl()
        {
            this.InitializeComponent();
            this.viewModel = new TimeTablePageControlViewModel();
        }

        /// <summary>
        /// Return the view model.
        /// </summary>
        public PageViewModelBase ViewModel
        {
            get 
            {
                return this.viewModel;
            }
        }

        /// <summary>
        /// Initializes the time table page.
        /// </summary>
        public async Task InitializeAsync(object parameter)
        {
            this.viewModel.MapControlViewModel.BusStops = null;

            var routeMapViewModel = parameter as RouteMapsAndSchedulesControlViewModel;
            if (routeMapViewModel != null)
            {
                await this.viewModel.SetRouteAndStopData(
                    routeMapViewModel.StopName,
                    routeMapViewModel.StopId,
                    routeMapViewModel.RouteName,
                    routeMapViewModel.RouteId);

                await this.viewModel.GetRouteData(routeMapViewModel.StopId, routeMapViewModel.RouteId);
            }
            else
            {
                var trackingData = parameter as TrackingData;
                if (trackingData != null)
                {
                    await this.viewModel.SetRouteAndStopData(
                        trackingData.StopName,
                        trackingData.StopId,
                        trackingData.Route.ShortName,
                        trackingData.RouteId);

                    await this.viewModel.GetRouteData(trackingData.StopId, trackingData.RouteId);
                }
            }

            this.viewModel.MapControlViewModel.ZoomToRouteShape();
        }

        /// <summary>
        /// Restore asynchronously.
        /// </summary>
        public Task RestoreAsync()
        {
            this.viewModel.MapControlViewModel.MapView.AnimateChange = true;
            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Nothing to do here.
        /// </summary>
        public Task RefreshAsync()
        {
            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Pages should be represent themselves as a string of parameters.
        /// </summary>
        public PageInitializationParameters GetParameters()
        {
            throw new NotImplementedException();
        }
    }
}