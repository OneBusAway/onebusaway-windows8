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

        public Point MapCenter { get; set; }

        public double ZoomLevel { get; set; }

        public double BoundsHeight { get; set; }

        public double BoundsWidth { get; set; }
    }
}
