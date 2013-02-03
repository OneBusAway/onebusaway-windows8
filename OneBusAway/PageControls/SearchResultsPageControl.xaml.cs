using OneBusAway.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace OneBusAway.PageControls
{
    /// <summary>
    /// Page control for the search results.
    /// </summary>
    public sealed partial class SearchResultsPageControl : UserControl, IPageControl
    {
        /// <summary>
        /// View model for the search results page.
        /// </summary>
        private SearchResultsPageControlViewModel viewModel;

        /// <summary>
        /// Creates the control.
        /// </summary>
        public SearchResultsPageControl()
        {
            this.InitializeComponent();

            this.viewModel = new SearchResultsPageControlViewModel(new DefaultUIHelper(this.Dispatcher));
            this.viewModel.MapControlViewModel.StopSelected += OnMapControlViewModelStopSelected;        
        }

        public PageViewModelBase ViewModel
        {
            get 
            {
                return this.viewModel;
            }
        }

        /// <summary>
        /// Initializes the search results page.
        /// </summary>
        public async Task InitializeAsync(object parameter)
        {
            this.viewModel.MapControlViewModel.Shapes = null;
            await this.viewModel.MapControlViewModel.FindUserLocationAsync();

            var queryText = parameter as string;
            if (!String.IsNullOrEmpty(queryText))
            {
                await this.viewModel.SearchAsync(queryText);
            }
        }

        /// <summary>
        /// Restores state from before.
        /// </summary>
        public Task RestoreAsync()
        {
            this.viewModel.MapControlViewModel.BusStops.ClearExistingStops = true;
            this.viewModel.MapControlViewModel.MapView.AnimateChange = true;
            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Nothing to do here.
        /// </summary>
        public Task RefreshAsync()
        {
            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// When the user selects a bus stop, see if we can navigate to a page to display the route / stop combination.
        /// </summary>
        private void OnMapControlViewModelStopSelected(object sender, StopSelectedEventArgs e)
        {
            NavigationController.Instance.GoToRealTimePageCommand.Execute(e);
        }
    }
}
