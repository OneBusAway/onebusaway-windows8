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

using OneBusAway.PageControls;
using OneBusAway.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.StartScreen;

namespace OneBusAway.Platforms.Windows8
{
    public class TileService : ITileService
    {
        public TileService()
        {
        }

        /// <summary>
        /// Returns true if the page control is currently pinned.
        /// </summary>
        public async Task<bool> PageControlIsCurrentlyPinned(IPageControl pageControl)
        {
            IPinablePageControl pinnablePageControl = pageControl as IPinablePageControl;
            if (pinnablePageControl != null)
            {
                // See if we're pinned to start:
                var query = from tile in await SecondaryTile.FindAllAsync()
                            where string.Equals(pinnablePageControl.TileId, tile.TileId, StringComparison.OrdinalIgnoreCase)
                            select tile;

                return query.Count() > 0;
            }

            return false;
        }

        /// <summary>
        /// Pins a secondary tile to the start screen.
        /// </summary>
        public async Task<bool> PinSecondaryTileAsync(IPinablePageControl pinnablePageControl)
        {
            Uri logoUri = new Uri("ms-appx:///Assets/Logo.scale-100.png");
            Uri smallLogoUri = new Uri("ms-appx:///Assets/SmallLogo.scale-100.png");
            Uri wideLogoUri = new Uri("ms-appx:///Assets/WideLogo.scale-100.png");
            Uri largeLogoUri = new Uri("ms-appx:///Assets/Square310x310Logo.scale-100.png");

            SecondaryTile secondaryTile = new SecondaryTile();
            secondaryTile.TileId = pinnablePageControl.TileId;
            secondaryTile.DisplayName = pinnablePageControl.TileName;
            secondaryTile.Arguments = pinnablePageControl.GetParameters().ToQueryString();

            secondaryTile.VisualElements.ShowNameOnSquare150x150Logo = true;
            secondaryTile.VisualElements.ShowNameOnWide310x150Logo = true;
            secondaryTile.VisualElements.ShowNameOnSquare310x310Logo = true;
            secondaryTile.VisualElements.Square30x30Logo = smallLogoUri;
            secondaryTile.VisualElements.Square150x150Logo = logoUri;
            secondaryTile.VisualElements.Wide310x150Logo = wideLogoUri;
            secondaryTile.VisualElements.Square310x310Logo = largeLogoUri;
            secondaryTile.VisualElements.ForegroundText = ForegroundText.Light;

            if (await secondaryTile.RequestCreateAsync())
            {
                try
                {
                    await pinnablePageControl.UpdateTileAsync(true);
                    
                }
                catch
                {
                    // In case of network error, don't bring down the app. Nothing we can do here...
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Unpins a secondary tile to the start screen.
        /// </summary>
        public async Task<bool> UnPinSecondaryTileAsync(IPinablePageControl pinnablePageControl)
        {
            var secondaryTile = (from tile in await SecondaryTile.FindAllAsync()
                                 where string.Equals(pinnablePageControl.TileId, tile.TileId, StringComparison.OrdinalIgnoreCase)
                                 select tile).FirstOrDefault();

            if (secondaryTile != null)
            {
                if (await secondaryTile.RequestDeleteAsync())
                {
                    await pinnablePageControl.UpdateTileAsync(false);
                    return true;
                }
            }

            return false;
        }
    }
}
