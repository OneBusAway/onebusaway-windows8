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
                return (T)Convert.ChangeType(childNode.Value, typeof(T));
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
