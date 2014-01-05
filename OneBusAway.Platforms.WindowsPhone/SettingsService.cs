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
