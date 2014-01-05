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
        /// Locks reads / writes to / from the settings file.
        /// </summary>
        private static readonly object readWriteLock = new object();

        /// <summary>
        /// The settings object.
        /// </summary>
        private static Dictionary<string, string> settings;

        /// <summary>
        /// To persist settings across sessions, they are written to this file.
        /// </summary>
        public const string SETTINGS_FILE_NAME = "Settings.xml";

        /// <summary>
        /// Nothing to do.
        /// </summary>
        public SettingsService()
        {
        }

        ~SettingsService()
        {
            this.Dispose(false);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool isDisposing)
        {
            WriteSettings();
        }

        public Dictionary<string, string> Settings
        {
            get
            {
                ReadSettings();
                return settings;
            }
        }

        /// <summary>
        /// Returns true if the settings contain a value.
        /// </summary>
        public bool Contains(string setting)
        {
            return this.Settings.ContainsKey(setting);
        }

        /// <summary>
        /// Not supported on Windows Phone.
        /// </summary>
        public string this[string setting]
        {
            get
            {
                return this.Settings[setting];
            }
            set
            {
                this.Settings[setting] = value;
            }
        }

        /// <summary>
        /// Reads settings from isolated storage.
        /// </summary>
        private static void ReadSettings()
        {
            if (settings == null)
            {                
                var store = IsolatedStorageFile.GetUserStoreForApplication();
                lock (readWriteLock)
                {
                    if (store.FileExists(SETTINGS_FILE_NAME))
                    {
                        KeyValuePair[] keyValuePairs = null;
                        XmlSerializer serializer = new XmlSerializer(typeof(KeyValuePair[]));
                        using (var stream = new StreamReader(store.OpenFile(SETTINGS_FILE_NAME, FileMode.Open)))
                        {
                            keyValuePairs = (KeyValuePair[])serializer.Deserialize(stream);
                        }

                        settings = keyValuePairs.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
                    }
                    else
                    {
                        settings = new Dictionary<string, string>();
                    }
                }
            }
        }

        /// <summary>
        /// Persists settings to isolates storage.
        /// </summary>
        private static void WriteSettings()
        {
            if (settings != null)
            {
                var store = IsolatedStorageFile.GetUserStoreForApplication();
                lock (readWriteLock)
                {
                    KeyValuePair[] keyValuePairs = (from key in settings.Keys
                                                    select new KeyValuePair
                                                    {
                                                        Key = key,
                                                        Value = settings[key]
                                                    }).ToArray();

                    XmlSerializer serializer = new XmlSerializer(typeof(KeyValuePair[]));
                    using (var stream = new StreamWriter(store.OpenFile(SETTINGS_FILE_NAME, FileMode.Create)))
                    {
                        serializer.Serialize(stream, keyValuePairs);
                    }
                }
            }
        }

        /// <summary>
        /// Used because Dictionary is not Xml Serializable [but List is].
        /// </summary>
        public class KeyValuePair
        {
            public string Key;
            public string Value;
        }
    }
}
