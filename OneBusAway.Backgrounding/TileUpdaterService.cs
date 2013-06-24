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
using Windows.System.Threading;
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
        /// Thred pool timer fires every minute.
        /// </summary>
        private ThreadPoolTimer timer;

        /// <summary>
        /// This is used to cancel the service.
        /// </summary>
        private CancellationTokenSource cancellationToken;

        /// <summary>
        /// This deferral is completed once the service exits.
        /// </summary>
        private BackgroundTaskDeferral deferral;

        /// <summary>
        /// This cache allows us to hold onto data before it expires.
        /// </summary>
        private TimedCache cache;

        /// <summary>
        /// Creates the tile updater service.
        /// </summary>
        private TileUpdaterService()
        {
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
        public bool CreateIfNeccessary(BackgroundTaskDeferral deferral)
        {
            if (this.timer == null)
            {
                lock (instance)
                {
                    if (this.timer == null)
                    {
                        this.deferral = deferral;
                        this.cancellationToken = new CancellationTokenSource();
                        this.timer = ThreadPoolTimer.CreatePeriodicTimer(new TimerElapsedHandler(OnTimerElapsed), TimeSpan.FromMinutes(1));

                        // Fire off an immediate update so that the user sees the tiles update right away!
                        var ignored = Task.Run(() => OnTimerElapsed(null));
                        return true;
                    }
                }
            }

            deferral.Complete();
            return false;
        }

        /// <summary>
        /// Cancels the in-progress task.
        /// </summary>
        public void Abort()
        {
            this.cache.Clear();

            if (this.timer != null)
            {
                this.deferral.Complete();
                this.cancellationToken.Cancel();
                this.timer.Cancel();
                this.timer = null;
            }
        }

        /// <summary>
        /// Runs the tile updating service.
        /// </summary>
        private async void OnTimerElapsed(ThreadPoolTimer timer)
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
                            await secondaryTileBuilder.AppendTileWithLargePictureAndTextAsync(
                                pinnedStopTile.TileId,
                                lat,
                                lon,
                                pinnedStopTile.DisplayName);

                            this.AppendTrackingDataToTile(secondaryTileBuilder, trackingData);
                        }
                    }
                }
            }
            catch
            {
                // Sometimes OBA will fail. What can you do?
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
                                       select rtd).Take(tileBuilder.IsMainTileUpdater ? 5 : 4).ToList();

            if (orderedTrackingData.Count > 1)
            {
                tileBuilder.EnableNotificationQueue();
            }

            foreach (TrackingData trackingData in orderedTrackingData)
            {
                tileBuilder.AppendTileWithBlockTextAndLines((trackingData.PredictedArrivalTime - DateTime.Now).Minutes.ToString(),
                    trackingData.Status,
                    string.Format("BUS {0}", trackingData.Route.ShortName.ToUpper()),
                    trackingData.TripHeadsign.ToUpper(),
                    trackingData.StopName.ToUpper(),
                    string.Format("{0} / {1}", trackingData.PredictedArrivalTime.ToString("h:mm"), trackingData.ScheduledArrivalTime.ToString("h:mm")));
            }
        }
    }
}
