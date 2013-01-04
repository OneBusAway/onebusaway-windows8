using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Globalization;
using System.Xml.Linq;
using OneBusAway.Model;
using OneBusAway.Utilities;

namespace OneBusAway.DataAccess
{
    /// <summary>
    /// Class talks to the OneBusAway server and grabs data from it.
    /// </summary>
    public class ObaDataAccess
    {
        /// <summary>
        /// Creates the data access.
        /// </summary>
        public ObaDataAccess()
        {
            this.Factory = new ObaServiceHelperFactory(UtilitiesConstants.SERVER_URL);
        }

        /// <summary>
        /// The service factory allows us to create web requests.
        /// </summary>
        public ObaServiceHelperFactory Factory
        {
            get;
            set;
        }

        /// <summary>
        /// Gets stops near a location
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public async Task<Stop[]> GetStopsForLocationAsync(double latitude, double longitude)
        {
            return await this.GetStopsForLocationAsync(latitude, longitude, 0.0, 0.0, 0.0);
        }

        public async Task<Stop[]> GetStopsForLocationAsync(double latitude, double longitude, double radius)
        {
            return await this.GetStopsForLocationAsync(latitude, longitude, radius, 0.0, 0.0);
        }

        public async Task<Stop[]> GetStopsForLocationAsync(double latitude, double longitude, double latitudeSpan, double longitudeSpan)
        {
            return await this.GetStopsForLocationAsync(latitude, longitude, 0.0, latitudeSpan, longitudeSpan);
        }

        /// <summary>
        /// Gets the stops near a location.
        /// </summary>
        private async Task<Stop[]> GetStopsForLocationAsync(double latitude, double longitude, double radius, double latitudeSpan, double longitudeSpan)
        {
            var helper = this.Factory.CreateHelper(ObaMethod.stops_for_location);
            helper.AddToQueryString("lat", latitude.ToString(CultureInfo.CurrentCulture));
            helper.AddToQueryString("lon", longitude.ToString(CultureInfo.CurrentCulture));

            if (radius > 0)
            {
                helper.AddToQueryString("radius", radius.ToString(CultureInfo.CurrentCulture));
            }
            else if (latitudeSpan > 0 && longitude > 0)
            {
                helper.AddToQueryString("latSpan", latitudeSpan.ToString(CultureInfo.CurrentCulture));
                helper.AddToQueryString("lonSpan", longitudeSpan.ToString(CultureInfo.CurrentCulture));
            }

            XDocument doc = await helper.SendAndRecieveAsync();
            return (from stopElement in doc.Descendants("stop")
                    select new Stop(stopElement)).ToArray();
        }

        /// <summary>
        /// Returns all of the agencies that OBA serves.
        /// </summary>
        public async Task<Agency[]> GetAllAgencies()
        {
            var helper = this.Factory.CreateHelper(ObaMethod.agencies_with_coverage);

            XDocument doc = await helper.SendAndRecieveAsync();
            return (from agencyWithCoverageElement in doc.Descendants("agency")
                    select new Agency(agencyWithCoverageElement)).ToArray();
        }

        /// <summary>
        /// Returns all of the route Ids for a particular agency.
        /// </summary>
        public async Task<Route[]> GetAllRouteIdsForAgency(Agency agency)
        {
            var helper = this.Factory.CreateHelper(ObaMethod.routes_for_agency);
            helper.SetId(agency.Id);

            XDocument doc = await helper.SendAndRecieveAsync();
            return (from routeElement in doc.Descendants("route")
                    select new Route(routeElement)
                    {
                        Agency = agency
                    }).ToArray();
        }

        /// <summary>
        /// Returns the routes for a particular stop.
        /// </summary>
        public async Task<Route[]> GetRoutesForStopAsync(string stopId)
        {
            var helper = this.Factory.CreateHelper(ObaMethod.stop);
            helper.SetId(stopId);

            XDocument doc = await helper.SendAndRecieveAsync();
            return (from routeElement in doc.Descendants("route")
                    select new Route(routeElement)).ToArray();
        }

