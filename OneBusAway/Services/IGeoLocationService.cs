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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneBusAway.Services
{
    /// <summary>
    /// Defines an object that can listen to events on the geo location service.
    /// Using the observer pattern instead of events so that the location service
    /// can use weak references to make manage garbage collection, instead of relying on
    /// the caller to unregister.
    /// </summary>
    public interface IGeoLocationServiceObserver
    {
        void OnUserLocationChanged(Point newLocation);
    }

    public interface IGeoLocationService
    {
        void RegisterForLocationChanged(IGeoLocationServiceObserver observer);

        Task<Point> FindUserLocationAsync();
    }
}
