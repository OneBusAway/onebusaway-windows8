/* Copyright 2013 Michael Braude and individual contributors.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using System;

namespace OneBusAway.Utilities
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
        trips_for_route,

        // Not a real OBA method - just identifies the regions service separately.
        regions,
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

    /// <summary>
    /// An enumeration to denote confidence of a search result
    /// </summary>
    public enum Confidence
    {
        Low = 0,
        Medium,
        High
    }
}