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
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using OneBusAway.Services;
using Windows.Storage;

namespace OneBusAway.Platforms.WindowsPhone
{
    public class SettingsService : ISettingsService
    {
        /// <summary>
        /// Nothing to do.
        /// </summary>
        public SettingsService()
        {
        }

        /// <summary>
        /// Returns true if the settings contain a value.
        /// </summary>
        public bool Contains(string setting)
        {
            return IsolatedStorageSettings.ApplicationSettings.Contains(setting);
        }

        /// <summary>
        /// Not supported on Windows Phone.
        /// </summary>
        public string this[string setting]
        {
            get
            {
                return IsolatedStorageSettings.ApplicationSettings[setting].ToString();
            }
            set
            {
                IsolatedStorageSettings.ApplicationSettings[setting] = value;
            }
        }
    }
}
