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

        public string StopName
        {
            get;
            set;
        }

        public string Route
        {
            get;
            set;
        }

        public string RouteName
        {
            get;
            set;
        }

        public StopAndRoutePair()
        {
        }

        public StopAndRoutePair(TrackingData trackingData)
        {
            this.Stop = trackingData.StopId;
            this.StopName = trackingData.StopName;
            this.Route = trackingData.RouteId;
            this.RouteName = trackingData.Route.ShortName;
        }

        public static bool operator ==(StopAndRoutePair a, StopAndRoutePair b)
        {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            return a.Equals(b);
        }

        public static bool operator !=(StopAndRoutePair a, StopAndRoutePair b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            return Equals(obj as StopAndRoutePair);
        }

        public bool Equals(StopAndRoutePair pair)
        {
            if ((object)pair == null)
            {
                return false;
            }

            return string.Equals(Stop, pair.Stop, StringComparison.OrdinalIgnoreCase)
                    && string.Equals(Route, pair.Route, StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode()
        {
            return Stop.GetHashCode() ^ Route.GetHashCode();
        }
    }
}
