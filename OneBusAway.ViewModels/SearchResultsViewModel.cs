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
    public class SearchResultsViewModel : PageViewModelBase
    {
        private SearchResultsControlViewModel searchResultsControlViewModel;
        private MapControlViewModel mapControlViewModel;

        /// <summary>
        /// Creates the search results page view model.
        /// </summary>
        public SearchResultsViewModel()
        {
            this.SearchResultsControlViewModel = new SearchResultsControlViewModel();
            this.MapControlViewModel = new MapControlViewModel();
            this.HeaderViewModel.SubText = "Search Results";
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

        public MapControlViewModel MapControlViewModel
        {
            get
            {
                return this.mapControlViewModel;
            }
            set
            {
                SetProperty(ref this.mapControlViewModel, value);
            }
        }
        
        /// <summary>
        /// Searches asynchronously for a stop with the given name.
        /// </summary>
        public async Task SearchAsync(string queryText)
        {
            await this.SearchResultsControlViewModel.Search(queryText);
        }
    }
}
