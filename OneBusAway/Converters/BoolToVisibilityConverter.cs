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
            bool? boolValue = (bool?)value;
            bool invert = false;

            if (parameter != null && Boolean.TryParse(parameter as string, out invert))
            {
                if (invert)
                {                    
                    return (boolValue.HasValue && boolValue.Value) ? Visibility.Collapsed : Visibility.Visible;
                }
            }

            return (boolValue.HasValue && boolValue.Value) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException();
        }
    }
}
