/* Copyright 2013 Microsoft
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneBusAway.ViewModels
{
    /// <summary>
    /// Constants for the view models assembly.
    /// </summary>
    public static class ViewModelConstants
    {
        /// <summary>
        /// Default map zoom when map control is loaded on any page
        /// </summary>
        public const double DefaultMapZoom = 12;

        /// <summary>
        /// Map zoom for a closer look than the default.
        /// </summary>
        public const double ZoomedInMapZoom = 16;

        /// <summary>
        /// The name of the route data cache file.
        /// </summary>
        public const string CacheFileName = "RouteData.xml";
    }
}
