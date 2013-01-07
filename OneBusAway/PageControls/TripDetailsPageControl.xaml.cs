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
    /// Page control for the trip details page.
    /// </summary>
    public sealed partial class TripDetailsPageControl : UserControl, IPageControl
    {
        /// <summary>
        /// The view model for the trip details.
        /// </summary>
        private TripDetailsPageControlViewModel viewModel;

        /// <summary>
        /// Creates the control.
        /// </summary>
        public TripDetailsPageControl()
        {
            this.InitializeComponent();

            this.viewModel = new TripDetailsPageControlViewModel();
            this.viewModel.MapControlViewModel.StopSelected += OnMapControlViewModelStopSelected;
            this.viewModel.TripTimelineControlViewModel.StopSelected += OnTripTimelineControlViewModelStopSelected;
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
        /// Initializes the page.
        /// </summary>
        public async Task InitializeAsync(object parameter)
        {
            var tripViewModel = this.viewModel.TripTimelineControlViewModel;
            var mapViewModel = this.viewModel.MapControlViewModel;
            
            TrackingData trackingData = (TrackingData)parameter;
            tripViewModel.TrackingData = trackingData;

            // get the trip details:
            await tripViewModel.GetTripDetailsAsync();
            tripViewModel.SelectStop(trackingData.StopId);

            // Copy bus data into the map control:
            mapViewModel.BusStops = null;
            mapViewModel.BusStops = tripViewModel.TripDetails.TripStops.Cast<Stop>().ToList();
            mapViewModel.SelectStop(trackingData.StopId);
            await mapViewModel.FindRouteShapeAsync(trackingData.RouteId, trackingData.TripHeadsign);
        }

        /// <summary>
        /// Restores the page.
        /// </summary>
        public Task RestoreAsync()
        {
            this.viewModel.MapControlViewModel.MapView.AnimateChange = true;
            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Update the selected stop on the map view model.
        /// </summary>
        private void OnMapControlViewModelStopSelected(object sender, StopSelectedEventArgs e)
        {
            this.viewModel.TripTimelineControlViewModel.SelectStop(e.SelectedStopId);
        }

        /// <summary>
        /// Update the selected stop on the trip details view model.
        /// </summary>
        private void OnTripTimelineControlViewModelStopSelected(object sender, StopSelectedEventArgs e)
        {
            var mapViewModel = this.viewModel.MapControlViewModel;
            mapViewModel.SelectStop(e.SelectedStopId);

            var mapCenter = mapViewModel.MapView;
            mapViewModel.MapView = new MapView(new Model.Point(e.Latitude, e.Longitude), mapCenter.ZoomLevel, true);
        }
    }
}
