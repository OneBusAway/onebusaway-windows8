/* Copyright 2014 Michael Braude and individual contributors.
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

using OneBusAway.DataAccess.BingService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.Storage;
using Windows.UI.Notifications;

namespace OneBusAway.DataAccess
{
    /// <summary>
    /// This class helps build the XmlDocument that is used to update tiles.
    /// </summary>
    public class TileXMLBuilder
    {
        /// <summary>
        /// The updater for the tile.
        /// </summary>
        private TileUpdater tileUpdater;

        /// <summary>
        /// The id of the tile updater.
        /// </summary>
        private string tileId;

        /// <summary>
        /// Creates a tile xml builder for the default tile.
        /// </summary>
        public TileXMLBuilder()
            : this(null)
        {
        }

        /// <summary>
        /// Creates the builder.
        /// </summary>
        public TileXMLBuilder(string tileId)
        {
            // Now we can update the tile:
            this.tileId = tileId;
            this.tileUpdater = (string.IsNullOrEmpty(this.tileId))
                ? TileUpdateManager.CreateTileUpdaterForApplication()
                : TileUpdateManager.CreateTileUpdaterForSecondaryTile(this.tileId);
        }

        /// <summary>
        /// Returns true if the is tile updater is for the main application or not.
        /// </summary>
        public bool IsMainTileUpdater
        {
            get
            {
                return string.IsNullOrEmpty(this.tileId);
            }
        }

        /// <summary>
        /// Enables the notification queue.
        /// </summary>
        public void EnableNotificationQueue()
        {
            this.tileUpdater.EnableNotificationQueue(true);
        }

        /// <summary>
        /// Appends TileWideImageAndText01 to the document.
        /// </summary>
        public async Task AppendTileWithLargePictureAndTextAsync(string tileId, double latitude, double longitude, string text)
        {
            string largeTileImageFile = await CreateTileMapImageAsync(tileId, latitude, longitude, 310, 310, TileSize.large);

            //<visual>
            //  <binding template="TileWideImageAndText01">
            //    <image id="1" src="image1.png" alt="alt text"/>
            //    <text id="1">Text Field 1</text>
            //  </binding>  
            //</visual>
            var document = new XmlDocument();
            var rootElement = document.CreateElement("tile");
            document.AppendChild(rootElement);

            XmlElement visualElement = document.CreateElement("visual");
            visualElement.SetAttribute("version", "2");
            rootElement.AppendChild(visualElement);

            XmlElement largeBindingElement = document.CreateElement("binding");
            largeBindingElement.SetAttribute("template", "TileSquare310x310ImageAndText01");
            visualElement.AppendChild(largeBindingElement);

            XmlElement largeImageElement = document.CreateElement("image");
            largeImageElement.SetAttribute("id", "1");
            largeImageElement.SetAttribute("src", string.Format(@"ms-appdata:///local/Tiles/{0}", largeTileImageFile));
            largeImageElement.SetAttribute("alt", text);
            largeBindingElement.AppendChild(largeImageElement);

            XmlElement largeTextElement = document.CreateElement("text");
            largeTextElement.SetAttribute("id", "1");
            largeTextElement.InnerText = text;
            largeBindingElement.AppendChild(largeTextElement);

            string wideTileImageFile = await CreateTileMapImageAsync(tileId, latitude, longitude, 310, 160, TileSize.wide);
            
            XmlElement wideBindingElement = document.CreateElement("binding");
            wideBindingElement.SetAttribute("template", "TileWide310x150ImageAndText01");
            wideBindingElement.SetAttribute("fallback", "TileWideImageAndText01");
            visualElement.AppendChild(wideBindingElement);

            XmlElement wideImageElement = document.CreateElement("image");
            wideImageElement.SetAttribute("id", "1");
            wideImageElement.SetAttribute("src", string.Format(@"ms-appdata:///local/Tiles/{0}", wideTileImageFile));
            wideImageElement.SetAttribute("alt", text);
            wideBindingElement.AppendChild(wideImageElement);

            XmlElement wideTextElement = document.CreateElement("text");
            wideTextElement.SetAttribute("id", "1");
            wideTextElement.InnerText = text;
            wideBindingElement.AppendChild(wideTextElement);

            // Also add the small tile image:
            //<tile>
            //  <visual>
            //    <binding template="TileSquareImage">
            //      <image id="1" src="image1" alt="alt text"/>
            //    </binding>  
            //  </visual>
            //</tile>
            string smallTileImageFile = await CreateTileMapImageAsync(tileId, latitude, longitude, 160, 160, TileSize.small);
            
            XmlElement smallBindingElement = document.CreateElement("binding");
            smallBindingElement.SetAttribute("template", "TileSquare150x150Image");
            smallBindingElement.SetAttribute("fallback", "TileSquareImage");
            visualElement.AppendChild(smallBindingElement);

            XmlElement smallImageElement = document.CreateElement("image");
            smallImageElement.SetAttribute("id", "1");
            smallImageElement.SetAttribute("src", string.Format(@"ms-appdata:///local/Tiles/{0}", smallTileImageFile));
            smallImageElement.SetAttribute("alt", text);
            smallBindingElement.AppendChild(smallImageElement);

            TileNotification notification = new TileNotification(document);
            notification.Tag = tileId.GetHashCode().ToString("X");
            this.tileUpdater.Update(notification);
        }

        /// <summary>
        /// Appends a wide tile with a big block of text. Used to display up-coming buses.
        /// </summary>
        public void AppendTileWithBlockTextAndLines(DateTimeOffset scheduledTime, string blockText, string statusText, string busName, string tripHeadsign, string stopName, string scheduledArrivalTime, string predictedArrivalTime)
        {
            //<tile>
            //  <visual>
            //    <binding template="TileWideBlockAndText01">
            //      <text id="1">Text Field 1</text>
            //      <text id="2">Text Field 2</text>
            //      <text id="3">Text Field 3</text>
            //      <text id="4">Text Field 4</text>
            //      <text id="5">T5</text>
            //      <text id="6">Text Field 6</text>
            //    </binding>  
            //  </visual>
            //</tile>
            var document = new XmlDocument();
            var rootElement = document.CreateElement("tile");
            document.AppendChild(rootElement);

            XmlElement visualElement = document.CreateElement("visual");
            visualElement.SetAttribute("version", "2");
            rootElement.AppendChild(visualElement);

            // Support large tiles for Win 8.1 and higher:
            //<visual version="2">
            //  <binding template="TileSquare310x310BlockAndText01">
            //    <text id="1">Text Field 1 (large text)</text>
            //    <text id="2">Text Field 2</text>
            //    <text id="3">Text Field 3</text>
            //    <text id="4">Text Field 4</text>
            //    <text id="5">Text Field 5</text>
            //    <text id="6">Text Field 6</text>
            //    <text id="7">Text Field 7</text>
            //    <text id="8">Text Field 8 (block text)</text>
            //    <text id="9">Text Field 9</text>
            //  </binding>  
            //</visual>
            XmlElement largeBindingElement = document.CreateElement("binding");
            largeBindingElement.SetAttribute("template", "TileSquare310x310BlockAndText01");

            AddSubTextElements(largeBindingElement, new string[]
            {
                busName,
                tripHeadsign,
                stopName,
                "SCHED / ETA",
                string.Format("{0} / {1}", scheduledArrivalTime, predictedArrivalTime),
                string.Empty,
                string.Empty,
                blockText,
                statusText,
            });

            visualElement.AppendChild(largeBindingElement);


            XmlElement wideBindingElement = document.CreateElement("binding");            
            wideBindingElement.SetAttribute("template", "TileWide310x150BlockAndText01");
            wideBindingElement.SetAttribute("fallback", "TileWideBlockAndText01");
            visualElement.AppendChild(wideBindingElement);

            AddSubTextElements(wideBindingElement, new string[]
            {
                busName,
                tripHeadsign,
                stopName,
                string.Format("{0} / {1}", scheduledArrivalTime, predictedArrivalTime),
                blockText,
                statusText,
            });

            // Unfortunately, there is only one small tile template that supports block text.
            //<tile>
            //  <visual>
            //    <binding template="TileSquareBlock">
            //      <text id="1">Text Field 1</text>
            //      <text id="2">Text Field 2</text>
            //    </binding>  
            //  </visual>
            //</tile>
            XmlElement smallBindingElement = document.CreateElement("binding");
            smallBindingElement.SetAttribute("template", "TileSquare150x150Block");
            smallBindingElement.SetAttribute("fallback", "TileSquareBlock");
            visualElement.AppendChild(smallBindingElement);

            XmlElement smallBlockTextElement = document.CreateElement("text");
            smallBlockTextElement.SetAttribute("id", "1");
            smallBlockTextElement.InnerText = blockText;
            smallBindingElement.AppendChild(smallBlockTextElement);

            XmlElement smallSubTextElement = document.CreateElement("text");
            smallSubTextElement.SetAttribute("id", "2");
            smallSubTextElement.InnerText = busName;
            smallBindingElement.AppendChild(smallSubTextElement);

            if ((scheduledTime - DateTime.Now).TotalMinutes < 1)
            {
                var notification = new TileNotification(document);
                notification.ExpirationTime = scheduledTime.AddMinutes(1);
                notification.Tag = (busName + tripHeadsign + stopName + scheduledTime.ToString("hh:mm")).GetHashCode().ToString("X");
                this.tileUpdater.Update(notification);
            }
            else
            {
                var notification = new ScheduledTileNotification(document, scheduledTime);
                notification.ExpirationTime = scheduledTime.AddMinutes(1);
                notification.Tag = (busName + tripHeadsign + stopName + scheduledTime.ToString("hh:mm")).GetHashCode().ToString("X");
                this.tileUpdater.AddToSchedule(notification);
            }
        }

        /// <summary>
        /// Private utility method that adds text elements to a binding element.
        /// </summary>
        private static void AddSubTextElements(XmlElement bindingElement, string[] texts)
        {
            for (int i = 0; i < texts.Length; i++)
            {
                string text = texts[i];
                if (!string.IsNullOrEmpty(text))
                {
                    XmlElement textElement = bindingElement.OwnerDocument.CreateElement("text");
                    textElement.SetAttribute("id", (i + 1).ToString());
                    textElement.InnerText = text;
                    bindingElement.AppendChild(textElement);
                }
            }
        }

        private enum TileSize 
        { 
            large,
            wide,
            small
        }

        /// <summary>
        /// Creates a map image for a tile.
        /// </summary>
        private static async Task<string> CreateTileMapImageAsync(string tileId, double latitude, double longitude, int width, int height, TileSize tileSize)
        {
            // Get the image from Bing and save to disk.
            string tileImageFile = string.Format("{0}-{1}.png", tileId, tileSize.ToString());
            string tileImagePath = Path.Combine("Tiles", tileImageFile);

            bool fileExists = false;
            try
            {
                var file = await ApplicationData.Current.LocalFolder.GetFileAsync(tileImagePath);
                fileExists = true;
            }
            catch
            {
            }

            // Create the file:
            if (!fileExists)
            {
                BingMapsServiceHelper bingMapsHelper = new BingMapsServiceHelper();
                using (var memoryStream = await bingMapsHelper.GetStaticImageBytesAsync(
                    latitude,
                    longitude,
                    width,
                    height))
                {
                    IStorageFile storageFile = await ApplicationData.Current.LocalFolder.CreateFileAsync(tileImagePath, CreationCollisionOption.ReplaceExisting);
                    using (var stream = await storageFile.OpenStreamForWriteAsync())
                    {
                        memoryStream.WriteTo(stream);
                    }
                }
            }

            return tileImageFile;
        }
    }
}
