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
    public class SearchResultsViewModel : PageViewModelBase
    {
        private ObservableCollection<SearchResult> searchResults;
        private string queryText;
        private bool resultsEmpty = false;

        public SearchResultsViewModel()
        {

        }

        public string QueryText
        {
            get
            {
                return queryText;
            }
            set
            {
                SetProperty(ref queryText, value);
            }
        }

        public bool ResultsEmpty
        {
            get
            {
                return resultsEmpty;
            }
            set
            {
                SetProperty(ref resultsEmpty, value);
            }
        }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification="Enables binding to source control")]
        public ObservableCollection<SearchResult> SearchResults
        {
            get
            {
                if (searchResults == null)
                {
                    searchResults = new ObservableCollection<SearchResult>();
                }

                return searchResults;
            }
            set
            {
                SetProperty(ref searchResults, value);
            }
        }
         
        public async void Search(string _queryText)
        {
            // Set to false to make app believe search results are present - so that the error message is not shown
            ResultsEmpty = false;

            if (!String.IsNullOrEmpty(_queryText))
            {
                queryText = _queryText;

                List<Location> results = await OneBusAway.DataAccess.BingService.BingMapsServiceHelper.GetLocationByQuery(queryText);

                searchResults.Clear();

                if (results != null)
                {

                    foreach (Location result in results)
                    {
                        SearchResult searchResult = new SearchResult(result.GetType());
                        searchResult.Title = result.Name;
                        searchResult.SubTitle = result.Address.FormattedAddress;

                        SearchResults.Add(searchResult);
                    }

                    ResultsEmpty = false;
                }
                else
                {
                    // TODO - Display error text on the search results page
                    ResultsEmpty = true;
                }
            }
            else
            {
                ResultsEmpty = true;
            }
        }
    }
}
