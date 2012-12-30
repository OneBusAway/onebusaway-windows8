using OneBusAway.Model;
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
        private double latitude;
        private double longitude;
        private MapControlViewModel mapControlViewModel;

        /// <summary>
        /// Creates the view model.
        /// </summary>
        public BusStopControlViewModel(MapControlViewModel mapControlViewModel, Stop stop)
        {
            this.MapControlViewModel = mapControlViewModel;
            this.StopId = stop.StopId;
            this.StopName = stop.Name;
            this.Latitude = stop.Latitude;
            this.Longitude = stop.Longitude;
            this.Direction = stop.Direction;
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

        public double Latitude
        {
            get
            {
                return this.latitude;
            }
            set
            {
                SetProperty(ref this.latitude, value);
            }
        }

        public double Longitude
        {
            get
            {
                return this.longitude;
            }
            set
            {
                SetProperty(ref this.longitude, value);
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
                return this.stopId;
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
                return this.direction;
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
                return this.stopName;
            }
            set
            {
                SetProperty(ref this.stopName, value);
            }
        }
    }
}
