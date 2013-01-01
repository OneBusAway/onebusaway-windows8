using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneBusAway.Model
{
    public class StopAndRoutePair
    {
        public string Stop
        {
            get;
            set;
        }

        public string Route
        {
            get;
            set;
        }

        public StopAndRoutePair(string stop, string route)
        {
            Stop = stop;
            Route = route;
        }
    }
}
