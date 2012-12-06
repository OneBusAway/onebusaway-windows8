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
        private Stop[] busStops;
        
        public MainPageViewModel()
        {
            this.HeaderViewModel.FavoritesIsEnabled = false;

            this.RoutesAndStopsViewModel = new RoutesAndStopsControlViewModel();
            this.MapControlViewModel = new ViewModels.MapControlViewModel();

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
            }
        }

        public string BingMapCredentials
        {
            get
            {
                return UtilitiesConstants.BingMapCredentials;
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Justification="Must be bound to an array or list")]
        public Stop[] BusStops
        {
            get
            {
                return busStops;
            }
            private set
            {
                SetProperty(ref this.busStops, value);
            }
        }        
        
        #endregion

        #region Public Methods

        public async void Load()
        {
            try
            {
                await this.RoutesAndStopsViewModel.PopulateAsync();
            }
            catch 
            {
            }
        }

        public async void RefreshStopsForLocationAsync(double latitude, double longitude, double latitudeSpan, double longitudeSpan)
        {
            try
            {
                this.BusStops = await new ObaDataAccess().GetStopsForLocationAsync(latitude, longitude, latitudeSpan, longitudeSpan);
            }
            catch
            {
            }
        }
        
        #endregion
    }
}
