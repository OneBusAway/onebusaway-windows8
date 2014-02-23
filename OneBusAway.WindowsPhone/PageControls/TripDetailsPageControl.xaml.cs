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
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using OneBusAway.DataAccess.ObaService;
using OneBusAway.Model;
using OneBusAway.ViewModels;
using OneBusAway.ViewModels.PageControls;

namespace OneBusAway.PageControls
{
    /// <summary>
    /// Page control for the trip details page.
    /// </summary>
    public sealed partial class TripDetailsPageControl : UserControl, IPageControl
    {
        /// <summary>
        /// The view model for the trip details.
        /// </summary>
        private TripDetailsPageControlViewModel viewModel;

        /// <summary>
        /// Creates the control.
        /// </summary>
        public TripDetailsPageControl()
        {
            this.InitializeComponent();

            this.viewModel = new TripDetailsPageControlViewModel();
            this.viewModel.MapControlViewModel.StopSelected += OnMapControlViewModelStopSelected;
            this.viewModel.TripTimelineControlViewModel.StopSelected += OnTripTimelineControlViewModelStopSelected;
        }

        /// <summary>
        /// Returns the view model.
        /// </summary>
        public PageViewModelBase ViewModel
        {
            get
            {
                return this.viewModel;
            }
        }

        /// <summary>
        /// Initializes the page.
        /// </summary>
        public async Task InitializeAsync(object parameter)
        {
            var tripViewModel = this.viewModel.TripTimelineControlViewModel;
            var mapViewModel = this.viewModel.MapControlViewModel;

            TrackingData trackingData = (TrackingData)parameter;
            tripViewModel.TrackingData = trackingData;

            // get the trip details:
            await tripViewModel.GetTripDetailsAsync();
            tripViewModel.SelectStop(trackingData.StopId);

            // Copy bus data into the map control:
            mapViewModel.BusStops = null;
            mapViewModel.BusStops = new BusStopList(tripViewModel.TripDetails.TripStops.Cast<Stop>());
            mapViewModel.SelectStop(trackingData.StopId, true);
            await mapViewModel.FindRouteShapeAsync(trackingData.RouteId, trackingData.StopId);
        }

        /// <summary>
        /// Restores the page.
        /// </summary>
        public async Task RestoreAsync()
        {
            this.viewModel.MapControlViewModel.MapView.AnimateChange = true;
            await this.RefreshAsync();
        }

        /// <summary>
        /// Refresh the trip details.
        /// </summary>
        public async Task RefreshAsync()
        {
            try
            {
                await this.viewModel.TripTimelineControlViewModel.GetTripDetailsAsync();

                // Find the closest stop in the trip timeline:
                var closestStop = (from tripStop in this.viewModel.TripTimelineControlViewModel.TripDetails.TripStops
                                   where tripStop.IsClosestStop
                                   select tripStop).FirstOrDefault();

                if (closestStop != null)
                {
                    this.viewModel.MapControlViewModel.SelectClosestStop(closestStop.StopId);
                }
                else
                {
                    this.viewModel.MapControlViewModel.UnselectClosestStop();
                }
            }
            catch (ObaException)
            {
                // This trip is over. Just ignore the exception, the user will see that the bus is at the end.
            }
        }

        /// <summary>
        /// Update the selected stop on the map view model.
        /// </summary>
        private void OnMapControlViewModelStopSelected(object sender, StopSelectedEventArgs e)
        {
            this.viewModel.TripTimelineControlViewModel.SelectStop(e.SelectedStopId);
            this.tripTimelineControl.ScrollToSelectedTripStop();
        }

        /// <summary>
        /// Called when the control is removed from the layout. Try and unselect the closest stop.
        /// </summary>
        private void OnControlUnloaded(object sender, RoutedEventArgs e)
        {
            this.viewModel.MapControlViewModel.UnselectClosestStop();
        }

        /// <summary>
        /// Update the selected stop on the trip details view model.
        /// </summary>
        private void OnTripTimelineControlViewModelStopSelected(object sender, StopSelectedEventArgs e)
        {
            var mapViewModel = this.viewModel.MapControlViewModel;
            mapViewModel.SelectStop(e.SelectedStopId);

            var mapCenter = mapViewModel.MapView;
            mapViewModel.MapView = new MapView(new Model.Point(e.Latitude, e.Longitude), mapCenter.ZoomLevel, true);
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