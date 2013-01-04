using OneBusAway.DataAccess;
using OneBusAway.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneBusAway.ViewModels
{
    /// <summary>
    /// View model for the search results page.
    /// </summary>
    public class SearchResultsControlViewModel : ViewModelBase
    {
        /// <summary>
        /// Data access to OneBusAway.
        /// </summary>
        private ObaDataAccess obaDataAccess;

        /// <summary>
        /// These are the search results.
        /// </summary>
        private SearchRouteResultViewModel[] searchResults;

        /// <summary>
        /// This is a list of all of the possible routes that the user could search for.
        /// </summary>
        private static List<Route> allResults = new List<Route>();
        
        /// <summary>
        /// This bool is true until we're done loading routes from OBA.
        /// </summary>
        private bool isLoadingRoutes;

        /// <summary>
        /// This is the selected search result.
        /// </summary>
        private SearchRouteResultViewModel selectedResult;

        /// <summary>
        /// Fires whenever a user selects a new route.
        /// </summary>
        public event EventHandler<RouteSelectedEventArgs> RouteSelected;

        /// <summary>
        /// Creates the search results control view model.
        /// </summary>
        public SearchResultsControlViewModel()
        {
            this.obaDataAccess = new ObaDataAccess();
            this.IsLoadingRoutes = AllRoutesCache.IsCacheUpToDate();
        }

        /// <summary>
        /// Gets / sets the search results.
        /// </summary>
        public SearchRouteResultViewModel[] SearchResults
        {
            get
            {
                return this.searchResults;
            }
            set
            {
                // Remove old event handlers:
                if (this.searchResults != null)
                {
                    foreach (var result in this.searchResults)
                    {
                        result.RouteSelected -= OnResultRouteSelected;
                    }
                }

                SetProperty(ref this.searchResults, value);

                // Add new event handlers:
                if (this.searchResults != null)
                {
                    foreach (var result in this.searchResults)
                    {
                        result.RouteSelected += OnResultRouteSelected;
                    }
                }
            }
        }

        /// <summary>
        /// Return while we are still loading routes from OBA.
        /// </summary>
        public bool IsLoadingRoutes
        {
            get
            {
                return this.isLoadingRoutes;
            }
            set
            {
                SetProperty(ref this.isLoadingRoutes, value);
            }
        }

        /// <summary>
        /// Searches all agencies for all bus numbers.
        /// </summary>
        public async Task Search(string query)
        {
            var listOfAllRoutes = await AllRoutesCache.GetAllRoutesAsync();
            this.IsLoadingRoutes = false;

            if (string.IsNullOrEmpty(query))
            {
                this.SearchResults = new SearchRouteResultViewModel[] { };
            }
            else
            {
                // Let's filter the results!
                this.SearchResults = (from result in listOfAllRoutes
                                      where (result.ShortName != null && result.ShortName.IndexOf(query, StringComparison.OrdinalIgnoreCase) > -1)
                                         || (result.Description != null && result.Description.IndexOf(query, StringComparison.OrdinalIgnoreCase) > -1)
                                      select new SearchRouteResultViewModel(result)).ToArray();
            }
        }

        /// <summary>
        /// Called when the user selects a route from the results.
        /// </summary>
        private void OnResultRouteSelected(object sender, RouteSelectedEventArgs e)
        {
            if (this.selectedResult != null)
            {
                this.selectedResult.IsSelected = false;
            }

            this.selectedResult = sender as SearchRouteResultViewModel;

            var routeSelected = this.RouteSelected;
            if (routeSelected != null)
            {
                routeSelected(this, e);
            }
        }
    }
}
