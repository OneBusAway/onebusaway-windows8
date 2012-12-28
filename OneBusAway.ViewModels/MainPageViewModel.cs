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
        RoutesAndStopsControlViewModel routesAndStopsViewModel;
        MapControlViewModel mapControlViewModel;
        
        public MainPageViewModel()
        {
            this.RoutesAndStopsViewModel = new RoutesAndStopsControlViewModel();
            this.MapControlViewModel = new MapControlViewModel();
            Load();
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
        
        #endregion

        #region Public Methods

        public async void Load()
        {
            try
            {
                await this.RoutesAndStopsViewModel.PopulateFavoritesAsync();
            }
            catch 
            {
            }
        }
        
        #endregion

        #region Event Handlers

        /// <summary>
        /// Called when the user selects a stop.
        /// </summary>
        private async void OnMapControlViewModelStopSelected(object sender, StopSelectedEventArgs e)
        {
            await this.routesAndStopsViewModel.PopulateStopAsync(e.StopName, e.SelectedStopId, e.Direction);
        }

        #endregion
    }
}
