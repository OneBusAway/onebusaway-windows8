using OneBusAway.Model;
using OneBusAway.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    public sealed partial class TripDetailsPage : Page
    {
        private TripDetailsPageViewModel viewModel;

        public TripDetailsPage()
        {
            this.InitializeComponent();
            this.viewModel = (TripDetailsPageViewModel)this.DataContext;
            this.viewModel.MapControlViewModel.StopSelected += OnMapControlViewModelStopSelected;
            this.viewModel.TripTimelineControlViewModel.StopSelected += OnTripTimelineControlViewModelStopSelected;
        }

        /// <summary>
        /// Tries and restores the view model from the navigation controller stack.
        /// </summary>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if (NavigationController.TryRestoreViewModel(e.NavigationMode, ref this.viewModel))
            {
                this.DataContext = this.viewModel;
            }
            else
            {
                var tripViewModel = this.viewModel.TripTimelineControlViewModel;
                var mapViewModel = this.viewModel.MapControlViewModel;

                TrackingData trackingData = (TrackingData)e.Parameter;
                tripViewModel.TrackingData = trackingData;;

                // get the trip details:
                await tripViewModel.GetTripDetailsAsync();
                tripViewModel.SelectStop(trackingData.StopId);

                // Copy bus data into the map control:
                mapViewModel.BusStops = tripViewModel.TripDetails.TripStops.Cast<Stop>().ToList();
                mapViewModel.SelectStop(trackingData.StopId);
                await mapViewModel.FindRouteShapeAsync(trackingData.RouteId, trackingData.TripHeadsign);
            }

            base.OnNavigatedTo(e);
        }

        /// <summary>
        /// Persist the view model.
        /// </summary>
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            NavigationController.TryPersistViewModel(e.NavigationMode, this.viewModel);
            base.OnNavigatedFrom(e);
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
            this.viewModel.MapControlViewModel.SelectStop(e.SelectedStopId);
        }
    }
}
