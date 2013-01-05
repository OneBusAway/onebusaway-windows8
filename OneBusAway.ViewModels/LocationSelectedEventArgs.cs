using OneBusAway.Model.BingService;
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
    public class LocationSelectedEventArgs : EventArgs
    {
        /// <summary>
        /// The route that was selected.
        /// </summary>
        public LocationSelectedEventArgs(Location _location)
        {
            this.Location = _location;
        }

        public Location Location
        {
            get;
            private set;
        }

    }
}
