using OneBusAway.ViewModels;
using OneBusAway.ViewModels.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace OneBusAway.Converters
{
    public class SubTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var viewModel = (HeaderControlViewModel)value;
            return (viewModel.HasSubText)
                ? viewModel.SubText
                : "ONE BUS AWAY";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException();
        }
    }
}
