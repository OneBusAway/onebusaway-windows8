using OneBusAway.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneBusAway.ViewModels
{
    public class StopSelectedEventArgs : EventArgs
    {
        public StopSelectedEventArgs(string stopName, string selectedStopId, string direction, double latitude, double longitude)
        {
            this.StopName = stopName;
            this.SelectedStopId = selectedStopId;
            this.Direction = direction;
            this.Latitude = latitude;
            this.Longitude = longitude;
        }

        public double Latitude
        {
            get;
            private set;
        }

        public double Longitude
        {
            get;
            private set;
        }

        public string StopName
        {
            get;
            private set;
        }

        public string SelectedStopId
        {
            get;
            private set;
        }

        public string Direction
        {
            get;
            private set;
        }
    }
}
