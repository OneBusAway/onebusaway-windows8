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
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace OneBusAway.Converters
{
    /// <summary>
    /// Allows in-line xaml string formats.
    /// </summary>
    public class StringFormatConverter : IValueConverter
    {
        /// <summary>
        /// Assumes the parameter is a string format.
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (value == null)
                ? string.Empty
                : string.Format(CultureInfo.CurrentCulture, parameter.ToString(), value.ToString()).ToUpper();
        }
        
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException();
        }
    }
}
