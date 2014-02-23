/* Copyright 2014 Michael Braude and individual contributors.
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

namespace OneBusAway.ViewModels
{
    /// <summary>
    /// A list of stops with one extra parameter.
    /// </summary>
    public class BusStopList : List<Stop>
    {
        /// <summary>
        /// Creates the list.
        /// </summary>
        public BusStopList()
            : base()
        {
        }

        public BusStopList(IEnumerable<Stop> collection)
            : base(collection)
        {
        }

        /// <summary>
        /// When true, the map control will clear existing bus stops.
        /// </summary>
        public bool ClearExistingStops
        {
            get;
            set;
        }
    }
}
