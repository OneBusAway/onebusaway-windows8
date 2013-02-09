using OneBusAway.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace OneBusAway.Converters
{
    public class TrackingDataToPredictedArrivalInMinutes : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            TrackingData trackingData = (TrackingData)value;
            return (trackingData.IsNoData)
                ? "-"
                : trackingData.PredictedArrivalInMinutes.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException();
        }
    }
}
