using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace OneBusAway.Converters
{
    public class StringArrayToTypeArrayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string[] valueArray = ((string)value).Split(',');
            Type convertToType = Type.GetType(parameter.ToString());
            return valueArray.Select(val => System.Convert.ChangeType(val, convertToType)).ToArray();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
