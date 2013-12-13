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
using OneBusAway.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

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
                    XmlSerializer serializer = new XmlSerializer(typeof(List<StopAndRoutePair>));
                    using (var stream = await ServiceRepository.FileService.ReadFileAsStreamAsync(FavoritesFileName))
                    {
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
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<StopAndRoutePair>));
                using (var memoryStream = new MemoryStream(0))
                {
                    serializer.Serialize(memoryStream, instance.favorites);
                    await ServiceRepository.FileService.WriteFileAsync(FavoritesFileName, memoryStream);
                }                
            }
            catch
            {
                // Ignore any IO exceptions...don't take down the process if we can't persist favs!
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
