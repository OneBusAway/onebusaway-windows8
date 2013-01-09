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
        private string stopHeaderText;
        private string stopSubHeaderText;
        private string stopOrDestinationText;
        private DateTime lastUpdated;

        public RoutesAndStopsControlViewModel()
        {
            this.obaDataAccess = new ObaDataAccess();
            this.StopHeaderText = Favorites;
            this.StopSubHeaderText = RealTime;
        }

        [SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Justification="Enables binding to xaml")]
        public TrackingData[] RealTimeData
        {
            get
            {
                return this.realTimeData;
            }
            set
            {
                SetProperty(ref this.realTimeData, value);
                FirePropertyChanged("DistinctRoutes");
            }
        }

        /// <summary>
        /// Returns the distinct routes from the real time data.
        /// </summary>
        public TrackingData[] DistinctRoutes
        {
            get
            {
                if (this.realTimeData == null)
                {
                    return new TrackingData[] { };
                }

                // Find all of the unique routes and order them by the ones that are predicted to come sooner:
                var query = from trackingData in this.realTimeData
                            group trackingData by trackingData.Route.Id into groupedRoutes
                            select groupedRoutes.OrderBy(gr => gr.PredictedArrivalTime).First();

                return query.ToArray();
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
                FirePropertyChanged("LastUpdatedText");
            }
        }

        /// <summary>
        /// Includes "LAST UPDATED" text.
        /// </summary>
        public string LastUpdatedText
        {
            get
            {
                return string.Format(CultureInfo.CurrentCulture, "LAST UPDATED: {0}", this.LastUpdated.ToString("h:mm tt"));
            }
        }

        public async Task PopulateFavoritesAsync()
        {
            List<TrackingData> trackingData = new List<TrackingData>();

            //  TO DO: Load favorites from some storage location...somewhere.
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
            this.LastUpdated = DateTime.Now;
        }

        public async Task PopulateStopAsync(string stopName, string stopId, string direction)
        {
            this.StopHeaderText = stopName;
            this.StopSubHeaderText = string.Format(CultureInfo.CurrentCulture, "{0} BOUND", direction);

            this.RealTimeData = await obaDataAccess.GetTrackingDataForStopAsync(stopId);
            this.LastUpdated = DateTime.Now;
        }
    }
}
