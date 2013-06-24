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
using OneBusAway.DataAccess.ObaService;

namespace OneBusAway.DataAccess
{
    /// <summary>
    /// Class talks to the OneBusAway server and grabs data from it.
    /// </summary>
    public class ObaDataAccess
    {
        /// <summary>
        /// This dictionary caches the lists of routes for a particular region.
        /// </summary>
        private static Dictionary<string, List<Route>> allRoutesCache = new Dictionary<string, List<Route>>();

        /// <summary>
        /// Creates the data access out of the current map view.
        /// </summary>
        /// <returns>A new ObaDataAccess object</returns>
        public static ObaDataAccess Create()
        {
            return Create(MapView.Current.MapCenter.Latitude, MapView.Current.MapCenter.Longitude);
        }

        /// <summary>
        /// Returns an oba data access with a specific location.
        /// </summary>
        public static ObaDataAccess Create(double lat, double lon)
        {
            return new ObaDataAccess(lat, lon);
        }

        /// <summary>
        /// Creates the data access.
        /// </summary>
        private ObaDataAccess(double latitude, double longitude)
        {
            this.Factory = new ObaServiceHelperFactory(latitude, longitude);
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
        /// Returns a list of all routes for the current location.
        /// </summary>
        /// <returns></returns>
        public async Task<List<Route>> GetAllRoutesForCurrentRegionAsync()
        {
            List<Route> allRoutes = null;
            Region closestRegion = await this.Factory.FindClosestRegionAsync();

            if (!allRoutesCache.TryGetValue(closestRegion.RegionName, out allRoutes))
            {
                allRoutes = new List<Route>();
                foreach (Agency agency in await this.GetAllAgenciesAsync())
                {
                    foreach (Route route in await this.GetAllRouteIdsForAgencyAsync(agency))
                    {
                        allRoutes.Add(route);
                    }
                }

                allRoutesCache[closestRegion.RegionName] = allRoutes;
            }

            return allRoutes;
        }

        /// <summary>
        /// Gets stops near a location
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public Task<Stop[]> GetStopsForLocationAsync(double latitude, double longitude)
        {
            return this.GetStopsForLocationAsync(latitude, longitude, 0.0, 0.0, 0.0);
        }

        /// <summary>
        /// Gets stops by a location and a specific area.
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="latitudeSpan"></param>
        /// <param name="longitudeSpan"></param>
        /// <returns></returns>
        public Task<Stop[]> GetStopsForLocationAsync(double latitude, double longitude, double latitudeSpan, double longitudeSpan)
        {
            return this.GetStopsForLocationAsync(latitude, longitude, 0.0, latitudeSpan, longitudeSpan);
        }

        /// <summary>
        /// Gets the stops near a location.
        /// </summary>
        private async Task<Stop[]> GetStopsForLocationAsync(double latitude, double longitude, double radius, double latitudeSpan, double longitudeSpan)
        {
            try
            {
                var helper = await this.Factory.CreateHelperAsync(ObaMethod.stops_for_location);
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

                XDocument doc = await helper.SendAndRecieveAsync(UtilitiesConstants.NoCacheAge);
                if (doc != null)
                {
                    return (from stopElement in doc.Descendants("stop")
                            select new Stop(stopElement)).ToArray();
                }
            }
            catch
            {
            }

            return new Stop[] { };
        }

        /// <summary>
        /// Returns all of the agencies that OBA serves.
        /// </summary>
        public async Task<Agency[]> GetAllAgenciesAsync()
        {
            try
            {
                ObaMethod method = ObaMethod.agencies_with_coverage;
                var helper = await this.Factory.CreateHelperAsync(method);
                var doc = await helper.SendAndRecieveAsync(cacheTimeout: 30 * 24 * 60 * 60 /* 30 days */);

                if (doc != null)
                {
                    return (from agencyWithCoverageElement in doc.Descendants("agency")
                            select new Agency(agencyWithCoverageElement)).ToArray();
                }
            }
            catch
            {
            }

            return new Agency[] { };
        }

        /// <summary>
        /// Returns all of the route Ids for a particular agency.
        /// </summary>
        public async Task<Route[]> GetAllRouteIdsForAgencyAsync(Agency agency)
        {
            try
            {
                ObaMethod method = ObaMethod.routes_for_agency;
                var helper = await this.Factory.CreateHelperAsync(method);
                helper.SetId(agency.Id);

                var doc = await helper.SendAndRecieveAsync(cacheTimeout: 5 * 24 * 60 * 60 /* 5 days */);
                if (doc != null)
                {
                    return (from routeElement in doc.Descendants("route")
                            select new Route(routeElement)
                            {
                                Agency = agency
                            }).ToArray();
                }
            }
            catch
            {
            }

            return new Route[] { };
        }

        /// <summary>
        /// Returns the routes for a particular stop.
        /// </summary>
        public async Task<Route[]> GetRoutesForStopAsync(string stopId)
        {
            try
            {
                ObaMethod method = ObaMethod.stop;
                
                var helper = await this.Factory.CreateHelperAsync(method);
                helper.SetId(stopId);
                var doc = await helper.SendAndRecieveAsync();

                if (doc != null)
                {
                    return (from routeElement in doc.Descendants("route")
                            select new Route(routeElement)).ToArray();
                }
            }
            catch
            {
            }

            return new Route[] { };
        }

        /// <summary>
        /// Returns the stops for a particular route.
        /// </summary>
        public async Task<Stop[]> GetStopsForRouteAsync(string routeId)
        {
            try
            {
                ObaMethod method = ObaMethod.stops_for_route;
                var helper = await this.Factory.CreateHelperAsync(method);
                helper.SetId(routeId);

                var doc = await helper.SendAndRecieveAsync();

                // Find all of the stops in the payload that have this route id:
                if (doc != null)
                {
                    return (from stopElement in doc.Descendants("stop")
                            select new Stop(stopElement)).ToArray();
                }
            }
            catch
            {
            }

            return new Stop[] { };
        }

        /// <summary>
        /// Returns the details for a particular stop.
        /// </summary>
        public async Task<Stop> GetStopDetailsAsync(string stopId)
        {
            try
            {
                ObaMethod method = ObaMethod.stop;
                var helper = await this.Factory.CreateHelperAsync(method);
                helper.SetId(stopId);

                var doc = await helper.SendAndRecieveAsync();

                // Find all of the stops in the payload that have this route id:
                if (doc != null)
                {
                    var entryElement = doc.Descendants("entry").First();
                    return new Stop(entryElement);
                }
            }
            catch
            {
            }

            return null;
        }

        /// <summary>
        /// Returns the schedule for a particular stop / route combination.
        /// </summary>
        public async Task<StopRouteSchedule[]> GetScheduleForStopAndRouteAsync(string stopId, string routeId, DateTime date)
        {
            XDocument doc = null;
            try
            {
                ObaMethod method = ObaMethod.schedule_for_stop;
                var helper = await this.Factory.CreateHelperAsync(method);
                helper.SetId(stopId);
                helper.AddToQueryString("date", date.ToString("yyyy-MM-dd"));

                doc = await helper.SendAndRecieveAsync(UtilitiesConstants.NoCacheAge);
                if (doc != null)
                {
                    var stopRouteScheduleElement = doc.Descendants("stopRouteSchedule")
                        .Where(xe => string.Equals(xe.GetFirstElementValue<string>("routeId"), routeId, StringComparison.OrdinalIgnoreCase))
                        .FirstOrDefault();

                    if (stopRouteScheduleElement == null)
                    {
                        throw new ArgumentException(string.Format("Unknown route / stop combination {0}/{1}", routeId, stopId));
                    }

                    DateTime serverTime = doc.Root.GetFirstElementValue<long>("currentTime").ToDateTime();
                    return (from stopRouteDirectionScheduleElement in stopRouteScheduleElement.Descendants("stopRouteDirectionSchedule")
                            select new StopRouteSchedule(serverTime, stopRouteDirectionScheduleElement)).ToArray();
                }
            }
            catch (Exception e)
            {
                // Make sure ArgumentException's bubble up.
                if (e is ArgumentException)
                {
                    throw;
                }
            }

            return new StopRouteSchedule[] { };
        }

        /// <summary>
        /// Returns route data for a particular route. Since there are two directions for each route,
        /// this method returns an array of routes - one for each direction.
        /// </summary>
        public async Task<RouteData[]> GetRouteDataAsync(string routeId)
        {
            try
            {
                ObaMethod method = ObaMethod.stops_for_route;

                var helper = await this.Factory.CreateHelperAsync(method);
                helper.SetId(routeId);

                var doc = await helper.SendAndRecieveAsync();

                if (doc != null)
                {
                    XElement dataElement = doc.Descendants("data").First();
                    return (from stopGroupElement in dataElement.Descendants("stopGroup")
                            select new RouteData(dataElement, stopGroupElement)).ToArray();
                }
            }
            catch
            {
            }

            return new RouteData[] { };
        }

        /// <summary>
        /// Returns the route data for a route.
        /// </summary>
        public async Task<RouteData> GetRouteDataAsync(string routeId, string stopId)
        {
            try
            {
                ObaMethod method = ObaMethod.stops_for_route;
                var helper = await this.Factory.CreateHelperAsync(method);
                helper.SetId(routeId);
                
                var doc = await helper.SendAndRecieveAsync();

                if (doc != null)
                {
                    XElement dataElement = doc.Descendants("data").First();
                    return new RouteData(dataElement, stopId);
                }
            }
            catch
            {
            }

            return new RouteData();
        }

        /// <summary>
        /// Returns real time tracking data for the buses at a particular stop.
        /// </summary>
        public async Task<TrackingData[]> GetTrackingDataForStopAsync(string stopId)
        {
            try
            {
                ObaMethod method = ObaMethod.arrivals_and_departures_for_stop;
                var helper = await this.Factory.CreateHelperAsync(method);

                helper.SetId(stopId);
                helper.AddToQueryString("minutesAfter", "60");

                XDocument doc = await helper.SendAndRecieveAsync(UtilitiesConstants.NoCacheAge);

                if (doc != null)
                {
                    DateTime serverTime = doc.Root.GetFirstElementValue<long>("currentTime").ToDateTime();

                    string stopName = doc.Descendants("stop").First().GetFirstElementValue<string>("name");

                    // Find all of the stops in the payload that have this route id:
                    return (from arrivalAndDepartureElement in doc.Descendants("arrivalAndDeparture")
                            select new TrackingData(serverTime, stopId, stopName, arrivalAndDepartureElement)).ToArray();
                }
            }
            catch
            {
            }

            return new TrackingData[] { };
        }

        /// <summary>
        /// Gets the trip data for a specific trip.
        /// </summary>
        public async Task<TripDetails> GetTripDetailsAsync(string tripId)
        {
            try
            {
                ObaMethod method = ObaMethod.trip_details;

                var helper = await this.Factory.CreateHelperAsync(method);
                helper.SetId(tripId);

                XDocument doc = await helper.SendAndRecieveAsync(UtilitiesConstants.NoCacheAge);

                if (doc != null)
                {
                    DateTime serverTime = doc.Root.GetFirstElementValue<long>("currentTime").ToDateTime();
                    XElement entryElement = doc.Descendants("entry").First();
                    return new TripDetails(entryElement, serverTime);
                }
            }
            catch
            {
            }

            return new TripDetails();
        }
    }
}
