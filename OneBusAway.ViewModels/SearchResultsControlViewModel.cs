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
        /// This task represents the work that has to happen with OBA before 
        /// we can respond to results.
        /// </summary>
        private static Task allResultsTask;

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
            this.IsLoadingRoutes = (allResultsTask == null || !allResultsTask.IsCompleted);
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
            // If this is null, then let's try and populate the list of all routes:
            if (allResultsTask == null)
            {
                lock (searchLock)
                {
                    if (allResultsTask == null)
                    {
                        allResultsTask = GetAllResults();
                    }
                }
            }

            // Wait for the all results to task to finish. If it's already finished then we'll just skip this.
            await allResultsTask;

            if (string.IsNullOrEmpty(query))
            {
                this.SearchResults = new Route[] { };
            }
            else
            {
                // Let's filter the results!
                this.SearchResults = (from result in allResults
                                      where (result.ShortName != null && result.ShortName.IndexOf(query, StringComparison.OrdinalIgnoreCase) > -1)
                                         || (result.Description != null && result.Description.IndexOf(query, StringComparison.OrdinalIgnoreCase) > -1)
                                      select result).ToArray();
            }
        }

        /// <summary>
        /// Gets all of the results.
        /// </summary>
        private async Task GetAllResults()
        {
            foreach (Agency agency in await this.obaDataAccess.GetAllAgencies())
            {
                allResults.AddRange(await this.obaDataAccess.GetAllRouteIdsForAgency(agency));
            }

            this.IsLoadingRoutes = false;
        }
    }
}
