using OneBusAway.DataAccess.BingService;
using OneBusAway.Model.BingService;
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
        Windows.Devices.Geolocation.Geolocator geolocator = new Windows.Devices.Geolocation.Geolocator();
        Windows.Devices.Geolocation.Geoposition userPosition;

        RoutesAndStopsControlViewModel routesAndStopsViewModel;

        public MainPageViewModel()
        {
            geolocator.PositionChanged += geolocator_PositionChanged;
            this.HeaderViewModel.FavoritesIsEnabled = false;
            this.RoutesAndStopsViewModel = new RoutesAndStopsControlViewModel();
        }

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

        public async void Load()
        {
            await this.RoutesAndStopsViewModel.PopulateAsync();
        }

        void geolocator_PositionChanged(Windows.Devices.Geolocation.Geolocator sender, Windows.Devices.Geolocation.PositionChangedEventArgs args)
        {
            userPosition = args.Position;
        }
    }
}
