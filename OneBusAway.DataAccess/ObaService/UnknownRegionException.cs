using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneBusAway.DataAccess.ObaService
{
    /// <summary>
    /// Thrown when the user is inside an unknown region.
    /// </summary>
    public class UnknownRegionException : Exception
    {
        public UnknownRegionException()
        {
        }
    }
}
