using OneBusAway.Model;
using OneBusAway.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace OneBusAway.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SearchResultsPage : Page
    {
        private SearchResultsViewModel searchResultsViewModel;

        public SearchResultsPage()
        {
            this.InitializeComponent();
            searchResultsViewModel = (SearchResultsViewModel)this.DataContext;

            this.searchResultsViewModel.MapControlViewModel.StopSelected += OnMapControlViewModelStopSelected;        
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if (NavigationController.TryRestoreViewModel(e.NavigationMode, ref this.searchResultsViewModel))
            {
                this.DataContext = this.searchResultsViewModel;
            }
            else
            {
                await this.searchResultsViewModel.MapControlViewModel.FindUserLocationAsync();
            }

            var queryText = e.Parameter as string;
            if (!String.IsNullOrEmpty(queryText))
            {
                await searchResultsViewModel.SearchAsync(queryText);
            }

            base.OnNavigatedTo(e);
        }

        /// <summary>
        /// Invoked when the user navigates from this page.
        /// </summary>
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            NavigationController.TryPersistViewModel(e.NavigationMode, this.searchResultsViewModel);
            base.OnNavigatedFrom(e);
        }

        /// <summary>
        /// When the user selects a bus stop, see if we can navigate to a page to display the route / stop combination.
        /// </summary>
        private void OnMapControlViewModelStopSelected(object sender, StopSelectedEventArgs e)
        {
            NavigationController.Instance.GoToMainPageCommand.Execute(e);
        }
    }
}
