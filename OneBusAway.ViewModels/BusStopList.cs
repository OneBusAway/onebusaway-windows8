using OneBusAway.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneBusAway.ViewModels
{
    /// <summary>
    /// A list of stops with one extra parameter.
    /// </summary>
    public class BusStopList : List<Stop>
    {
        /// <summary>
        /// Creates the list.
        /// </summary>
        public BusStopList()
            : base()
        {
        }

        public BusStopList(IEnumerable<Stop> collection)
            : base(collection)
        {
        }

        /// <summary>
        /// When true, the map control will clear existing bus stops.
        /// </summary>
        public bool ClearExistingStops
        {
            get;
            set;
        }
    }
}
