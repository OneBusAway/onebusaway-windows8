using OneBusAway.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneBusAway.ViewModels
{
    public class RouteMapPageViewModel : PageViewModelBase
    {
        public RouteMapPageViewModel()
        {

        }

        private Point _MapCenter;
        public Point MapCenter
        {
            get
            {
                if (_MapCenter == null)
                {
                    _MapCenter = new Point() { Latitude = 47.648195, Longitude = -122.145286 };
                }
                return _MapCenter;
            }
            set
            {
                SetProperty(ref _MapCenter, value);
            }
        }
    }
}
