using OneBusAway.DataAccess;
using OneBusAway.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneBusAway.ViewModels
{
    public class RoutesAndStopsControlViewModel : ViewModelBase
    {
        private const string Favorites = "FAVORITES";
        private const string RealTime = "REALTIME";

        private ObaDataAccess obaDataAccess;
        private TrackingData[] realTimeData;
        private RouteMapsAndSchedulesControlViewModel[] routeAndMapsViewModels;
        private string stopId;
        private string stopHeaderText;
        private string stopSubHeaderText;
        private string stopOrDestinationText;
        private bool showNoFavoritesMessage;
        private bool showNoItemsMessage;
        private DateTime lastUpdated;
        private string filteredRouteId;
        private bool isFiltered;

        public RoutesAndStopsControlViewModel()
        {
            this.obaDataAccess = new ObaDataAccess();
            this.StopHeaderText = Favorites;
            this.StopSubHeaderText = RealTime;
            this.LastUpdated = DateTime.Now;
        }

        [SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Justification="Enables binding to xaml")]
        public TrackingData[] RealTimeData
        {
            get
            {
                if (this.isFiltered)
                {
                    return (from trackingData in this.realTimeData
                            where string.Equals(trackingData.Route.Id, this.filteredRouteId, StringComparison.OrdinalIgnoreCase)
                            select trackingData).ToArray();
                }
                else
                {
                    return this.realTimeData;
                }
            }
            set
            {
                SetProperty(ref this.realTimeData, value);
            }
        }

        /// <summary>
        /// Returns the distinct routes from the real time data.
        /// </summary>
        public RouteMapsAndSchedulesControlViewModel[] RouteAndMapsViewModels
        {
            get
            {
                return this.routeAndMapsViewModels;
            }
            set
            {
                SetProperty(ref this.routeAndMapsViewModels, value);
            }
        }

        public bool ShowNoFavoritesMessage
        {
            get
            {
                return this.showNoFavoritesMessage;
            }
            set
            {
                SetProperty(ref this.showNoFavoritesMessage, value);
            }
        }

        public bool ShowNoItemsMessage
        {

            get
            {
                return this.showNoItemsMessage;
            }
            set
            {
                SetProperty(ref this.showNoItemsMessage, value);
            }
        }

        public string StopOrDestinationText
        {
            get
            {
                return this.stopOrDestinationText;
            }
            set
            {
                SetProperty(ref this.stopOrDestinationText, value);
            }
        }

        public string StopId
        {
            get
            {
                return this.stopId;
            }
            set
            {
                SetProperty(ref this.stopId, value);
            }
        }

        public string StopHeaderText
        {
            get
            {
                return this.stopHeaderText;
            }
            set
            {
                SetProperty(ref this.stopHeaderText, value);

                if (string.Equals(this.stopHeaderText, Favorites, StringComparison.OrdinalIgnoreCase))
                {
                    this.StopOrDestinationText = "STOP";
                }
                else
                {
                    this.StopOrDestinationText = "DESTINATION";
                }
            }
        }

        public string StopSubHeaderText
        {
            get
            {
                return this.stopSubHeaderText;
            }
            set
            {
                SetProperty(ref this.stopSubHeaderText, value);
            }
        }

        public DateTime LastUpdated
        {
            get
            {
                return this.lastUpdated;
            }
            set
            {
                SetProperty(ref this.lastUpdated, value);
            }
        }

        public async Task PopulateFavoritesAsync()
        {
            List<TrackingData> trackingData = new List<TrackingData>();

            var favs = Model.Favorites.Get();

            this.StopHeaderText = Favorites;
            this.StopSubHeaderText = RealTime;
            
            foreach (StopAndRoutePair fav in favs)
            {
                TrackingData[] tdataArray = await obaDataAccess.GetTrackingDataForStopAsync(fav.Stop);

                foreach (TrackingData tdata in tdataArray)
                {
                    if (string.Equals(fav.Route, tdata.RouteId, StringComparison.OrdinalIgnoreCase))
                    {
                        tdata.Context = TrackingData.Favorites;
                        trackingData.Add(tdata);
                        break;
                    }
                }
            }

            this.RealTimeData = trackingData.ToArray();
            this.RouteAndMapsViewModels = null;
            this.LastUpdated = DateTime.Now;
            this.ShowNoFavoritesMessage = trackingData.Count == 0;
        }

        /// <summary>
        /// Populates a stop asyncrhonously.
        /// </summary>
        public async Task PopulateStopAsync(string stopName, string stopId, string direction)
        {
            this.StopId = stopId;            
            this.StopHeaderText = stopName;
            this.StopSubHeaderText = string.Format(CultureInfo.CurrentCulture, "{0} BOUND", direction);

            await this.RefreshStopAsync();
        }

        /// <summary>
        /// Refreshes the currently displayed bus stop.
        /// </summary>
        public async Task RefreshStopAsync()
        {
            if (!string.IsNullOrEmpty(this.StopId))
            {
                this.RealTimeData = await obaDataAccess.GetTrackingDataForStopAsync(this.StopId);

                this.RouteAndMapsViewModels = (from route in await obaDataAccess.GetRoutesForStopAsync(this.StopId)
                                               select new RouteMapsAndSchedulesControlViewModel()
                                               {
                                                   StopId = this.stopId,
                                                   RouteId = route.Id,
                                                   RouteName = route.ShortName,
                                                   StopName = this.StopHeaderText
                                               }).ToArray();

                this.LastUpdated = DateTime.Now;
                this.ShowNoItemsMessage = this.RealTimeData.Length == 0;
            }
        }

        /// <summary>
        /// Toggles filtering by a specific route.
        /// </summary>
        public void ToggleFilterByRouteAsync(Route route)
        {
            if (this.isFiltered)
            {
                this.filteredRouteId = null;
                this.isFiltered = false;
            }
            else
            {
                this.filteredRouteId = route.Id;
                this.isFiltered = true;
            }

            FirePropertyChanged("RealTimeData");
            FirePropertyChanged("DistinctRoutes");
        }
    }
}
