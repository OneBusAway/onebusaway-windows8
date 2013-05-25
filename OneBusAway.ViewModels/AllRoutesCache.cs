using OneBusAway.DataAccess;
using OneBusAway.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Storage;

namespace OneBusAway.ViewModels
{
    /// <summary>
    /// This class will either query OBA for all routes for all agencies, or it will
    /// read them from a localy cached XML file.
    /// 
    /// The XML file is updated weekly.
    /// </summary>
    public class AllRoutesCache
    {
        /// <summary>
        /// Singleton class.
        /// </summary>
        private static AllRoutesCache instance = new AllRoutesCache();

        /// <summary>
        /// A task that represents communicating to OBA to get the complete routes data.
        /// </summary>
        private Dictionary<string, Task> refreshTasks;

        /// <summary>
        /// A list of all the routes.
        /// </summary>
        private Dictionary<string, List<Route>> allRoutes;

        /// <summary>
        /// Creates the cache.
        /// </summary>
        private AllRoutesCache()
        {
            this.refreshTasks = new Dictionary<string, Task>();
            this.allRoutes = new Dictionary<string, List<Route>>();
        }

        /// <summary>
        /// Ensures that the cache is up to date. If it's not then we will ping OBA for the latest.
        /// </summary>
        public static async Task<string> EnsureUpToDateAsync()
        {
            var dataAccess = ObaDataAccess.Create();
            string regionName = await dataAccess.FindRegionNameAsync();

            if (!string.IsNullOrEmpty(regionName))
            {
                // If this is null, then let's try and populate the list of all routes:
                if (!instance.refreshTasks.ContainsKey(regionName))
                {
                    lock (instance)
                    {
                        if (!instance.refreshTasks.ContainsKey(regionName))
                        {
                            instance.refreshTasks[regionName] = GetAllResultsAsync(regionName);
                        }
                    }
                }

                await instance.refreshTasks[regionName];
            }

            return regionName;
        }

        /// <summary>
        /// Returns true if the cache is up to date.
        /// </summary>
        public static async Task<bool> IsCacheUpToDateAsync()
        {
            // This will do a blocking wait, but that shouldn't result in lots of 
            // blocked time, usually, because the only thing this is waiting on
            // is the regions xml file. And that 
            string regionName = await ObaDataAccess.Create().FindRegionNameAsync();

            if (!string.IsNullOrEmpty(regionName))
            {
                Task refreshTask = null;
                if (instance.refreshTasks.TryGetValue(regionName, out refreshTask))
                {
                    return refreshTask.IsCompleted;
                }
            }

            return false;
        }

        /// <summary>
        /// Returns a list of all the routes asynchronously.
        /// </summary>
        public static async Task<List<Route>> GetAllRoutesAsync()
        {
            List<Route> allRoutes = null;
            string regionName = await EnsureUpToDateAsync();

            if (string.IsNullOrEmpty(regionName) || !instance.allRoutes.TryGetValue(regionName, out allRoutes))
            {
                // In this case, the region is either empty or there are no routes for this region.
                // Return an empty list:
                allRoutes = new List<Route>();
            }

            return allRoutes;
        }

        /// <summary>
        /// Gets all of the results.
        /// </summary>
        private static Task GetAllResultsAsync(string regionName)
        {
            return Task.Run(async delegate
                {
                    if (!await ReadResultsFromCacheFileAsync(regionName))
                    {
                        XDocument document = new XDocument();
                        document.Add(new XElement("routes"));

                        List<Route> allRoutes = null;
                        if (!instance.allRoutes.TryGetValue(regionName, out allRoutes))
                        {
                            allRoutes = new List<Route>();
                            instance.allRoutes[regionName] = allRoutes;
                        }

                        var obaDataAccess = ObaDataAccess.Create();
                        foreach (Agency agency in await obaDataAccess.GetAllAgencies())
                        {
                            foreach (Route route in await obaDataAccess.GetAllRouteIdsForAgency(agency))
                            {
                                allRoutes.Add(route);
                                document.Root.Add(route.ToXElement());
                            }
                        }

                        try
                        {
                            var newFile = await ApplicationData.Current.LocalFolder.CreateFileAsync(ComputeCacheFilePath(regionName), CreationCollisionOption.ReplaceExisting);
                            using (var stream = await newFile.OpenStreamForWriteAsync())
                            {
                                document.Save(stream);
                            }
                        }
                        catch
                        {
                            // Just in case this fails, let's not take down the application. The worst that happens
                            // here is we just don't save the cache file. We will get another try when the app restarts.
                        }
                    }
                });
        }

        /// <summary>
        /// Reads all the results from the cache file.
        /// </summary>
        private static async Task<bool> ReadResultsFromCacheFileAsync(string regionName)
        {
            try
            {
                var existingFile = await ApplicationData.Current.LocalFolder.GetFileAsync(ComputeCacheFilePath(regionName));
                var properties = await existingFile.GetBasicPropertiesAsync();

                // If the existing cache file is more than 7 days old, let's invalidate it:
                if ((DateTime.Now - properties.DateModified).TotalDays > 7)
                {
                    await existingFile.DeleteAsync();
                    return false;
                }
                else
                {
                    using (var stream = await existingFile.OpenStreamForReadAsync())
                    {
                        var document = XDocument.Load(stream, LoadOptions.None);

                        // Read all routes from the cache file:
                        instance.allRoutes[regionName] = (from routeElement in document.Descendants("route")
                                                          select new Route(routeElement)).ToList();
                    }
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Computes the path to the cache file for a region.
        /// </summary>
        /// <param name="regionName"></param>
        /// <returns></returns>
        private static string ComputeCacheFilePath(string regionName)
        {
            string filePath = Path.Combine("ObaCache", regionName);
            return Path.Combine(filePath, ViewModelConstants.CacheFileName);
        }
    }
}
