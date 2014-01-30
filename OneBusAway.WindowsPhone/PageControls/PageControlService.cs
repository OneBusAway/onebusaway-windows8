using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OneBusAway.PageControls;
using OneBusAway.Services;

namespace OneBusAway.WindowsPhone.PageControls
{
    public class PageControlService : IPageControlService
    {
        public PageControlService()
        {
        }

        /// <summary>
        /// Returns a page control for a given type.
        /// </summary>
        public IPageControl CreatePageControl(PageControlTypes pageControlType)
        {
            switch (pageControlType)
            {
                case PageControlTypes.Favorites:
                    return new FavoritesPageControl();

                case PageControlTypes.RealTime:
                    return new RealTimePageControl();

                case PageControlTypes.TimeTable:
                    return new TimeTablePageControl();

                case PageControlTypes.TripDetails:
                    return new TripDetailsPageControl();

                default:
                    throw new NotSupportedException();
            }            
        }
    }
}
