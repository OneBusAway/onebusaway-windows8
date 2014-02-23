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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OneBusAway.PageControls;
using OneBusAway.Services;

namespace OneBusAway.Platforms.WindowsPhone
{
    public class TileService : ITileService
    {
        public Task<bool> PageControlIsCurrentlyPinned(IPageControl pageControl)
        {
            return Task.FromResult<bool>(false);
        }

        public Task<bool> PinSecondaryTileAsync(IPinablePageControl pinnablePageControl)
        {
            return Task.FromResult<bool>(false);
        }

        public Task<bool> UnPinSecondaryTileAsync(IPinablePageControl pinnablePageControl)
        {
            return Task.FromResult<bool>(false);
        }
    }
}
