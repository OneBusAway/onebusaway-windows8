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
    /// This class contains schedule data for a particular route / stop combination.
    /// </summary>
    public sealed partial class TimeTablePageControl : UserControl, IPageControl
    {
        /// <summary>
        /// The view model for the time table page.
        /// </summary>
        private TimeTablePageControlViewModel viewModel;

        /// <summary>
        /// Creates the page control.
        /// </summary>
        public TimeTablePageControl()
        {
            this.InitializeComponent();
            this.viewModel = new TimeTablePageControlViewModel();
        }

        /// <summary>
        /// Return the view model.
        /// </summary>
        public PageViewModelBase ViewModel
        {
            get 
            {
                return this.viewModel;
            }
        }

        /// <summary>
        /// Initializes the time table page.
        /// </summary>
        public async Task InitializeAsync(object parameter)
        {
            this.viewModel.MapControlViewModel.BusStops = null;

            var routeMapViewModel = parameter as RouteMapsAndSchedulesControlViewModel;
            if (routeMapViewModel != null)
            {
                await this.viewModel.SetRouteAndStopData(
                    routeMapViewModel.StopName,
                    routeMapViewModel.StopId, 
                    routeMapViewModel.RouteName,
                    routeMapViewModel.RouteId);

                await this.viewModel.GetRouteData(routeMapViewModel.StopId, routeMapViewModel.RouteId);
            }

            this.viewModel.MapControlViewModel.ZoomToRouteShape();
        }

        /// <summary>
        /// Restore asynchronously.
        /// </summary>
        public Task RestoreAsync()
        {
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
    }
}
