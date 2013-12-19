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
            throw new NotImplementedException();
        }
    }
}
