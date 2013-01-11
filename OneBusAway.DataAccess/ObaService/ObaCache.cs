using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Windows.Storage;
using Windows.Storage.FileProperties;

using OneBusAway.Model;
using OneBusAway.Utilities;

namespace OneBusAway.DataAccess
{
    /// <summary>
    /// Helper class to save Oba query results locally for faster retrieval
    /// </summary>
    public static class ObaCache
    {
        private static readonly string ObaCacheFolderName = "ObaCache";
        private static bool isCacheFolderExisted = false;

        /// <summary>
        /// Get cached query
        /// </summary>
        /// <param name="method">Method</param>
        /// <param name="id">Id</param>
        /// <param name="expectedCacheAge">Expected cache age in seconds</param>
        /// <returns>Cached query if existed and new; null otherwise.</returns>
        public static async Task<XDocument> GetCache(ObaMethod method, string id, int expectedCacheAge = UtilitiesConstants.DefaultCacheAge)
        {
            if (string.IsNullOrEmpty(id)) return null;

            try
            {
                using (Stream stream = await GetCacheFile(method, id))
                {
                    if (stream == null) return null;

                    XDocument doc = XDocument.Load(stream);

                    if (doc == null) return null;

                    DateTime serverTime = doc.Root.GetFirstElementValue<long>("currentTime").ToDateTime();
                    if (DateTime.Now.ToUniversalTime() - serverTime > TimeSpan.FromSeconds(expectedCacheAge))
                    {
                        // Cache is older than expected.
                        return null;
                    }

                    return doc;
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Get cached file.
        /// </summary>
        /// <param name="method">Method</param>
        /// <param name="id">Id</param>
        /// <returns>Read stream if the file exists; null otherwise</returns>
        private static async Task<Stream> GetCacheFile(ObaMethod method, string id)
        {
            string fileName = CombinePath(method, id);

            try
            {
                return await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync(fileName);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Save query result to cache
        /// </summary>
        /// <param name="method">Method</param>
        /// <param name="id">Id</param>
        /// <param name="doc">Query result</param>
        public static async Task SaveCache(ObaMethod method, string id, XDocument doc)
        {
            if (string.IsNullOrEmpty(id)) return;
            if (doc == null) return;

            string fileName = CombinePath(method, id);

            try
            {
                await CheckCacheFolder();
                using (Stream stream = await ApplicationData.Current.LocalFolder.OpenStreamForWriteAsync(fileName, CreationCollisionOption.ReplaceExisting))
                {
                    if (stream == null) return;
                    doc.Save(stream);
                }
            }
            catch
            {
                return;
            }
        }

        /// <summary>
        /// Helper function to keep path the cache file consistent.
        /// </summary>
        /// <param name="method">Method</param>
        /// <param name="id">Id</param>
        /// <returns>Combined path</returns>
        private static string CombinePath(ObaMethod method, string id)
        {
            return Path.Combine(ObaCacheFolderName, method.ToString() + "_" + id + ".xml");
        }

        /// <summary>
        /// Helper function to create cache folder if it doesn't exist.
        /// </summary>
        private static async Task CheckCacheFolder()
        {
            if (isCacheFolderExisted) return;

            await ApplicationData.Current.LocalFolder.CreateFolderAsync(ObaCacheFolderName, CreationCollisionOption.OpenIfExists);
            isCacheFolderExisted = true;
        }
    }
}
