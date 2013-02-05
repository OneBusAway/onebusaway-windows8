using OneBusAway.Model;
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
    /// The main page control.
    /// </summary>
    public sealed partial class FavoritesPageControl : UserControl, IPageControl
    {
        /// <summary>
        /// The view model
        /// </summary>
        private FavoritesPageControlViewModel viewModel;

        /// <summary>
        /// Creates the control.
        /// </summary>
        public FavoritesPageControl()
        {
            this.InitializeComponent();
            this.viewModel = new FavoritesPageControlViewModel();
            this.viewModel.StopSelected += OnViewModelStopSelected;
        }

        /// <summary>
        /// Returns the view model.
        /// </summary>
        public PageViewModelBase ViewModel
        {
            get
            {
                return this.viewModel;
            }
        }

        /// <summary>
        /// Initializes the favorites controls.
        /// </summary>
        public async Task InitializeAsync(object parameter)
        {
            this.viewModel.MapControlViewModel.Shapes = null;
            this.viewModel.MapControlViewModel.UnSelectStop();

            await Favorites.Initialize();
            await this.viewModel.MapControlViewModel.FindUserLocationAsync();
            await this.viewModel.RoutesAndStopsViewModel.PopulateFavoritesAsync();
        }

        /// <summary>
        /// Restore asynchronously.
        /// </summary>
        public async Task RestoreAsync()
        {
            this.viewModel.MapControlViewModel.MapView = NavigationController.Instance.MapView;
            this.viewModel.MapControlViewModel.UnSelectStop();
            await this.viewModel.RoutesAndStopsViewModel.PopulateFavoritesAsync();
        }

        /// <summary>
        /// Nothing to do here.
        /// </summary>
        public async Task RefreshAsync()
        {
            await this.viewModel.RoutesAndStopsViewModel.PopulateFavoritesAsync();
        }

        /// <summary>
        /// When the user selects a stop, we need to navigate to it.
        /// </summary>
        private void OnViewModelStopSelected(object sender, StopSelectedEventArgs e)
        {
            NavigationController.Instance.GoToRealTimePageCommand.Execute(e);            
        }
    }
}
