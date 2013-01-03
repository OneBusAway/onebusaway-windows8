using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace OneBusAway.Converters
{
    public class BoolToObaGreenConverter : IValueConverter
    {
        private SolidColorBrush obaGreenBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0x78, 0xAA, 0x36));
        private SolidColorBrush grayBrush = new SolidColorBrush(Colors.Gray);

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool invert = false;
            if (parameter != null)
            {
                bool.TryParse(parameter as string, out invert);
            }

            if (invert)
            {
                return ((bool)value)
                    ? grayBrush
                    : obaGreenBrush;
            }
            else
            {
                return ((bool)value)
                    ? obaGreenBrush
                    : grayBrush;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
