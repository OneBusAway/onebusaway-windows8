using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneBusAway.ViewModels
{
    /// <summary>
    /// View Model for the bus stop control.
    /// </summary>
    public class BusStopControlViewModel : ViewModelBase
    {
        bool isSelected;
        private string stopId;
        private string direction;
        private string stopName;
        private MapControlViewModel mapControlViewModel;

        /// <summary>
        /// Creates the view model.
        /// </summary>
        public BusStopControlViewModel(MapControlViewModel mapControlViewModel)
        {
            this.MapControlViewModel = mapControlViewModel;            
        }

        /// <summary>
        /// Returns the owner map control view model.
        /// </summary>
        public MapControlViewModel MapControlViewModel
        {
            get
            {
                return this.mapControlViewModel;
            }
            set
            {
                SetProperty(ref this.mapControlViewModel, value);
            }
        }

        public bool IsSelected
        {
            get
            {
                return this.isSelected;
            }
            set
            {
                SetProperty(ref this.isSelected, value);
            }
        }

        public string StopId
        {
            get
            {
                return stopId;
            }
            set
            {
                SetProperty(ref this.stopId, value);
            }
        }

        public string Direction
        {
            get
            {
                return direction;
            }
            set
            {
                SetProperty(ref this.direction, value);
            }
        }

        public string StopName
        {
            get
            {
                return stopName;
            }
            set
            {
                SetProperty(ref this.stopName, value);
            }
        }
    }
}
