using OneBusAway.DataAccess;
using OneBusAway.DataAccess.BingService;
using OneBusAway.Model;
using OneBusAway.Model.BingService;
using OneBusAway.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneBusAway.ViewModels
{
    /// <summary>
    /// View model for the main page.
    /// </summary>
    public class RealTimePageControlViewModel : PageViewModelBase, ITrackingDataViewModel
    {
        private RoutesAndStopsControlViewModel routesAndStopsViewModel;
        
        public RealTimePageControlViewModel()
        {
            this.HeaderViewModel.SubText = "REAL TIME";
            this.RoutesAndStopsViewModel = new RoutesAndStopsControlViewModel();
            this.MapControlViewModel.StopSelected += OnMapControlViewModelStopSelectedAsync;
        }

        #region Public Properties

        /// <summary>
        /// Gets / sets the real time data.
        /// </summary>
        public TrackingData[] RealTimeData
        {
            get
            {
                return this.routesAndStopsViewModel.RealTimeData;
            }
            set
            {
                this.routesAndStopsViewModel.RealTimeData = value;
            }
        }

        /// <summary>
        /// The view model for the routes and stops page.
        /// </summary>
        public RoutesAndStopsControlViewModel RoutesAndStopsViewModel
        {
            get
            {
                return this.routesAndStopsViewModel;                
            }
            set
            {
                SetProperty(ref this.routesAndStopsViewModel, value);
            }
        }

        /// <summary>
        /// Navigates directly to a particular stop.
        /// </summary>
        public async Task NavigateDirectlyToStop(double latitude, double longitude, string selectedStopId, string stopName, string direction)
        {
            await this.MapControlViewModel.RefreshStopsForLocationAsync(MapView.Current);
            this.MapControlViewModel.SelectStop(selectedStopId);

            await this.RoutesAndStopsViewModel.PopulateStopAsync(
                stopName,
                selectedStopId,
                direction);
        }

        /// <summary>
        /// Navigates directly to a particular stop.
        /// </summary>
        public async Task NavigateDirectlyToStop(double latitude, double longitude, string selectedStopId)
        {
            var center = new Model.Point(latitude, longitude);
            this.MapControlViewModel.MapView = new MapView(center, ViewModelConstants.ZoomedInMapZoom, false);

            await this.MapControlViewModel.RefreshStopsForLocationAsync(MapView.Current);
            this.MapControlViewModel.SelectStop(selectedStopId);

            await this.RoutesAndStopsViewModel.PopulateStopAsync(selectedStopId);
        }

        #endregion
        #region Event Handlers

        /// <summary>
        /// Called when the user selects a stop.
        /// </summary>
        protected async void OnMapControlViewModelStopSelectedAsync(object sender, StopSelectedEventArgs e)
        {
            await this.routesAndStopsViewModel.PopulateStopAsync(e.StopName, e.SelectedStopId, e.Direction);            
        }

        #endregion        
    }
}
