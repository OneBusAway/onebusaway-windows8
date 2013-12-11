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
        public Shape(XElement encodedPolylineElement)
        {
            string encoded = encodedPolylineElement.GetFirstElementValue<string>("points");
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
