using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace OneBusAway.Converters
{
    public class LastUpdatedTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is DateTime)
            {
                DateTime time = (DateTime)value;
                if (time > DateTime.MinValue)
                {
                    return String.Format("LAST UPDATED: {0}", time.ToString("hh:mm tt"));
                }
            }

            return String.Empty;            
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException();
        }
    }
}
