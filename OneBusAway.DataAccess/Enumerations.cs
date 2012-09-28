using System;

namespace OneBusAway.DataAccess
{
    /// <summary>
    /// A collection of methods that OneBusAway supports.
    /// </summary>
    public enum ObaMethod
    {
        agencies_with_coverage,
        agency,
        arrival_and_departure_for_stop,
        arrivals_and_departures_for_stop,
        cancel_alarm,
        current_time,
        plan_trip,
        register_alarm_for_arrival_and_departure_at_stop,
        route_ids_for_agency,
        route,
        routes_for_agency,
        routes_for_location,
        schedule_for_stop,
        shape,
        stop_ids_for_agency,
        stop,
        stops_for_location,
        stops_for_route,
        trip_details,
        trip_for_vehicle,
        trip,
        trips_for_location,
        trips_for_route
    };

    /// <summary>
    /// An enumeration of the available methods available to REST APIs.
    /// </summary>
    public enum HttpMethod
    {
        /// <summary>
        /// Post method type.
        /// </summary>
        POST = 0,

        /// <summary>
        /// Get method type.
        /// </summary>
        GET = 1
    }
}