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
        public StopSelectedEventArgs(string selectedStopId)
        {
            this.SelectedStopId = selectedStopId;
        }

        public string SelectedStopId
        {
            get;
            private set;
        }
    }
}
