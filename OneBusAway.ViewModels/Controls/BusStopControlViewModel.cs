<<<<<<< HEAD:OneBusAway.ViewModels/BusStopControlViewModel.cs
﻿using OneBusAway.Model;
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
=======
﻿using OneBusAway.Model;
using System;
using System.Collections.Generic;
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
        bool isSelected;
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
>>>>>>> 5fa5eaa... Re-arranging the view models by moving page controls & user control view models into sudifferent namepsaces / folders.:OneBusAway.ViewModels/Controls/BusStopControlViewModel.cs
