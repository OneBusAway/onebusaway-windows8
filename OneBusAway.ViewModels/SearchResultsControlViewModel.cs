using OneBusAway.DataAccess;
using OneBusAway.DataAccess.BingService;
using OneBusAway.Model;
using OneBusAway.Model.BingService;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        /// List of locations obtained by querying Bing maps service
        /// </summary>
        private SearchLocationResultViewModel[] bingMapsSearchResults;

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
        private object selectedResult;

        /// <summary>
        /// Fires whenever a user selects a new route.
        /// </summary>
        public event EventHandler<RouteSelectedEventArgs> RouteSelected;

        /// <summary>
        /// Fires when a user selects a location from the search results
        /// </summary>
        public event EventHandler<LocationSelectedEventArgs> LocationSelected;

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
                FirePropertyChanged("SearchResultsExist");

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

        public bool SearchResultsExist
        {
            get
            {
                return (SearchResults != null && SearchResults.Count() > 0)
                    || (BingMapsSearchResults != null && BingMapsSearchResults.Length > 0);
            }
        }

        public SearchLocationResultViewModel[] BingMapsSearchResults
        {
            get
            {
                return this.bingMapsSearchResults;
            }
            set
            {
                if (this.bingMapsSearchResults != null)
                {
                    foreach (var result in this.bingMapsSearchResults)
                    {
                        result.LocationSelected -= OnSearchLocationResultViewModelLocationSelected;
                    }
                }

                SetProperty(ref this.bingMapsSearchResults, value);
                FirePropertyChanged("SearchResultsExist");

                if (this.bingMapsSearchResults != null)
                {
                    foreach (var result in this.bingMapsSearchResults)
                    {
                        result.LocationSelected += OnSearchLocationResultViewModelLocationSelected;
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
        /// Private helper method returns all routes.
        /// </summary>
        private async Task<List<Route>> LoadAllRoutesAsync()
        {
            var listOfAllRoutes = await AllRoutesCache.GetAllRoutesAsync();
            this.IsLoadingRoutes = false;
            return listOfAllRoutes;
        }

        /// <summary>
        /// Searches all agencies for all bus numbers.
        /// </summary>
        public async Task SearchAsync(string query, OneBusAway.Model.Point userLocation = null)
        {
            try
            {
                if (string.IsNullOrEmpty(query))
                {
                    this.SearchResults = null;
                    this.BingMapsSearchResults = null;
                }
                else
                {
                    // If the user selected a suggestion, then that means they want a specific bus:
                    if (query.StartsWith("BUS", StringComparison.OrdinalIgnoreCase))
                    {
                        await DisplayAndSelectSpecificRouteAsync(query.Substring(3).Trim());
                    }
                    else
                    {
                        var listOfAllRoutes = await this.LoadAllRoutesAsync();

                        // Let's filter the results!
                        this.SearchResults = (from result in listOfAllRoutes
                                              where (result.ShortName != null && result.ShortName.IndexOf(query, StringComparison.OrdinalIgnoreCase) > -1)
                                                 || (result.Description != null && result.Description.IndexOf(query, StringComparison.OrdinalIgnoreCase) > -1)
                                              select new SearchRouteResultViewModel(result)).ToArray();

                        var bingMapResults = await BingMapsServiceHelper.GetLocationByQuery(query, Utilities.Confidence.Low, userLocation);
                        this.BingMapsSearchResults = (from result in bingMapResults
                                                      select new SearchLocationResultViewModel(result)).ToArray();
                    }

                }
            }
            catch
            {

            }
        }

        /// <summary>
        /// Loads and displays a specific route.
        /// </summary>
        public async Task DisplayAndSelectSpecificRouteAsync(string routeIdOrShortName)
        {
            var listOfAllRoutes = await this.LoadAllRoutesAsync();
            this.BingMapsSearchResults = null;

            this.SearchResults = (from result in listOfAllRoutes
                                  where string.Equals(routeIdOrShortName, result.ShortName, StringComparison.OrdinalIgnoreCase)
                                     || string.Equals(routeIdOrShortName, result.Id, StringComparison.OrdinalIgnoreCase)
                                  select new SearchRouteResultViewModel(result)).ToArray();

            if (this.SearchResults.Length == 1)
            {
                await this.SearchResults[0].SelectAsync();
            }
        }

        /// <summary>
        /// Returns a list of suggestions for the user the OBA list of routes.
        /// </summary>
        public async Task<IEnumerable<string>> GetSuggestionsAsync(string query, OneBusAway.Model.Point userLocation)
        {
            var listOfAllRoutes = await AllRoutesCache.GetAllRoutesAsync();
            return from result in listOfAllRoutes
                   where (result.ShortName != null && result.ShortName.IndexOf(query, StringComparison.OrdinalIgnoreCase) > -1)
                      || (result.Description != null && result.Description.IndexOf(query, StringComparison.OrdinalIgnoreCase) > -1)
                   select string.Format(CultureInfo.CurrentCulture, "BUS {0}", result.ShortName);
        }

        /// <summary>
        /// Selects specific routes from an enumerable of stop ids.
        /// </summary>
        public async Task SelectSpecificRoutesAsync(IEnumerable<string> routeIds)
        {
            var listOfAllRoutes = await this.LoadAllRoutesAsync();
            this.SearchResults = (from result in listOfAllRoutes
                                  where routeIds.Contains(result.Id)
                                  select new SearchRouteResultViewModel(result)).ToArray();
        }

        /// <summary>
        /// Should be called to let us know that the currently selected result is being processed.
        /// </summary>
        public void SetIsLoadingCurrentRoute(bool isLoading)
        {
            var searchRouteResultViewModel = this.selectedResult as SearchRouteResultViewModel;
            if (searchRouteResultViewModel != null)
            {
                searchRouteResultViewModel.IsGettingRouteData = isLoading;
            }
        }

        /// <summary>
        /// Called when the user selects a route from the results.
        /// </summary>
        private void OnResultRouteSelected(object sender, RouteSelectedEventArgs e)
        {
            ResetSelectedResult();

            this.selectedResult = sender as SearchRouteResultViewModel;

            var routeSelected = this.RouteSelected;
            if (routeSelected != null)
            {
                routeSelected(this, e);
            }
        }

        /// <summary>
        /// Called when the user selects an address from the bing search results.
        /// </summary>
        void OnSearchLocationResultViewModelLocationSelected(object sender, LocationSelectedEventArgs e)
        {
            ResetSelectedResult();

            this.selectedResult = sender as SearchLocationResultViewModel;

            var locationSelected = this.LocationSelected;
            if (locationSelected != null)
            {
                locationSelected(this, e);
            }
        }

        /// <summary>
        /// Resets the last selected item from the search results control.
        /// </summary>
        private void ResetSelectedResult()
        {
            if (this.selectedResult != null)
            {
                if (this.selectedResult is SearchLocationResultViewModel)
                {
                    (selectedResult as SearchLocationResultViewModel).IsSelected = false;
                }
                else if (this.selectedResult is SearchRouteResultViewModel)
                {
                    (selectedResult as SearchRouteResultViewModel).IsSelected = false;
                    (selectedResult as SearchRouteResultViewModel).IsGettingRouteData = false;
                }
            }
        }
    }
}
