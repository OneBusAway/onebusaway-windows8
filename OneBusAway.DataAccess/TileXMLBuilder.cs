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
        /// Only enable the notification queue if we have more than 1 update to show.
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
            string wideTileImageFile = await CreateTileMapImageAsync(tileId, latitude, longitude, 310, 160, true);

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
            rootElement.AppendChild(visualElement);

            XmlElement wideBindingElement = document.CreateElement("binding");
            wideBindingElement.SetAttribute("template", "TileWideImageAndText01");
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
            string smallTileImageFile = await CreateTileMapImageAsync(tileId, latitude, longitude, 160, 160, false);
            
            XmlElement smallBindingElement = document.CreateElement("binding");
            smallBindingElement.SetAttribute("template", "TileSquarePeekImageAndText04");
            visualElement.AppendChild(smallBindingElement);

            XmlElement smallImageElement = document.CreateElement("image");
            smallImageElement.SetAttribute("id", "1");
            smallImageElement.SetAttribute("src", string.Format(@"ms-appdata:///local/Tiles/{0}", smallTileImageFile));
            smallImageElement.SetAttribute("alt", text);
            smallBindingElement.AppendChild(smallImageElement);

            XmlElement smallTextElement = document.CreateElement("text");
            smallTextElement.SetAttribute("id", "1");
            smallTextElement.InnerText = text;
            smallBindingElement.AppendChild(smallTextElement);

            TileNotification notification = new TileNotification(document);
            this.tileUpdater.Update(notification);
        }

        /// <summary>
        /// Appends a wide tile with a big block of text. Used to display up-coming buses.
        /// </summary>
        public void AppendWideTileWithBlockTextAndLines(string blockText, string subBlockText, string text1 = null, string text2 = null, string text3 = null, string text4 = null)
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
            rootElement.AppendChild(visualElement);

            XmlElement bindingElement = document.CreateElement("binding");
            bindingElement.SetAttribute("template", "TileWideBlockAndText01");
            visualElement.AppendChild(bindingElement);

            string[] texts = new string[]
            {
                text1,
                text2,
                text3,
                text4,
                blockText,
                subBlockText,
            };

            for (int i = 0; i < texts.Length; i++)
            {
                string text = texts[i];
                if (!string.IsNullOrEmpty(text))
                {
                    XmlElement textElement = document.CreateElement("text");
                    textElement.SetAttribute("id", (i + 1).ToString());
                    textElement.InnerText = text;
                    bindingElement.AppendChild(textElement);
                }
            }

            TileNotification notification = new TileNotification(document);
            notification.ExpirationTime = DateTime.Now.AddMinutes(1);
            this.tileUpdater.Update(notification);
        }

        /// <summary>
        /// Creates a map image for a tile.
        /// </summary>
        private static async Task<string> CreateTileMapImageAsync(string tileId, double latitude, double longitude, int width, int height, bool isWide)
        {
            // Get the image from Bing and save to disk.
            string tileImageFile = string.Format("{0}-{1}.png", tileId, isWide ? "wide" : "small");
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
