using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneBusAway.Model
{
    public class MapView
    {
        public MapView(Point mapCenter, double zoomLevel)
        {
            MapCenter = mapCenter;
            ZoomLevel = zoomLevel;
        }

        public MapView(Point mapCenter, double zoomLevel, double boundsHeight, double boundsWidth)
        {
            MapCenter = mapCenter;
            ZoomLevel = zoomLevel;
            BoundsHeight = boundsHeight;
            BoundsWidth = boundsWidth;
        }

        /// <summary>
        /// This is a place where we can cache the current map view so that moving from page to page can re-use 
        /// the same map view.
        /// </summary>
        public static MapView Current
        {
            get;
            set;
        }

        public Point MapCenter { get; set; }

        public double ZoomLevel { get; set; }

        public double BoundsHeight { get; set; }

        public double BoundsWidth { get; set; }
    }
}
