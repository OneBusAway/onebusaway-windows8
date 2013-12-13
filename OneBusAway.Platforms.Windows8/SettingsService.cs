using OneBusAway.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace OneBusAway.Platforms.Windows8
{
    public class SettingsService : ISettingsService
    {
        public bool Contains(string setting)
        {
            return ApplicationData.Current.LocalSettings.Values.ContainsKey(setting);
        }

        public string this[string setting]
        {
            get
            {
                return (string)ApplicationData.Current.LocalSettings.Values[setting];
            }
            set
            {
                ApplicationData.Current.LocalSettings.Values[setting] = value;
            }
        }
    }
}
