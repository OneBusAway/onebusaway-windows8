using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OneBusAway.Model
{
    /// <summary>
    /// This is the shape of a bus route.
    /// </summary>
    public class Shape : BindableBase
    {
        private Point[] points;

        public Shape()
        {
        }

        /// <summary>
        /// Creates a shape out of a google encoded uri.  This code was taken from the WP7 OneBusAway
        /// client - 
        /// 
        /// http://onebusawaywp7.codeplex.com/SourceControl/changeset/view/94877#1472108
        /// 
        /// Google documentation on polyline decoding:
        /// 
        /// http://code.google.com/apis/maps/documentation/polylinealgorithm.html
        /// 
        /// </summary>
        public Shape(XElement shapeElement)
        {
            string encoded = shapeElement.GetFirstElementValue<string>("points");
            int index = 0;
            int latitude = 0;
            int longitude = 0;

            int length = encoded.Length;
            List<Point> pointList = new List<Point>();

            while (index < length)
            {
                latitude += DecodePoint(encoded, index, out index);
                longitude += DecodePoint(encoded, index, out index);

                Point point = new Point();
                point.Latitude = (latitude * 1e-5);
                point.Longitude = (longitude * 1e-5);

                pointList.Add(point);
            }

            this.Points = pointList.ToArray();
        }

        public Point[] Points
        {
            get
            {
                return this.points;
            }
            set
            {
                SetProperty(ref this.points, value);
            }
        }

        /// <summary>
        /// Helper method taken from the WP7 implementation:
        /// 
        /// http://onebusawaywp7.codeplex.com/SourceControl/changeset/view/94877#1472108
        /// </summary>
        private static int DecodePoint(string encoded, int startindex, out int finishindex)
        {
            int b;
            int shift = 0;
            int result = 0;

            //magic google algorithm, see http://code.google.com/apis/maps/documentation/polylinealgorithm.html
            do
            {
                b = Convert.ToInt32(encoded[startindex++]) - 63;
                result |= (b & 0x1f) << shift;
                shift += 5;
            } while (b >= 0x20);
            //if negative flip
            int dlat = (((result & 1) > 0) ? ~(result >> 1) : (result >> 1));

            //set output index
            finishindex = startindex;

            return dlat;
        }
    }
}
