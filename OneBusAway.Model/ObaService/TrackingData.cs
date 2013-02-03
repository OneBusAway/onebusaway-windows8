using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OneBusAway.Model
{
    /// <summary>
    /// Data structure that represents a route and it's scheduled time vs predicted arrival time.
    /// </summary>
    public class TrackingData : BindableBase
    {
        public const string Favorites = "FAVORITES";
        public const string OneStop = "ONESTOP";

        private string routeId;
        private string tripId;
        private string stopName;
        private string stopId;
        private string tripHeadsign;
        private int scheduledArrivalInMinutes;
        private int predictedArrivalInMinutes;
        private DateTime scheduledArrivalTime;
        private DateTime predictedArrivalTime;
        private string status;
        private Route route;

        private string context;
        private string stopOrDestination;

        /// <summary>
        /// Creates the tracking data out of an arrival and departure element.
        /// </summary>
        public TrackingData(DateTime serverTime, string stopId, string stopName, XElement arrivalAndDepartureElement)
        {
            this.RouteId = arrivalAndDepartureElement.GetFirstElementValue<string>("routeId");
            this.TripId = arrivalAndDepartureElement.GetFirstElementValue<string>("tripId");
            this.TripHeadsign = arrivalAndDepartureElement.GetFirstElementValue<string>("tripHeadsign");
            this.StopName = stopName;
            this.StopId = stopId;
            this.Context = OneStop;

            scheduledArrivalTime = arrivalAndDepartureElement.GetFirstElementValue<long>("scheduledArrivalTime").ToDateTime();
            this.ScheduledArrivalInMinutes = (scheduledArrivalTime - serverTime).Minutes;

            long predictedTime = arrivalAndDepartureElement.GetFirstElementValue<long>("predictedArrivalTime");
            if (predictedTime > 0)
            {
                DateTime predictedArrivalDateTime = predictedTime.ToDateTime();
                this.PredictedArrivalInMinutes = (predictedArrivalDateTime - serverTime).Minutes;
            }
            else
            {
                this.PredictedArrivalInMinutes = this.scheduledArrivalInMinutes;
            }

            this.scheduledArrivalTime = DateTime.Now.AddMinutes(this.scheduledArrivalInMinutes);
            this.predictedArrivalTime = DateTime.Now.AddMinutes(this.predictedArrivalInMinutes);

            int difference = this.predictedArrivalInMinutes - this.scheduledArrivalInMinutes;
            if (difference > 0)
            {
                this.status = "DELAY";
            }
            else
            {
                this.status = "ON TIME";
            }

            // Grab the route element from the document and parse it into a Route object:
            var routeElements = (from routeElement in arrivalAndDepartureElement.Document.Descendants("route")
                                 where routeElement.GetFirstElementValue<string>("id") == this.RouteId
                                 select routeElement).ToList();

            this.Route = new Route(routeElements[0]);
        }

        public string Context
        {
            get
            {
                return this.context;
            }
            set
            {
                this.context = value;
                if (string.Equals(value, Favorites, StringComparison.OrdinalIgnoreCase))
                {
                    this.StopOrDestination = this.StopName;
                }
                else
                {
                    this.StopOrDestination = this.TripHeadsign;
                }
            }
        }

        public Route Route
        {
            get
            {
                return this.route;
            }
            set
            {
                SetProperty(ref this.route, value);
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

        public StopAndRoutePair StopAndRoute
        {
            get
            {
                return new StopAndRoutePair(this.stopId, this.routeId);
            }
        }

        public string TripId
        {
            get
            {
                return this.tripId;
            }
            set
            {
                SetProperty(ref this.tripId, value);
            }
        }

        public string TripHeadsign
        {
            get
            {
                return this.tripHeadsign;
            }
            set
            {
                SetProperty(ref this.tripHeadsign, value);
            }
        }

        public string StopOrDestination
        {
            get
            {
                return this.stopOrDestination;
            }
            set
            {
                SetProperty(ref this.stopOrDestination, value);
            }
        }

        public int ScheduledArrivalInMinutes
        {
            get
            {
                return this.scheduledArrivalInMinutes;
            }
            set
            {
                SetProperty(ref this.scheduledArrivalInMinutes, value);
            }
        }

        public int PredictedArrivalInMinutes
        {
            get
            {
                return this.predictedArrivalInMinutes;
            }
            set
            {
                SetProperty(ref this.predictedArrivalInMinutes, value);
            }
        }

        public DateTime ScheduledArrivalTime
        {
            get
            {
                return this.scheduledArrivalTime;
            }
            set
            {
                SetProperty(ref this.scheduledArrivalTime, value);
            }
        }

        public DateTime PredictedArrivalTime
        {
            get
            {
                return this.predictedArrivalTime;
            }
            set
            {
                SetProperty(ref this.predictedArrivalTime, value);
            }
        }

        public string Status
        {
            get
            {
                return this.status;
            }
            set
            {
                SetProperty(ref this.status, value);
            }
        }
    }
}
