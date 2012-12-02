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
        private List<Stop> busStops = new List<Stop>();
        private OneBusAway.Model.Point mapCenter;
        private double zoomLevel = Constants.DefaultMapZoom;
        private OneBusAway.Model.Point userLocation;

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

        public List<Stop> BusStops
        {
            get
            {
                return busStops;
            }
            set
            {
                SetProperty(ref this.busStops, value);
            }
        }

        public OneBusAway.Model.Point MapCenter
        {
            get
            {
                return mapCenter;
            }
            set
            {
                SetProperty(ref this.mapCenter, value);                
            }
        }

        public double ZoomLevel
        {
            get
            {
                return zoomLevel;
            }
            set
            {
                SetProperty(ref this.zoomLevel, value);
            }
        }

        public OneBusAway.Model.Point UserLocation
        {
            get
            {
                return userLocation;
            }
            set
            {
                SetProperty(ref this.userLocation, value);
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
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        public async void RefreshStopsForLocationAsync(double latitude, double longitude, double latitudeSpan, double longitudeSpan)
        {
            try
            {
                var result = await new ObaDataAccess().GetStopsForLocationAsync(latitude, longitude, latitudeSpan, longitudeSpan);

                BusStops = result.ToList();
            }
            catch
            {
            }
        }
        
        #endregion
    }
}
