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
        private Task refreshTask;

        /// <summary>
        /// A list of all the routes.
        /// </summary>
        private List<Route> allRoutes;

        /// <summary>
        /// Creates the cache.
        /// </summary>
        private AllRoutesCache()
        {
            this.allRoutes = new List<Route>();
        }

        /// <summary>
        /// Ensures that the cache is up to date. If it's not then we will ping OBA for the latest.
        /// </summary>
        public static async Task EnsureUpToDateAsync()
        {
            // If this is null, then let's try and populate the list of all routes:
            if (instance.refreshTask == null)
            {
                lock (instance)
                {
                    if (instance.refreshTask == null)
                    {
                        instance.refreshTask = GetAllResultsAsync();
                    }
                }
            }

            await instance.refreshTask;
        }

        /// <summary>
        /// Returns true if the cache is up to date.
        /// </summary>
        public static bool IsCacheUpToDate()
        {
            return (instance.refreshTask == null || !instance.refreshTask.IsCompleted);
        }

        /// <summary>
        /// Returns a list of all the routes asynchronously.
        /// </summary>
        public static async Task<List<Route>> GetAllRoutesAsync()
        {
            await EnsureUpToDateAsync();
            return instance.allRoutes;
        }

        /// <summary>
        /// Gets all of the results.
        /// </summary>
        private static Task GetAllResultsAsync()
        {
            return Task.Run(async delegate 
                {
                    if (!await ReadResultsFromCacheFileAsync())
                    {
                        XDocument document = new XDocument();
                        document.Add(new XElement("routes"));

                        var obaDataAccess = ObaDataAccess.Create();

                        foreach (Agency agency in await obaDataAccess.GetAllAgencies())
                        {
                            foreach (Route route in await obaDataAccess.GetAllRouteIdsForAgency(agency))
                            {
                                instance.allRoutes.Add(route);
                                document.Root.Add(route.ToXElement());
                            }
                        }

                        try
                        {
                            var newFile = await ApplicationData.Current.LocalFolder.CreateFileAsync(ViewModelConstants.CacheFileName, CreationCollisionOption.ReplaceExisting);
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
        private static async Task<bool> ReadResultsFromCacheFileAsync()
        {
            try
            {
                var existingFile = await ApplicationData.Current.LocalFolder.GetFileAsync(ViewModelConstants.CacheFileName);
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
                        instance.allRoutes = (from routeElement in document.Descendants("route")
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
    }
}
