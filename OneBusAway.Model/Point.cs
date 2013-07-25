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

namespace OneBusAway.Model
{
    /// <summary>
    /// A point is a latitude / longitude pair that's displayed as a path on the map.
    /// </summary>
    public class Point : BindableBase
    {
        private double latitude;
        private double longitude;

        public Point()
        {
        }

        public Point(double _latitude, double _longitude)
        {
            latitude = _latitude;
            longitude = _longitude;
        }

        public double Latitude
        {
            get
            {
                return this.latitude;
            }
            set
            {
                SetProperty(ref this.latitude, value);
            }
        }

        public double Longitude
        {
            get
            {
                return this.longitude;
            }
            set
            {
                SetProperty(ref this.longitude, value);
            }
        }

        public static bool operator ==(Point a, Point b)
        {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            return a.Equals(b);
        }

        public static bool operator !=(Point a, Point b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            return Equals(obj as Point);
        }

        public bool Equals(Point point)
        {
            if ((object)point == null)
            {
                return false;
            }

            return  Latitude == point.Latitude && Longitude == point.Longitude;
        }

        public override int GetHashCode()
        {
            return Latitude.GetHashCode() ^ Longitude.GetHashCode();
        }
    }
}
