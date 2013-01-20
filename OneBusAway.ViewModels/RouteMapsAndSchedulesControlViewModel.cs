using OneBusAway.Model;
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
