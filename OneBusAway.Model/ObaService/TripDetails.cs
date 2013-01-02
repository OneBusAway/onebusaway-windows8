using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OneBusAway.Model.ObaService
{
    /// <summary>
    /// Model for trip details.
    /// </summary>
    public class TripDetails : BindableBase
    {
        /// <summary>
        /// Creates the trip details.
        /// </summary>
        public TripDetails(XElement statusElement)
        {
        }

    }
}
