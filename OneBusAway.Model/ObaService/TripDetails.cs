using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OneBusAway.Model
{
    /// <summary>
    /// Model for trip details.
    /// </summary>
    public class TripDetails : BindableBase
    {
        private double knownLatitude;
        private double knownLongitude;
        private string vehicleId;
        private string tripId;
        private string closestStopId;
        private string nextStopId;

        /// <summary>
        /// Creates the trip details.
        /// </summary>
        public TripDetails(XElement entryElement)
        {
            this.TripId = entryElement.GetFirstElementValue<string>("tripId");
            this.VehicleId = entryElement.GetFirstElementValue<string>("vehicleId");
            this.ClosestStopId = entryElement.GetFirstElementValue<string>("closestStop");
            this.NextStopId = entryElement.GetFirstElementValue<string>("nextStop");

            var lastKnownLocationElement = entryElement.Descendants("lastKnownLocation").First();
            this.KnownLatitude = lastKnownLocationElement.GetFirstElementValue<double>("lat");
            this.KnownLongitude = lastKnownLocationElement.GetFirstElementValue<double>("lon");

            //<entry class="tripDetails">
            //    <tripId>40_21580184</tripId>
            //    <serviceDate>1357113600000</serviceDate>
            //    <status>
            //    <activeTripId>40_21580184</activeTripId>
            //    <blockTripSequence>0</blockTripSequence>
            //    <serviceDate>1357113600000</serviceDate>
            //    <scheduledDistanceAlongTrip>4200.542521894713</scheduledDistanceAlongTrip>
            //    <totalDistanceAlongTrip>31368.062697873433</totalDistanceAlongTrip>
            //    <position>
            //        <lat>47.62131784992367</lat>
            //        <lon>-122.32801704050095</lon>
            //    </position>
            //    <orientation>100.26638970466908</orientation>
            //    <closestStop>1_1070</closestStop>
            //    <closestStopTimeOffset>-98</closestStopTimeOffset>
            //    <nextStop>1_71350</nextStop>
            //    <nextStopTimeOffset>434</nextStopTimeOffset>
            //    <status>default</status>
            //    <predicted>true</predicted>
            //    <lastUpdateTime>1357166368000</lastUpdateTime>
            //    <lastKnownLocation>
            //        <lat>47.61576461791992</lat>
            //        <lon>-122.33000946044922</lon>
            //    </lastKnownLocation>
            //    <scheduleDeviation>660</scheduleDeviation>
            //    <distanceAlongTrip>4200.542521894713</distanceAlongTrip>
            //    <vehicleId>40_9616</vehicleId>
            //    </status>
            //</entry>
        }

        public double KnownLatitude
        {
            get
            {
                return this.knownLatitude;
            }
            set
            {
                SetProperty(ref this.knownLatitude, value);
            }
        }

        public double KnownLongitude
        {
            get
            {
                return this.knownLongitude;
            }
            set
            {
                SetProperty(ref this.knownLongitude, value);
            }
        }

        public string VehicleId
        {
            get
            {
                return this.vehicleId;
            }
            set
            {
                SetProperty(ref this.vehicleId, value);
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

        public string ClosestStopId
        {
            get
            {
                return this.closestStopId;
            }
            set
            {
                SetProperty(ref this.closestStopId, value);
            }
        }

        public string NextStopId
        {
            get
            {
                return this.nextStopId;
            }
            set
            {
                SetProperty(ref this.nextStopId, value);
            }
        }
    }
}
