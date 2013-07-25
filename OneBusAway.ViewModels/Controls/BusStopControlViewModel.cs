using OneBusAway.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneBusAway.ViewModels.Controls
{
    /// <summary>
    /// View Model for the bus stop control.
    /// </summary>
    public class BusStopControlViewModel : ViewModelBase
    {
        private Stop stop;
        private bool isSelected;
        private bool isClosestStop;        
        private string stopId;
        private string direction;
        private string stopName;
        private double latitude;
        private double longitude;
        private double zIndex;

        /// <summary>
        /// Creates the view model.
        /// </summary>
        public BusStopControlViewModel(Stop stop)
        {
            this.StopId = stop.StopId;
            this.StopName = stop.Name;            
            this.Latitude = stop.Latitude;
            this.Longitude = stop.Longitude;
            this.Direction = stop.Direction;
            this.IsClosestStop = stop.IsClosestStop;

            this.stop = stop;
            this.stop.PropertyChanged += OnStopPropertyChanged;
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

        public double ZIndex
        {
            get
            {
                return this.zIndex;
            }
            set
            {
                SetProperty(ref this.zIndex, value);
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

        public bool IsClosestStop
        {
            get
            {
                return this.isClosestStop;
            }
            set
            {
                SetProperty(ref this.isClosestStop, value);
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

        /// <summary>
        /// Called when the stop's property changed hander is invoked. If the IsClosestStop 
        /// value changes, we need to update our value as well.
        /// </summary>
        private void OnStopPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (string.Equals("IsClosestStop", e.PropertyName, StringComparison.OrdinalIgnoreCase))
            {
                this.IsClosestStop = this.stop.IsClosestStop;
            }
        }
    }
}