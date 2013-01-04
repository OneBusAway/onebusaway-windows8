using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneBusAway.ViewModels
{
    /// <summary>
    /// Event args for when a route is selected.
    /// </summary>
    public class RouteSelectedEventArgs : EventArgs
    {
        /// <summary>
        /// The route that was selected.
        /// </summary>
        public RouteSelectedEventArgs(string routeId)
        {
            this.RouteId = routeId;
        }

        public string RouteId
        {
            get;
            private set;
        }

    }
}
