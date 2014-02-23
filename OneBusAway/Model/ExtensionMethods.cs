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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OneBusAway.Model
{
    /// <summary>
    /// A class full of helpful extension methods.
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Returns the first elements value as type T.
        /// </summary>
        public static T GetFirstElementValue<T>(this XElement element, string childNodeName)
        {
            var childNode = element.Descendants(childNodeName).FirstOrDefault();
            
            if (childNode == null)
            {
                return default(T);
            }

            try
            {
                return (T)Convert.ChangeType(childNode.Value.Trim(), typeof(T));
            }
            catch
            {
                return default(T);
            }
        }

        /// <summary>
        /// Converts the OBA server time into a DateTime.
        /// </summary>
        public static DateTime ToDateTime(this long serverTime)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(serverTime);
        }
    }
}
