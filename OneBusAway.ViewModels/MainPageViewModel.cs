using OneBusAway.DataAccess;
using OneBusAway.DataAccess.BingService;
using OneBusAway.Model;
using OneBusAway.Model.BingService;
using OneBusAway.Utilities;
using System;
using System.Collections.Generic;
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

        public MainPageViewModel()
        {
            this.HeaderViewModel.FavoritesIsEnabled = false;

            this.RoutesAndStopsViewModel = new RoutesAndStopsControlViewModel();

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


        public string BingMapCredentials
        {
            get
        {
                return Constants.BingMapCredentials;               
            }
        }
        
        #endregion

        #region Public Methods

        public async void Load()
        {
            await this.RoutesAndStopsViewModel.PopulateAsync();
        }

        public async Task<Stop[]> GetStopsForLocation(double latitude, double longitude)
        {
            return await new ObaDataAccess().GetStopsForLocationAsync(latitude, longitude, 0.0);
        }

        public async Task<Stop[]> GetStopsForLocation(double latitude, double longitude, double radius)
        {
            return await new ObaDataAccess().GetStopsForLocationAsync(latitude, longitude, radius);
        }

        public async Task<Stop[]> GetStopsForLocation(double latitude, double longitude, double latitudeSpan, double longitudeSpan)
        {
            return await new ObaDataAccess().GetStopsForLocationAsync(latitude, longitude, latitudeSpan, longitudeSpan);
        }
        
        #endregion
    }
}
