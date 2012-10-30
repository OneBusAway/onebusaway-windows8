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

            return (T)Convert.ChangeType(childNode.Value, typeof(T));
        }

    }
}
