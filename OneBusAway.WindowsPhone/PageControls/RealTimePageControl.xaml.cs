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

using System.Threading.Tasks;
using System.Windows.Controls;
using OneBusAway.ViewModels;
using OneBusAway.ViewModels.PageControls;

namespace OneBusAway.PageControls
{
    /// <summary>
    /// Page control for real time info.
    /// </summary>
    public sealed partial class RealTimePageControl : UserControl, IPinablePageControl
    {
        /// <summary>
        /// This is the view model for the real time page.
        /// </summary>
        private RealTimePageControlViewModel viewModel;

        /// <summary>
        /// Creates the real time page control.
        /// </summary>
        public RealTimePageControl()
        {
            this.InitializeComponent();
            this.viewModel = new RealTimePageControlViewModel();
            this.viewModel.MapControlViewModel.StopSelected += OnStopSelected;
        }

        /// <summary>
        /// Returns the view model for this page.
        /// </summary>
        public PageViewModelBase ViewModel
        {
            get
            {
                return this.viewModel;
            }
        }

        /// <summary>
        /// This is the id of the tile that can be pinned.
        /// </summary>
        public string TileId
        {
            get
            {
                return this.viewModel.RoutesAndStopsViewModel.StopId.Replace(' ', '_');
            }
        }

        /// <summary>
        /// Returns the name of the tile.
        /// </summary>
        public string TileName
        {
            get
            {
                return this.viewModel.RoutesAndStopsViewModel.StopHeaderText.ToUpper();
            }
        }

        /// <summary>
        /// Initializes the control.
        /// </summary>
        public async Task InitializeAsync(object parameter)
        {
            if (parameter is PageInitializationParameters)
            {
                PageInitializationParameters parameters = (PageInitializationParameters)parameter;
                string stopId = parameters.GetParameter<string>("stopId");
                double lat = parameters.GetParameter<double>("lat");
                double lon = parameters.GetParameter<double>("lon");

                if (!string.IsNullOrEmpty(stopId) && lat != 0 && lon != 0)
                {
                    await this.viewModel.NavigateDirectlyToStop(lat, lon, stopId);
                }
            }
            else if (parameter is StopSelectedEventArgs)
            {
                StopSelectedEventArgs stopSelectedEventArgs = (StopSelectedEventArgs)parameter;
                await this.viewModel.NavigateDirectlyToStop(
                    stopSelectedEventArgs.Latitude,
                    stopSelectedEventArgs.Longitude,
                    stopSelectedEventArgs.SelectedStopId,
                    stopSelectedEventArgs.StopName,
                    stopSelectedEventArgs.Direction);
            }
        }

        /// <summary>
        /// Restores asynchronously.
        /// </summary>
        public Task RestoreAsync()
        {
            this.viewModel.MapControlViewModel.MapView.AnimateChange = true;
            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Refresh the realtime data.
        /// </summary>
        public async Task RefreshAsync()
        {
            await this.viewModel.RoutesAndStopsViewModel.RefreshStopAsync();
        }

        /// <summary>
        /// Pages should be represent themselves as a string of parameters.
        /// </summary>
        public PageInitializationParameters GetParameters()
        {
            PageInitializationParameters parameters = new PageInitializationParameters();

            var selectedStop = this.viewModel.MapControlViewModel.SelectedBusStop;
            parameters.SetParameter("pageControl", this.GetType().FullName);
            parameters.SetParameter("stopId", selectedStop.StopId);
            parameters.SetParameter("lat", selectedStop.Latitude);
            parameters.SetParameter("lon", selectedStop.Longitude);

            return parameters;
        }

        /// <summary>
        /// Updates a secondary tile.
        /// </summary>
        public Task UpdateTileAsync(bool added)
        {
            // TO DO:
            //if (added)
            //{
            //    var busStop = this.viewModel.MapControlViewModel.SelectedBusStop;

            //    TileXMLBuilder builder = new TileXMLBuilder(this.TileId);
            //    await builder.AppendTileWithLargePictureAndTextAsync(this.TileId,
            //        busStop.Latitude,
            //        busStop.Longitude,
            //        this.TileName);
            //}
            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Called when the user selects a new stop. Update the IsCurrentControlPinned property.
        /// </summary>
        private async void OnStopSelected(object sender, StopSelectedEventArgs e)
        {
            await NavigationController.Instance.UpdateIsPinnableAsync(this);
        }
    }
}