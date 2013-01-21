using OneBusAway.DataAccess;
using OneBusAway.Model;
using OneBusAway.Model.BingService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneBusAway.ViewModels
{
    /// <summary>
    /// View model for the search results page.
    /// </summary>
    public class SearchResultsPageControlViewModel : PageViewModelBase
    {
        private SearchResultsControlViewModel searchResultsControlViewModel;
        private ObaDataAccess obaDataAccess;

        /// <summary>
        /// Creates the search results page view model.
        /// </summary>
        public SearchResultsPageControlViewModel()
        {
            this.HeaderViewModel.SubText = "SEARCH RESULTS";

            this.SearchResultsControlViewModel = new SearchResultsControlViewModel();
            this.SearchResultsControlViewModel.RouteSelected += OnSearchResultsControlViewModelRouteSelected;
            this.SearchResultsControlViewModel.LocationSelected += OnSearchResultsControlViewModelLocationSelected;

            this.MapControlViewModel.RefreshBusStopsOnMapViewChanged = false;
            this.obaDataAccess = new ObaDataAccess();
        }            

        public SearchResultsControlViewModel SearchResultsControlViewModel
        {
            get
            {
                return this.searchResultsControlViewModel;
            }
            set
            {
                SetProperty(ref this.searchResultsControlViewModel, value);
            }
        }

        /// <summary>
        /// Searches asynchronously for a stop with the given name.
        /// </summary>
        public async Task SearchAsync(string queryText)
        {            
            await this.SearchResultsControlViewModel.SearchAsync(queryText, MapControlViewModel.UserLocation);
        }

        /// <summary>
        /// Returns a list of suggestions.
        /// </summary>
        public async Task<IEnumerable<string>> GetSuggestionsAsync(string queryText)
        {
            return await this.SearchResultsControlViewModel.GetSuggestionsAsync(queryText, MapControlViewModel.UserLocation);
        }

        /// <summary>
        /// Called when the user selects a route's search result.
        /// </summary>
        private async void OnSearchResultsControlViewModelRouteSelected(object sender, RouteSelectedEventArgs e)
        {
            // If this is true then the user just clicked from a location-based search. Clear old bus stops!
            if (this.MapControlViewModel.RefreshBusStopsOnMapViewChanged)
            {
                this.MapControlViewModel.BusStops = null;                
            }

            this.searchResultsControlViewModel.SetIsLoadingCurrentRoute(true);

            try
            {
                var routes = await this.obaDataAccess.GetRouteDataAsync(e.RouteId);

                this.MapControlViewModel.BusStops = (from route in routes
                                                     from stop in route.Stops
                                                     select stop).ToList();

                this.MapControlViewModel.Shapes = (from route in routes
                                                   from shape in route.Shapes
                                                   select shape).ToList();
            }
            finally
            {
                this.searchResultsControlViewModel.SetIsLoadingCurrentRoute(false);
            }

            this.MapControlViewModel.ZoomToRouteShape();
        }

        /// <summary>
        /// Called when the user selects a location as the search result
        /// </summary>
        /// <param name="sender">SearchResultsControlViewModel</param>
        /// <param name="e">SearchLocationResultViewModel</param>
        private async void OnSearchResultsControlViewModelLocationSelected(object sender, LocationSelectedEventArgs e)
        {
            if (!this.MapControlViewModel.RefreshBusStopsOnMapViewChanged)
            {
                this.MapControlViewModel.BusStops = null;
                this.MapControlViewModel.Shapes = null;
            }
            
            var point = new OneBusAway.Model.Point(e.Location.Point.Coordinates[0], e.Location.Point.Coordinates[1]);
            this.MapControlViewModel.UserLocation = point;
            this.MapControlViewModel.MapView = new MapView(point, ViewModelConstants.ZoomedInMapZoom, true);            

            // Find all the bus stops at this location and then show the routes for this address:
            await this.MapControlViewModel.RefreshStopsForLocationAsync();

            // Find all of the unique routes for this location:
            await this.SearchResultsControlViewModel.SelectSpecificRoutesAsync((from stop in this.MapControlViewModel.BusStops
                                                                                from route in stop.Routes
                                                                                select route.Id).Distinct());
        } 
    }
}
