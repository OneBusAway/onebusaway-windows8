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
