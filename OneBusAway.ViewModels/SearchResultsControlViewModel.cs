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
        /// Used to do a double lock around the search task.
        /// </summary>
        private static readonly object searchLock = new object();

        /// <summary>
        /// Data access to OneBusAway.
        /// </summary>
        private ObaDataAccess obaDataAccess;

        /// <summary>
        /// These are the search results.
        /// </summary>
        private Route[] searchResults;

        /// <summary>
        /// This is a list of all of the possible routes that the user could search for.
        /// </summary>
        private static List<Route> allResults = new List<Route>();
        
        /// <summary>
        /// This bool is true until we're done loading routes from OBA.
        /// </summary>
        private bool isLoadingRoutes;

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
        public Route[] SearchResults
        {
            get
            {
                return this.searchResults;
            }
            set
            {
                SetProperty(ref this.searchResults, value);
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
                this.SearchResults = new Route[] { };
            }
            else
            {
                // Let's filter the results!
                this.SearchResults = (from result in listOfAllRoutes
                                      where (result.ShortName != null && result.ShortName.IndexOf(query, StringComparison.OrdinalIgnoreCase) > -1)
                                         || (result.Description != null && result.Description.IndexOf(query, StringComparison.OrdinalIgnoreCase) > -1)
                                      select result).ToArray();
            }
        }
    }
}
