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
        private ObaDataAccess obaDataAccess;
        private TrackingData[] realTimeData;

        public RoutesAndStopsControlViewModel()
        {
            this.obaDataAccess = new ObaDataAccess();
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

        public async Task PopulateFavoritesAsync(List<Favorite> favs)
        {
            List<TrackingData> trackingData = new List<TrackingData>();

            // FIXME: These two lines should be removed
            // They are here to add a fake favorite for now.
            favs = new List<Favorite>();
            favs.Add(new Favorite("1_75403", "1_67"));

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
        }

        public async Task PopulateStopAsync(string stopId)
        {
            this.RealTimeData = await obaDataAccess.GetTrackingDataForStopAsync(stopId);
        }
    }
}
