using OneBusAway.DataAccess;
using OneBusAway.DataAccess.BingService;
using OneBusAway.Model;
using OneBusAway.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using Windows.UI.StartScreen;

namespace OneBusAway.Backgrounding
{
    /// <summary>
    /// This class is used to add live tiles in the background process.
    /// </summary>
    internal class TileUpdaterService
    {
        /// <summary>
        /// The one and only instance of the service.
        /// </summary>
        private static TileUpdaterService instance = new TileUpdaterService();

        /// <summary>
        /// This is the task that represent the service's ongoing work.
        /// </summary>
        private Task backgroundTask;

        /// <summary>
        /// True as long as the updater service is running.
        /// </summary>
        private volatile bool isRunning;

        /// <summary>
        /// The deferral allows us to run beyond just our run state.
        /// </summary>
        private BackgroundTaskDeferral deferral;

        /// <summary>
        /// This is used to cancel the service.
        /// </summary>
        private CancellationTokenSource cancellationToken;

        /// <summary>
        /// This cache allows us to hold onto data before it expires.
        /// </summary>
        private TimedCache cache;

        /// <summary>
        /// Creates the tile updater service.
        /// </summary>
        private TileUpdaterService()
        {
            this.backgroundTask = Task.FromResult<object>(null);
            this.cancellationToken = new CancellationTokenSource();
            this.cache = new TimedCache();
        }

        /// <summary>
        /// Returns the one and only instance.
        /// </summary>
        public static TileUpdaterService Instance
        {
            get
            {
                return instance;
            }
        }

        /// <summary>
        /// If the update loop isn't already running, this will start it.
        /// </summary>
        public Task CreateIfNeccessary(BackgroundTaskDeferral deferral)
        {
            if (!this.isRunning || this.backgroundTask.Status == TaskStatus.RanToCompletion)
            {
                this.isRunning = true;                
                this.deferral = deferral;
                this.cancellationToken = new CancellationTokenSource();
                this.backgroundTask = Task.Run(new Func<Task>(this.RunAsync));
                return this.backgroundTask;
            }

            // If the task is already running, just return a completed task.
            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Cancels the in-progress task.
        /// </summary>
        public void Abort()
        {
            this.isRunning = false;
            this.cancellationToken.Cancel();
            this.cache.Clear();
        }

        /// <summary>
        /// Runs the tile updating service.
        /// </summary>
        private async Task RunAsync()
        {
            try
            {
                while (this.isRunning)
                {
                    try
                    {
                        // First update the favorites:
                        var obaDataAccess = ObaDataAccess.Create();
                        var favorites = await this.cache.GetOrAddAsync<List<StopAndRoutePair>>(
                            "favorites",
                            () => Model.Favorites.GetAsync());

                        // Get the tracking data for favorites & filter it out by the routes:
                        List<TrackingData> favoritesRealTimeData = new List<TrackingData>();
                        foreach (StopAndRoutePair favorite in favorites)
                        {
                            this.cancellationToken.Token.ThrowIfCancellationRequested();

                            // Get tracking data for this stop:
                            TrackingData[] trackingData = await this.cache.GetOrAddAsync<TrackingData[]>(
                                favorite.Stop,
                                () => obaDataAccess.GetTrackingDataForStopAsync(favorite.Stop));

                            // Adds the tracking data to the list:
                            favoritesRealTimeData.AddRange(from data in trackingData
                                                           where string.Equals(favorite.Route, data.RouteId, StringComparison.OrdinalIgnoreCase)
                                                           select data);
                        }

                        // Now it's time to update the main tile with data:
                        TileXMLBuilder mainTileBuilder = new TileXMLBuilder();
                        this.AppendTrackingDataToTile(mainTileBuilder, favoritesRealTimeData);

                        // And now we can update the secondary tiles!
                        var pinnedStopTiles = await SecondaryTile.FindAllAsync();
                        foreach (var pinnedStopTile in pinnedStopTiles)
                        {
                            this.cancellationToken.Token.ThrowIfCancellationRequested();
                            PageInitializationParameters parameters = null;

                            // Be safe and try this first...should never happen.
                            if (PageInitializationParameters.TryCreate(pinnedStopTile.Arguments, out parameters))
                            {
                                double lat = parameters.GetParameter<double>("lat");
                                double lon = parameters.GetParameter<double>("lon");
                                string stopId = parameters.GetParameter<string>("stopId");

                                if (!string.IsNullOrEmpty(stopId) && lat != 0 && lon != 0)
                                {
                                    // Get the tracking data:
                                    TrackingData[] trackingData = await this.cache.GetOrAddAsync<TrackingData[]>(
                                        stopId,
                                        () => obaDataAccess.GetTrackingDataForStopAsync(stopId));

                                    // Update the tile:
                                    TileXMLBuilder secondaryTileBuilder = new TileXMLBuilder(pinnedStopTile.TileId);
                                    this.AppendTrackingDataToTile(secondaryTileBuilder, trackingData);
                                }
                            }
                        }

                        await Task.Delay(TimeSpan.FromMinutes(1), this.cancellationToken.Token);
                    }
                    catch
                    {
                        // Sometimes OBA will fail. What can you do?
                    }
                }
            }
            finally
            {
                // Always make sure we set this to completed
                this.deferral.Complete();
            }
        }

        /// <summary>
        /// Appends count number of tiles to the tile builder.
        /// </summary>
        private void AppendTrackingDataToTile(TileXMLBuilder tileBuilder, IEnumerable<TrackingData> unorderedTrackingData)
        {
            var orderedTrackingData = (from rtd in unorderedTrackingData
                                       where !rtd.IsNoData && rtd.PredictedArrivalTime > DateTime.Now
                                       orderby rtd.PredictedArrivalInMinutes
                                       select rtd).Take(4).ToList();

            if (orderedTrackingData.Count > 1)
            {
                tileBuilder.EnableNotificationQueue();
            }

            foreach (TrackingData trackingData in orderedTrackingData)
            {
                tileBuilder.AppendWideTileWithBlockTextAndLines((trackingData.PredictedArrivalTime - DateTime.Now).Minutes.ToString(),
                    trackingData.Status,
                    string.Format("BUS {0}", trackingData.Route.ShortName.ToUpper()),
                    trackingData.TripHeadsign.ToUpper(),
                    trackingData.StopName.ToUpper(),
                    string.Format("{0} / {1}", trackingData.PredictedArrivalTime.ToString("h:mm"), trackingData.ScheduledArrivalTime.ToString("h:mm")));
            }
        }
    }
}
