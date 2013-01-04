using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace OneBusAway.Converters
{
    public class BoolToThicknessConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            double thickness = 0.0;
            if (value == null || !double.TryParse(parameter as string, out thickness))
            {
                return new Thickness(0.0);
            }

            bool boolValue = (bool)value;
            return new Thickness((boolValue) ? thickness : 0.0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException();
        }
    }
}
