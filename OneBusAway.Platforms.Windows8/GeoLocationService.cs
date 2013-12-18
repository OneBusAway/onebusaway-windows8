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
using OneBusAway.Model;
using OneBusAway.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace OneBusAway.Platforms.Windows8
{
    public class GeoLocationService : IGeoLocationService
    {
        public async Task<Point> FindUserLocationAsync()
        {
            Geolocator locator = new Geolocator();
            Geoposition geoPosition = await locator.GetGeopositionAsync();
            BasicGeoposition position = geoPosition.Coordinate.Point.Position;
            return new Point(position.Latitude, position.Longitude);
        }
    }
}
