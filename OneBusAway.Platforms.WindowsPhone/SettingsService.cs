using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            // Not supported on windows phone:
            return false;
        }

        /// <summary>
        /// Not supported on Windows Phone.
        /// </summary>
        public string this[string setting]
        {
            get
            {
                throw new NotSupportedException();
            }
            set
            {
                throw new NotSupportedException();
            }
        }
    }
}
