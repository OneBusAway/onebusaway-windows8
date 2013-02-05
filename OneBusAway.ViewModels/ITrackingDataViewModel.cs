using OneBusAway.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneBusAway.ViewModels
{
    /// <summary>
    /// View model that supports tracking data.
    /// </summary>
    public interface ITrackingDataViewModel
    {
        TrackingData[] RealTimeData
        {
            get;
            set;
        }
    }
}
