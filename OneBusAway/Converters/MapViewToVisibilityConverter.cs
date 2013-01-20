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
            MapView mapView = value as MapView;
            if (mapView != null)
            {
                bool invert = false;
                if (parameter != null)
                {
                    bool.TryParse(parameter as string, out invert);
                }

                if (mapView.ZoomLevel < UtilitiesConstants.MinBusStopVisibleZoom)
                {
                    return (invert)
                        ? Visibility.Visible
                        : Visibility.Collapsed;                        
                }
                else
                {
                    return (invert)
                        ? Visibility.Collapsed
                        : Visibility.Visible;
                }
            }

            return Visibility.Collapsed;
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
