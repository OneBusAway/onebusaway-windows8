using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace OneBusAway.Converters
{
    public class IsCurrentHourToThicknessConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            DateTime[] valueTime = (DateTime[])value;
            if (valueTime.Length == 0)
            {
                return new Thickness(0);
            }

            double thickness = 0.0;
            if (parameter != null)
            {
                double.TryParse(parameter as string, out thickness);
            }

            return (valueTime[0].Hour == DateTime.Now.Hour)
                ? new Thickness(thickness)
                : new Thickness(0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
