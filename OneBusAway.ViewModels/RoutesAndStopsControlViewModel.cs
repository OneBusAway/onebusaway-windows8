using OneBusAway.DataAccess;
using OneBusAway.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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

        public async Task PopulateFavoritesAsync(List<Favorite> favs)
        {
            List<TrackingData> trackingData = new List<TrackingData>();

            // FIXME: These two lines should be removed
            // They are here to add a fake favorite for now.
            favs = new List<Favorite>();
            favs.Add(new Favorite("1_75403", "1_67"));

            this.StopHeaderText = Favorites;
            this.StopSubHeaderText = RealTime;

            foreach (Favorite fav in favs)
            {
                TrackingData[] tdataArray = await obaDataAccess.GetTrackingDataForStopAsync(fav.Stop);

                foreach (TrackingData tdata in tdataArray)
                {
                    if (string.Equals(fav.Route, tdata.RouteId, StringComparison.OrdinalIgnoreCase))
                    {
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
            this.StopSubHeaderText = string.Format("{0} BOUND", direction);
            this.RealTimeData = await obaDataAccess.GetTrackingDataForStopAsync(stopId);
            this.LastUpdated = DateTime.Now;
        }
    }
}
