using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneBusAway.PageControls
{
    /// <summary>
    /// Defines a page control that can pin to the start screen.
    /// </summary>
    public interface IPinablePageControl : IPageControl
    {
        string TileId
        {
            get;
        }

        string TileName
        {
            get;
        }

        Task UpdateTileAsync(bool added);
    }
}
