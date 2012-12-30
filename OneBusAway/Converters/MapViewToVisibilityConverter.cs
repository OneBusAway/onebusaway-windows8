using OneBusAway.Model;
using OneBusAway.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace OneBusAway.Converters
{
    /// <summary>
    /// Automatically hides a BusStop (or similar control) when the map view
    /// has zoomed outside of the maximum view.
    /// </summary>
    public class MapViewToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Returns Visible if the maps zoom level is greater than the minimum bus stop visible zoom.
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            MapView mapView = (MapView)value;
            return (mapView.ZoomLevel < UtilitiesConstants.MinBusStopVisibleZoom)
                ? Visibility.Collapsed
                : Visibility.Visible;
        }

        /// <summary>
        /// Not supported.
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException();
        }
    }
}
