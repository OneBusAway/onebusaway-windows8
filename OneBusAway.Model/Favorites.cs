using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.Storage;
using Windows.Storage.FileProperties;

namespace OneBusAway.Model
{
    public class Favorites
    {
        private static StorageFile favoritesFile;
        private const string FavoritesFileName = "favs.txt";

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

            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            favoritesFile = await localFolder.CreateFileAsync(FavoritesFileName, CreationCollisionOption.OpenIfExists);

            IList<string> lines = await FileIO.ReadLinesAsync(favoritesFile);
            foreach (string line in lines)
            {
                string stop = line.Substring(0, line.IndexOf(","));
                string route = line.Substring(line.IndexOf(",") + 1);
                instance.favorites.Add(new StopAndRoutePair(stop, route));
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

        public static async Task<bool> Add(StopAndRoutePair pair)
        {
            if (instance == null)
            {
                throw new Exception("Favorites.Add: Favorites not initialized.");
            }

            if (Contains(instance.favorites, pair))
            {
                return false;
            }

            instance.favorites.Add(pair);
            List<string> list = new List<string>();
            list.Add(pair.Stop + "," + pair.Route);
            await FileIO.AppendLinesAsync(favoritesFile, list.AsEnumerable<string>());
            return true;
        }

        public static bool IsFavorite(StopAndRoutePair pair)
        {
            if (instance == null)
            {
                throw new Exception("Favorites.IsFavorite: Favorites not initialized.");
            }

            return Contains(instance.favorites, pair);
        }

        private static bool Contains(List<StopAndRoutePair> list, StopAndRoutePair pair)
        {
            foreach (StopAndRoutePair item in list)
            {
                if (string.Equals(item.Stop, pair.Stop, StringComparison.OrdinalIgnoreCase)
                    && string.Equals(item.Route, pair.Route, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
