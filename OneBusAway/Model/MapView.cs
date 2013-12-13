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
using OneBusAway.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneBusAway.Model
{
    public class MapView
    {
        /// <summary>
        /// The latitude of Seattle's city center.
        /// </summary>
        public const double DefaultLatitude = 39.450000762939453;

        /// <summary>
        /// The longitude of Seattle's city center.
        /// </summary>
        public const double DefaultLongitude = -98.907997131347656;

        /// <summary>
        /// This is the current map view.
        /// </summary>
        private static MapView current;

        public MapView(Point mapCenter, double zoomLevel, bool animateChange = false)
        {
            MapCenter = mapCenter;
            ZoomLevel = zoomLevel;
            AnimateChange = animateChange;
        }

        public MapView(Point mapCenter, double zoomLevel, double boundsHeight, double boundsWidth, bool animateChange = false)
        {
            MapCenter = mapCenter;
            ZoomLevel = zoomLevel;
            BoundsHeight = boundsHeight;
            BoundsWidth = boundsWidth;
            AnimateChange = animateChange;
        }

        /// <summary>
        /// This is a place where we can cache the current map view so that moving from page to page can re-use 
        /// the same map view.
        /// </summary>
        public static MapView Current
        {
            get
            {
                if (current == null)
                {
                    current = GetDefaultMapView();
                }

                return current;
            }
            set
            {
                current = value;
                ServiceRepository.SettingsService["DefaultLatitude"] = current.MapCenter.Latitude.ToString();
                ServiceRepository.SettingsService["DefaultLongitude"] = current.MapCenter.Longitude.ToString();
                ServiceRepository.SettingsService["DefaultZoom"] = current.ZoomLevel.ToString();
            }
        }

        /// <summary>
        /// Returns the default location of the user, or their last known location.
        /// </summary>
        /// <returns></returns>
        private static MapView GetDefaultMapView()
        {
            double latitude = DefaultLatitude;
            double longitude = DefaultLongitude;
            double zoom = 4.0;

            if (ServiceRepository.SettingsService.Contains("DefaultLatitude") &&
                ServiceRepository.SettingsService.Contains("DefaultLongitude") &&
                ServiceRepository.SettingsService.Contains("DefaultZoom"))
            {
                latitude = Convert.ToDouble(ServiceRepository.SettingsService["DefaultLatitude"]);
                longitude = Convert.ToDouble(ServiceRepository.SettingsService["DefaultLongitude"]);
                zoom = Convert.ToDouble(ServiceRepository.SettingsService["DefaultZoom"]);
            }

            return new MapView(
                new Point(latitude, longitude),
                zoom,
                false);
        }

        public Point MapCenter { get; set; }

        public double ZoomLevel { get; set; }

        public double BoundsHeight { get; set; }

        public double BoundsWidth { get; set; }

        public bool AnimateChange { get; set; }
    }
}
