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
using OneBusAway.Model;
using OneBusAway.ViewModels;
using OneBusAway.ViewModels.PageControls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using System.Windows.Controls;
using Windows.UI.Core;

namespace OneBusAway.PageControls
{
    /// <summary>
    /// The main page control.
    /// </summary>
    public sealed partial class FavoritesPageControl : UserControl, IPageControl
    {
        /// <summary>
        /// The view model
        /// </summary>
        private FavoritesPageControlViewModel viewModel;

        /// <summary>
        /// Creates the control.
        /// </summary>
        public FavoritesPageControl()
        {
            this.InitializeComponent();
            this.viewModel = new FavoritesPageControlViewModel();
            this.viewModel.StopSelected += OnViewModelStopSelected;
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
        /// Initializes the favorites controls.
        /// </summary>
        public async Task InitializeAsync(object parameter)
        {
            this.viewModel.MapControlViewModel.Shapes = null;
            this.viewModel.MapControlViewModel.UnSelectStop();

            await this.viewModel.MapControlViewModel.FindUserLocationAsync();
            await this.viewModel.RoutesAndStopsViewModel.PopulateFavoritesAsync();
        }

        /// <summary>
        /// Restore asynchronously.
        /// </summary>
        public Task RestoreAsync()
        {
            this.viewModel.MapControlViewModel.MapView = NavigationController.Instance.MapView;
            this.viewModel.MapControlViewModel.UnSelectStop();

            // Don't let this restore stop the page transition. Populate it on idle after we've refreshed the UI:
            var ignored = CoreWindow.GetForCurrentThread().Dispatcher.RunIdleAsync(async args => await this.viewModel.RoutesAndStopsViewModel.PopulateFavoritesAsync());
            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Nothing to do here.
        /// </summary>
        public async Task RefreshAsync()
        {
            await this.viewModel.RoutesAndStopsViewModel.PopulateFavoritesAsync();
        }

        /// <summary>
        /// When the user selects a stop, we need to navigate to it.
        /// </summary>
        private void OnViewModelStopSelected(object sender, StopSelectedEventArgs e)
        {
            NavigationController.Instance.GoToRealTimePageCommand.Execute(e);
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