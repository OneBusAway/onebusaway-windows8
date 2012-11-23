using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace OneBusAway.Converters
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Converts a bool to a visibility.  If the parameter is set we will try 
        /// & parse it and use it to determine whether the value should be inverted.
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool invert = false;
            if (parameter != null && Boolean.TryParse(parameter as string, out invert))
            {
                if (invert)
                {
                    return ((bool)value) ? Visibility.Collapsed : Visibility.Visible;
                }
            }

            return ((bool)value) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException();
        }
    }
}
