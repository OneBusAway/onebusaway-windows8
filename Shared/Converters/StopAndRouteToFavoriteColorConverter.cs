/* Copyright 2013 Michael Braude and individual contributors.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using System;

#if WINDOWS_PHONE
using System.Windows.Data;
using System.Windows;
using System.Windows.Media;
using System.Globalization;
#else
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI;
#endif

namespace OneBusAway.Converters
{
    public class StopAndRouteToFavoriteColorConverter : IValueConverter
    {
#if WINDOWS_PHONE
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
#else
        public object Convert(object value, Type targetType, object parameter, string language)
#endif
        {
            bool isFavorite = (bool)value;
            return (isFavorite)
                ? new SolidColorBrush(Color.FromArgb(0xFF, 0x78, 0xAA, 0x36))
                : new SolidColorBrush(Color.FromArgb(0xFF, 0xCF, 0xCF, 0xCF));
        }

#if WINDOWS_PHONE
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
#else
        public object ConvertBack(object value, Type targetType, object parameter, string language)
#endif
        {
            throw new NotSupportedException();
        }
    }
}
