using OneBusAway.DataAccess;
using OneBusAway.DataAccess.BingService;
using OneBusAway.Model;
using OneBusAway.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    public sealed class TileUpdaterService : IBackgroundTask 
    {
        /// <summary>
        /// True as long as the updater service is running.
        /// </summary>
        private bool isRunning;

        /// <summary>
        /// The deferral allows us to run beyond just our run state.
        /// </summary>
        private BackgroundTaskDeferral deferral;

        /// <summary>
        /// Creates the tile updater service.
        /// </summary>
        public TileUpdaterService()
        {
        }

        /// <summary>
        /// Runs the tile updating service.
        /// </summary>
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            this.isRunning = true;
            this.deferral = taskInstance.GetDeferral();
            taskInstance.Canceled += OnTaskInstanceCanceled;

            try
            {
                // First update the favorites and send the notification to the main tile:
                RoutesAndStopsControlViewModel routesAndStopsViewModel = new RoutesAndStopsControlViewModel();
                await routesAndStopsViewModel.PopulateFavoritesAsync();

                // Order the favorites by their scheduled arrival time:
                TileXMLBuilder mainTileBuilder = new TileXMLBuilder();
                this.AppendTrackingDataToTile(mainTileBuilder, routesAndStopsViewModel.RealTimeData, 5);

                // Now do pinned stops:
                var obaDataAccess = ObaDataAccess.Create();
                var pinnedStopTiles = await SecondaryTile.FindAllAsync();
                foreach (var pinnedStopTile in pinnedStopTiles)
                {
                    if (this.isRunning)
                    {
                        TileXMLBuilder secondaryTileBuilder = new TileXMLBuilder(pinnedStopTile.TileId);
                        PageInitializationParameters parameters = null;

                        // Be safe and try this first...should never happen. Note - we can't delete here
                        // because we're in a background process:
                        if (PageInitializationParameters.TryCreate(pinnedStopTile.Arguments, out parameters))
                        {
                            double lat = parameters.GetParameter<double>("lat");
                            double lon = parameters.GetParameter<double>("lon");
                            string stopId = parameters.GetParameter<string>("stopId");

                            if (!string.IsNullOrEmpty(stopId) && lat != 0 && lon != 0)
                            {
                                // Append the first tile which is the map:
                                await secondaryTileBuilder.AppendTileWithLargePictureAndTextAsync(stopId, lat, lon, pinnedStopTile.DisplayName);
                                TrackingData[] trackingData = await obaDataAccess.GetTrackingDataForStopAsync(stopId);
                                this.AppendTrackingDataToTile(secondaryTileBuilder, trackingData, 4);
                            }
                        }
                    }
                }
            }
            catch
            {
                // to do...how do we process this?!?!?
            }
            finally
            {
                deferral.Complete();
            }
        }

        /// <summary>
        /// Appends count number of tiles to the tile builder.
        /// </summary>
        private void AppendTrackingDataToTile(TileXMLBuilder tileBuilder, TrackingData[] unorderedTrackingData, int count)
        {
            var orderedTrackingData = (from rtd in unorderedTrackingData
                                       where !rtd.IsNoData
                                       orderby rtd.PredictedArrivalInMinutes
                                       select rtd).Take(count).ToList();

            if (orderedTrackingData.Count > 1)
            {
                tileBuilder.EnableNotificationQueue();
            }

            foreach (TrackingData trackingData in orderedTrackingData)
            {
                tileBuilder.AppendWideTileWithBlockTextAndLines(trackingData.PredictedArrivalInMinutes.ToString(),
                    trackingData.Status,
                    string.Format("BUS {0}", trackingData.Route.ShortName.ToUpper()),
                    trackingData.TripHeadsign.ToUpper(),
                    trackingData.StopName.ToUpper(),
                    string.Format("{0} / {1}", trackingData.PredictedArrivalTime.ToString("h:mm"), trackingData.ScheduledArrivalTime.ToString("h:mm")));
            }
        }

        /// <summary>
        /// Cancel the pending background task.
        /// </summary>
        private void OnTaskInstanceCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            this.isRunning = false;
        }
    }
}
