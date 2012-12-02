using OneBusAway.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Search Contract item template is documented at http://go.microsoft.com/fwlink/?LinkId=234240

namespace OneBusAway
{
    // TODO: Edit the manifest to enable searching
    //
    // The package manifest could not be automatically updated.  Open the package manifest
    // file and ensure that support for activation for searching is enabled.
    /// <summary>
    /// This page displays search results when a global search is directed to this application.
    /// </summary>
    public sealed partial class SearchResultsPage : OneBusAway.Common.LayoutAwarePage
    {
        SearchResultsViewModel searchViewModel;

        public SearchResultsPage()
        {
            this.InitializeComponent();

            searchViewModel = this.DataContext as SearchResultsViewModel;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var queryText = e.Parameter as String;

            // TODO: Application-specific searching logic.  The search process is responsible for
            //       creating a list of user-selectable result categories:
            //
            //       filterList.Add(new Filter("<filter name>", <result count>));
            //
            //       Only the first filter, typically "All", should pass true as a third argument in
            //       order to start in an active state.  Results for the active filter are provided
            //       in Filter_SelectionChanged below.

            //var filterList = new List<Filter>();
            //filterList.Add(new Filter("All", 0, true));

            //// Communicate results through the view model
            //this.DefaultViewModel["QueryText"] = '\u201c' + queryText + '\u201d';
            //this.DefaultViewModel["Filters"] = filterList;
            //this.DefaultViewModel["ShowFilters"] = filterList.Count > 1;
        }
    }
}
