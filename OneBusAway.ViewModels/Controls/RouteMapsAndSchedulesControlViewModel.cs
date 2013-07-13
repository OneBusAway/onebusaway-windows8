<<<<<<< HEAD:OneBusAway.ViewModels/RouteMapsAndSchedulesControlViewModel.cs
﻿using OneBusAway.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneBusAway.ViewModels
{
    /// <summary>
    /// View model for the route maps and schedules control.
    /// </summary>
    public class RouteMapsAndSchedulesControlViewModel : BindableBase
    {
        private string stopId;
        private string routeId;
        private string stopName;
        private string routeName;
        private string routeDescription;    

        /// <summary>
        /// Creates the control.
        /// </summary>
        public RouteMapsAndSchedulesControlViewModel()
        {
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

        public string RouteDescription
        {
            get
            {
                return this.routeDescription;
            }
            set
            {
                SetProperty(ref this.routeDescription, value);
            }
        }

        public string RouteName
        {
            get
            {
                return this.routeName;
            }
            set
            {
                SetProperty(ref this.routeName, value);
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

        public string RouteId
        {
            get
            {
                return this.routeId;
            }
            set
            {
                SetProperty(ref this.routeId, value);
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
    /// View model for the route maps and schedules control.
    /// </summary>
    public class RouteMapsAndSchedulesControlViewModel : BindableBase
    {
        private string stopId;
        private string routeId;
        private string stopName;
        private string routeName;
        private string routeDescription;    

        /// <summary>
        /// Creates the control.
        /// </summary>
        public RouteMapsAndSchedulesControlViewModel()
        {
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

        public string RouteDescription
        {
            get
            {
                return this.routeDescription;
            }
            set
            {
                SetProperty(ref this.routeDescription, value);
            }
        }

        public string RouteName
        {
            get
            {
                return this.routeName;
            }
            set
            {
                SetProperty(ref this.routeName, value);
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

        public string RouteId
        {
            get
            {
                return this.routeId;
            }
            set
            {
                SetProperty(ref this.routeId, value);
            }
        }        
    }
}
>>>>>>> 5fa5eaa... Re-arranging the view models by moving page controls & user control view models into sudifferent namepsaces / folders.:OneBusAway.ViewModels/Controls/RouteMapsAndSchedulesControlViewModel.cs
