/* Copyright 2014 Michael Braude and individual contributors.
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

#if WINDOWS_PHONE
using System.Windows;
using System.Windows.Media;
#else
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
#endif

namespace OneBusAway.Controls
{
    public static class ControlUtilities
    {
        /// <summary>
        /// Walks the logical tree until it finds type T.
        /// </summary>
        public static T GetParent<T>(DependencyObject parent)
            where T : DependencyObject
        {
            parent = VisualTreeHelper.GetParent(parent);
            while (parent != null)
            {
                if (parent is T)
                {
                    break;
                }

                parent = VisualTreeHelper.GetParent(parent);
            }

            return parent as T;
        }
    }
}
