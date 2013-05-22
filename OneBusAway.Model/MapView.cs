using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace OneBusAway.Model
{
    public class MapView
    {
        /// <summary>
        /// The latitude of Seattle's city center.
        /// </summary>
        public const double DefaultLatitude = 47.603561401367188;

        /// <summary>
        /// The longitude of Seattle's city center.
        /// </summary>
        public const double DefaultLongitude = -122.32943725585937;

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
                    current = new MapView(
                        GetDefaultLocation(),
                        12.0,
                        false);
                }

                return current;
            }
            set
            {
                current = value;
                ApplicationData.Current.RoamingSettings.Values["DefaultLatitude"] = current.MapCenter.Latitude;
                ApplicationData.Current.RoamingSettings.Values["DefaultLongitude"] = current.MapCenter.Longitude;
            }
        }

        private static Point GetDefaultLocation()
        {
            double latitude = DefaultLatitude;
            double longitude = DefaultLongitude;
            var roamingProperties = ApplicationData.Current.RoamingSettings.Values;

            if (roamingProperties.ContainsKey("DefaultLatitide"))
            {
                Double.TryParse(roamingProperties["DefaultLatitude"] as string, out latitude);
            }

            if (roamingProperties.ContainsKey("DefaultLongitude"))
            {
                Double.TryParse(roamingProperties["DefaultLongitude"] as string, out longitude);
            }

            return new Point(latitude, longitude);
        }

        public Point MapCenter { get; set; }

        public double ZoomLevel { get; set; }

        public double BoundsHeight { get; set; }

        public double BoundsWidth { get; set; }

        public bool AnimateChange { get; set; }
    }
}
