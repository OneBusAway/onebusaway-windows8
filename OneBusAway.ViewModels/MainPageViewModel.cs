using OneBusAway.DataAccess.BingService;
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

        public async void Load()
        {
            await this.RoutesAndStopsViewModel.PopulateAsync();
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

        #region Public Properties

        public string BingMapCredentials
        {
            get
        {
                return Constants.BingMapCredentials;               
            }
        }
        
        #endregion
    }
}
