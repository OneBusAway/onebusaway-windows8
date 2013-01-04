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
    public class MainPageViewModel : PageViewModelBase
    {
        private RoutesAndStopsControlViewModel routesAndStopsViewModel;
        private MapControlViewModel mapControlViewModel;
        private ObaDataAccess obaDataAccess;
        
        public MainPageViewModel()
        {
            this.RoutesAndStopsViewModel = new RoutesAndStopsControlViewModel();
            this.MapControlViewModel = new MapControlViewModel();
            this.obaDataAccess = new ObaDataAccess();
        }

        #region Public Properties

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

        public MapControlViewModel MapControlViewModel
        {
            get
            {
                return this.mapControlViewModel;
            }
            set
            {                
                SetProperty(ref this.mapControlViewModel, value);
                this.mapControlViewModel.StopSelected += OnMapControlViewModelStopSelected;
            }
        }

        public string BingMapCredentials
        {
            get
            {
                return UtilitiesConstants.BingMapCredentials;
            }
        }

        /// <summary>
        /// Navigates directly to a particular stop.
        /// </summary>
        public async Task NavigateDirectlyToStop(double latitude, double longitude, string selectedStopId, string stopName, string direction)
        {
            var center = new Model.Point(latitude, longitude);

            await this.MapControlViewModel.RefreshStopsForLocationAsync(MapView.Current);
            this.MapControlViewModel.SelectStop(selectedStopId);

            await this.RoutesAndStopsViewModel.PopulateStopAsync(
                stopName,
                selectedStopId,
                direction);
            
            this.HeaderViewModel.SubText = "REAL TIME";
        }

        #endregion
        #region Event Handlers

        /// <summary>
        /// Called when the user selects a stop.
        /// </summary>
        private async void OnMapControlViewModelStopSelected(object sender, StopSelectedEventArgs e)
        {
            await this.routesAndStopsViewModel.PopulateStopAsync(e.StopName, e.SelectedStopId, e.Direction);
            this.HeaderViewModel.SubText = "REAL TIME";
        }

        #endregion
    }
}
