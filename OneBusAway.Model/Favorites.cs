using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Windows.Storage;
using Windows.Storage.FileProperties;

namespace OneBusAway.Model
{
    public class Favorites
    {
        private const string FavoritesFileName = "favs.xml";

        private bool isInitialized;
        private List<StopAndRoutePair> favorites;
        private static Favorites instance = new Favorites();        

        /// <summary>
        /// Create the favorites object.
        /// </summary>
        private Favorites()
        {
            this.isInitialized = false;
            this.favorites = new List<StopAndRoutePair>();
        }

        /// <summary>
        /// Initialize the favorites.
        /// </summary>
        private static async Task InitializeAsync()
        {
            if (!instance.isInitialized)
            {
                try
                {
                    using (Stream stream = await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync(FavoritesFileName))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(List<StopAndRoutePair>));
                        instance.favorites = (List<StopAndRoutePair>)serializer.Deserialize(stream);
                    }
                }
                catch
                {
                    // Ignore exception if file doesn't exist.
                }
                finally
                {
                    instance.isInitialized = true;
                }
            }
        }

        /// <summary>
        /// Persists the favorites to disk.
        /// </summary>
        public static async Task PersistAsync()
        {
            await InitializeAsync();
            using (Stream stream = await ApplicationData.Current.LocalFolder.OpenStreamForWriteAsync(FavoritesFileName, CreationCollisionOption.ReplaceExisting))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<StopAndRoutePair>));
                serializer.Serialize(stream, instance.favorites);
            }
        }

        public static async Task<List<StopAndRoutePair>> GetAsync()
        {
            await InitializeAsync();
            return instance.favorites;
        }

        /// <summary>
        /// Adds a favorite to the collection.
        /// </summary>
        public static async Task AddAsync(StopAndRoutePair pair)
        {
            await InitializeAsync();
            instance.favorites.Add(pair);
        }

        /// <summary>
        /// Removes a favorite from the collection.
        /// </summary>
        public static async Task RemoveAsync(StopAndRoutePair pair)
        {
            await InitializeAsync();
            instance.favorites.Remove(pair);
        }

        /// <summary>
        /// Returns true if the pair is a favorite.
        /// </summary>
        public static async Task<bool> IsFavoriteAsync(StopAndRoutePair pair)
        {
            await InitializeAsync();
            return instance.favorites.Contains(pair);
        }
    }
}
