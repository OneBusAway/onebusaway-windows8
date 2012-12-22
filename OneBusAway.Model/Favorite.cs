using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneBusAway.Model
{
    public class Favorite
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

        public Favorite(string stop, string route)
        {
            Stop = stop;
            Route = route;
        }
    }
}
