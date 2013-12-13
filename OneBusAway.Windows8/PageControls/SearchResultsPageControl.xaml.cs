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
using OneBusAway.ViewModels;
using OneBusAway.ViewModels.PageControls;
using OneBusAway.Platforms.Windows8;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace OneBusAway.PageControls
{
    /// <summary>
    /// Page control for the search results.
    /// </summary>
    public sealed partial class SearchResultsPageControl : UserControl, IPageControl
    {
        /// <summary>
        /// View model for the search results page.
        /// </summary>
        private SearchResultsPageControlViewModel viewModel;

        /// <summary>
        /// Creates the control.
        /// </summary>
        public SearchResultsPageControl()
        {
            this.InitializeComponent();

            this.viewModel = new SearchResultsPageControlViewModel(new DefaultUIHelper(this.Dispatcher));
            this.viewModel.MapControlViewModel.StopSelected += OnMapControlViewModelStopSelected;        
        }

        public PageViewModelBase ViewModel
        {
            get 
            {
                return this.viewModel;
            }
        }

        /// <summary>
        /// Initializes the search results page.
        /// </summary>
        public async Task InitializeAsync(object parameter)
        {
            this.viewModel.MapControlViewModel.Shapes = null;
            await this.viewModel.MapControlViewModel.FindUserLocationAsync();

            var queryText = parameter as string;
            if (!String.IsNullOrEmpty(queryText))
            {
                await this.viewModel.SearchAsync(queryText);
            }
        }

        /// <summary>
        /// Restores state from before.
        /// </summary>
        public Task RestoreAsync()
        {
            this.viewModel.MapControlViewModel.SelectedBusStop = null;
            this.viewModel.MapControlViewModel.BusStops.ClearExistingStops = true;
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
        /// When the user selects a bus stop, see if we can navigate to a page to display the route / stop combination.
        /// </summary>
        private void OnMapControlViewModelStopSelected(object sender, StopSelectedEventArgs e)
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