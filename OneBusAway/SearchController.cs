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
using OneBusAway.PageControls;
using OneBusAway.Pages;
using OneBusAway.ViewModels.PageControls;
using Windows.ApplicationModel.Search;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace OneBusAway
{
    /// <summary>
    /// A helper class that is used to handle search queries and suggestions from the Windows UI.
    /// </summary>
    public class SearchController
    {
        /// <summary>
        /// The search pane.
        /// </summary>
        private SearchPane searchPane;

        /// <summary>
        /// Creates the search controller.
        /// </summary>
        public SearchController()
        {
            this.searchPane = SearchPane.GetForCurrentView();
            if (this.searchPane != null)
            {
                this.searchPane.QuerySubmitted += OnAppQuerySubmitted;            
            }
        }

        /// <summary>
        /// Shows the search pane.
        /// </summary>
        public void ShowSearchPane()
        {
            if (this.searchPane != null)
            {
                this.searchPane.Show();
            }
        }

        /// <summary>
        /// Called when the user submits a search query.
        /// </summary>
        private async void OnAppQuerySubmitted(SearchPane sender, SearchPaneQuerySubmittedEventArgs args)
        {
            var frame = Window.Current.Content as Frame;
            if (frame != null)
            {
                var searchResultsPage = frame.Content as MainPage;
                if (searchResultsPage != null)
                {
                    var viewModel = searchResultsPage.DataContext as SearchResultsPageControlViewModel;
                    if (viewModel != null)
                    {
                        await viewModel.SearchAsync(args.QueryText);
                    }
                    else
                    {
                        await NavigationController.Instance.NavigateToPageControlAsync<SearchResultsPageControl>(args.QueryText);
                    }
                }
            }
        }
    }
}
