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
using OneBusAway.DataAccess;
using OneBusAway.DataAccess.BingService;
using OneBusAway.DataAccess.ObaService;
using OneBusAway.Model;
using OneBusAway.Platforms.Windows8;
using OneBusAway.Services;
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
        /// Runs the tile updating service.
        /// </summary>
        public static async Task UpdateTilesAsync(CancellationToken token)
        {
            // Inject the platform services into the PCL:
            ServiceRepository.FileService = new FileService();
            ServiceRepository.GeoLocationService = new GeoLocationService();
            ServiceRepository.SettingsService = new SettingsService();

            try
            {
                // First update the favorites:                
                var favorites = await Model.Favorites.GetAsync();

                // Get the tracking data for favorites & filter it out by the routes:
                List<TrackingData> favoritesRealTimeData = new List<TrackingData>();
                foreach (StopAndRoutePair favorite in favorites)
                {
                    token.ThrowIfCancellationRequested();

                    // Get tracking data for this stop:
                    var obaDataAccess = ObaDataAccess.Create();
                    TrackingData[] trackingData = await obaDataAccess.GetTrackingDataForStopAsync(favorite.Stop, token);

                    // Adds the tracking data to the list:
                    favoritesRealTimeData.AddRange(from data in trackingData
                                                   where string.Equals(favorite.Route, data.RouteId, StringComparison.OrdinalIgnoreCase)
                                                   select data);
                }

                // Now it's time to update the main tile with data:
                TileXMLBuilder mainTileBuilder = new TileXMLBuilder();
                AppendTrackingDataToTile(mainTileBuilder, favoritesRealTimeData);

                // And now we can update the secondary tiles!
                var pinnedStopTiles = await SecondaryTile.FindAllAsync();
                foreach (var pinnedStopTile in pinnedStopTiles)
                {
                    token.ThrowIfCancellationRequested();
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
                            var obaDataAccess = ObaDataAccess.Create(lat, lon);
                            TrackingData[] trackingData = await obaDataAccess.GetTrackingDataForStopAsync(stopId, token);

                            TileXMLBuilder secondaryTileBuilder = new TileXMLBuilder(pinnedStopTile.TileId);

                            await secondaryTileBuilder.AppendTileWithLargePictureAndTextAsync(
                                    pinnedStopTile.TileId,
                                    lat,
                                    lon,
                                    pinnedStopTile.DisplayName);
                            
                            AppendTrackingDataToTile(secondaryTileBuilder, trackingData);
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
        private static void AppendTrackingDataToTile(TileXMLBuilder tileBuilder, IEnumerable<TrackingData> unorderedTrackingData)
        {
            // Append 15 minutes worth of scheduled tile notifications:
            DateTime time = DateTime.Now;
            tileBuilder.EnableNotificationQueue();

            for (int i = 0; i < 15; i++)
            {
                var orderedTrackingData = (from rtd in unorderedTrackingData
                                           where !rtd.IsNoData && rtd.PredictedArrivalTime > time
                                           orderby rtd.PredictedArrivalInMinutes ascending
                                           select rtd).Take(5).ToList();
                
                foreach (TrackingData trackingData in orderedTrackingData)
                {
                    tileBuilder.AppendTileWithBlockTextAndLines(
                        time,
                        (trackingData.PredictedArrivalTime - time).Minutes.ToString(),
                        trackingData.Status,
                        string.Format("BUS {0}", trackingData.Route.ShortName.ToUpper()),
                        trackingData.TripHeadsign.ToUpper(),
                        trackingData.StopName.ToUpper(),
                        trackingData.ScheduledArrivalTime.ToString("h:mm"),
                        trackingData.PredictedArrivalTime.ToString("h:mm"));
                }

                time = time.AddMinutes(1);
            }
        }
    }
}