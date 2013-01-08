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

        private List<StopAndRoutePair> favorites;
        private static Favorites instance;

        private Favorites()
        {
            favorites = new List<StopAndRoutePair>();
        }

        public static async Task Initialize()
        {
            if (instance != null) return;

            instance = new Favorites();

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
        }

        public static async Task Persist()
        {
            if (instance == null || instance.favorites.Count == 0) return;

            using (Stream stream = await ApplicationData.Current.LocalFolder.OpenStreamForWriteAsync(FavoritesFileName, CreationCollisionOption.ReplaceExisting))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<StopAndRoutePair>));
                serializer.Serialize(stream, instance.favorites);
            }
        }

        public static List<StopAndRoutePair> Get()
        {
            if (instance == null)
            {
                throw new Exception("Favorites.Get: Favorites not initialized.");
            }

            return instance.favorites;
        }

        public static bool Add(StopAndRoutePair pair)
        {
            if (instance == null)
            {
                throw new Exception("Favorites.Add: Favorites not initialized.");
            }

            if (pair == null)
            {
                return false;
            }

            if (instance.favorites.Contains(pair))
            {
                return false;
            }

            instance.favorites.Add(pair);
            return true;
        }

        public static bool Remove(StopAndRoutePair pair)
        {
            if (instance == null)
            {
                throw new Exception("Favorites.Remove: Favorites not initialized.");
            }

            if (pair == null)
            {
                return false;
            }

            if (!instance.favorites.Contains(pair))
            {
                return false;
            }

            instance.favorites.Remove(pair);
            return true;
        }

        public static bool IsFavorite(StopAndRoutePair pair)
        {
            if (instance == null)
            {
                throw new Exception("Favorites.IsFavorite: Favorites not initialized.");
            }

            return instance.favorites.Contains(pair);
        }
    }
}
