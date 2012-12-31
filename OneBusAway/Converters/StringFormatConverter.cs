using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace OneBusAway.Converters
{
    /// <summary>
    /// Allows in-line xaml string formats.
    /// </summary>
    public class StringFormatConverter : IValueConverter
    {
        /// <summary>
        /// Assumes the parameter is a string format.
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (value == null)
                ? string.Empty
                : string.Format(CultureInfo.CurrentCulture, parameter.ToString(), value.ToString());
        }
        
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException();
        }
    }
}
