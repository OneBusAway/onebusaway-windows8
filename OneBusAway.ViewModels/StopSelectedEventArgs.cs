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
        public StopSelectedEventArgs(string stopName, string selectedStopId, string direction)
        {
            this.StopName = stopName;
            this.SelectedStopId = selectedStopId;
            this.Direction = direction;
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
