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
        /// Returns the shape of a route for a particular route.
        /// </summary>
        public async Task<Shape> GetShapeForRouteAsync(string routeId)
        {
            var helper = this.Factory.CreateHelper(ObaMethod.shape);
            helper.SetId(routeId);

            XDocument doc = await helper.SendAndRecieveAsync();

            // Find all of the stops in the payload that have this route id:
            return (from entryElement in doc.Descendants("entry")
                    select new Shape(entryElement)).First();
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
                    select new TrackingData(serverTime, stopName, arrivalAndDepartureElement)).ToArray();
        }
    }
}
