using OneBusAway.DataAccess;
using OneBusAway.Model;
using System;
using System.Collections.Generic;
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

        public async Task PopulateAsync()
        {
            this.RealTimeData = await obaDataAccess.GetTrackingDataForStopAsync("1_75403");
        }

    }
}