        /// <summary>
        /// Returns the stops for a particular route.
        /// </summary>
        public async Task<Stop[]> GetStopsForRouteAsync(string routeId)
        {
            var helper = this.Factory.CreateHelper(ObaMethod.stops_for_route);
            helper.SetId(routeId);

            XDocument doc = await helper.SendAndRecieveAsync();

            // Find all of the stops in the payload that have this route id:
            return (from stopElement in doc.Descendants("stop")
                    select new Stop(stopElement)).ToArray();
        }

        /// <summary>
        /// Returns the schedule for a particular stop / route combination.
        /// </summary>
        public async Task<StopRouteSchedule> GetScheduleForStopAndRoute(string stopId, string routeId)
        {
            var helper = this.Factory.CreateHelper(ObaMethod.schedule_for_stop);
            helper.SetId(stopId);

            XDocument doc = await helper.SendAndRecieveAsync();

            // Find all of the stops in the payload that have this route id:

            var stopRouteScheduleElement = doc.Descendants("stopRouteSchedule")
                .Where(xe => string.Equals(xe.GetFirstElementValue<string>("routeId"), routeId, StringComparison.OrdinalIgnoreCase))
                .FirstOrDefault();

            if (stopRouteScheduleElement == null)
            {
                throw new ArgumentException(string.Format("Unknown route / stop combination {0}/{1}", routeId, stopId));
            }

            DateTime serverTime = doc.Root.GetFirstElementValue<long>("currentTime").ToDateTime();
            return new StopRouteSchedule(serverTime, stopRouteScheduleElement.Descendants("scheduleStopTimes").First());
        }

        /// <summary>
        /// Returns route data for a particular route. Since there are two directions for each route,
        /// this method returns an array of routes - one for each direction (trip headsign).
        /// </summary>
        public async Task<RouteData[]> GetRouteDataAsync(string routeId)
        {
            var helper = this.Factory.CreateHelper(ObaMethod.stops_for_route);
            helper.SetId(routeId);

            XDocument doc = await helper.SendAndRecieveAsync();
            XElement dataElement = doc.Descendants("data").First();

            string []tripHeadsigns = (from stopGroupsElement in dataElement.Descendants("stopGroup")
                                      let nameElement = stopGroupsElement.Descendants("names").First()
                                      select nameElement.Descendants("string").First().Value).ToArray();

            return (from tripHeadsign in tripHeadsigns
                    select new RouteData(dataElement, tripHeadsign)).ToArray();
        }

        /// <summary>
        /// Returns the route data for a route.
        /// </summary>
        public async Task<RouteData> GetRouteDataAsync(string routeId, string tripHeadsign)
        {
            var helper = this.Factory.CreateHelper(ObaMethod.stops_for_route);
            helper.SetId(routeId);
            
            XDocument doc = await helper.SendAndRecieveAsync();
            XElement dataElement = doc.Descendants("data").First();
            return new RouteData(dataElement, tripHeadsign);            
        }

        /// <summary>
        /// Returns real time tracking data for the buses at a particular stop.
        /// </summary>
        public async Task<TrackingData[]> GetTrackingDataForStopAsync(string stopId)
        {
            var helper = this.Factory.CreateHelper(ObaMethod.arrivals_and_departures_for_stop);
            helper.SetId(stopId);

            XDocument doc = await helper.SendAndRecieveAsync();
            DateTime serverTime = doc.Root.GetFirstElementValue<long>("currentTime").ToDateTime();

            string stopName = doc.Descendants("stop").First().GetFirstElementValue<string>("name");

            // Find all of the stops in the payload that have this route id:
            return (from arrivalAndDepartureElement in doc.Descendants("arrivalAndDeparture")
                    select new TrackingData(serverTime, stopId, stopName, arrivalAndDepartureElement)).ToArray();
        }

        /// <summary>
        /// Gets the trip data for a specific trip.
        /// </summary>
        public async Task<TripDetails> GetTripDetailsAsync(string tripId)
        {
            var helper = this.Factory.CreateHelper(ObaMethod.trip_details);
            helper.SetId(tripId);

            XDocument doc = await helper.SendAndRecieveAsync();
            DateTime serverTime = doc.Root.GetFirstElementValue<long>("currentTime").ToDateTime();

            XElement entryElement = doc.Descendants("entry").First();
            return new TripDetails(entryElement, serverTime);
        }
    }
}
