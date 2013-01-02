using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneBusAway.ViewModels
{
    /// <summary>
    /// View model for hte stop and routes page.
    /// </summary>
    public class StopAndRoutesPageViewModel : PageViewModelBase
    {
        /// <summary>
        /// View model for the map.
        /// </summary>
        private MapControlViewModel mapControlViewModel;

        /// <summary>
        /// View model for the route timeline control.
        /// </summary>
        private RouteTimelineControlViewModel routeTimelineControlViewModel;

        /// <summary>
        /// Creates the view model.
        /// </summary>
        public StopAndRoutesPageViewModel()
        {
            this.HeaderViewModel.SubText = "ROUTE MAP";
            this.MapControlViewModel = new MapControlViewModel();
            this.RouteTimelineControlViewModel = new RouteTimelineControlViewModel();
        }

        /// <summary>
        /// Returns the map control view model.
        /// </summary>
        public MapControlViewModel MapControlViewModel
        {
            get
            {
                return this.mapControlViewModel;
            }
            set
            {
                SetProperty(ref this.mapControlViewModel, value);
            }
        }

        /// <summary>
        /// Returns the route timeline control view model.
        /// </summary>
        public RouteTimelineControlViewModel RouteTimelineControlViewModel
        {
            get
            {
                return this.routeTimelineControlViewModel;
            }
            set
            {
                SetProperty(ref this.routeTimelineControlViewModel, value);
            }
        }
    }
}
