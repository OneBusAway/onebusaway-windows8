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
using OneBusAway.Model;
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
    public class TrackingDataToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            TrackingData trackingData = (TrackingData)value;

            if (trackingData.IsNoData)
            {
                return new SolidColorBrush(Color.FromArgb(0xFF, 0xcc, 0x99, 0x00));
            }
            else
            {
                int difference = trackingData.PredictedArrivalInMinutes - trackingData.ScheduledArrivalInMinutes;

                if (difference > 0)
                {
                    return new SolidColorBrush(Color.FromArgb(0xFF, 0x24, 0xA0, 0xF2));
                }

                return new SolidColorBrush(Color.FromArgb(0xFF, 0x66, 0x66, 0x66));
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException();
        }
    }
}
