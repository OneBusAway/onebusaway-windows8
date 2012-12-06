using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneBusAway.ViewModels
{
    /// <summary>
    /// View model for the map control
    /// </summary>
    public class MapControlViewModel : ViewModelBase
    {
        private OneBusAway.Model.Point mapCenter;
        private double zoomLevel = ViewModelConstants.DefaultMapZoom;
        private OneBusAway.Model.Point userLocation;

        /// <summary>
        /// Creates the map control view model.
        /// </summary>
        public MapControlViewModel()
        {
        }

        public OneBusAway.Model.Point MapCenter
        {
            get
            {
                return mapCenter;
            }
            set
            {
                SetProperty(ref this.mapCenter, value);
            }
        }

        public double ZoomLevel
        {
            get
            {
                return zoomLevel;
            }
            set
            {
                SetProperty(ref this.zoomLevel, value);
            }
        }

        public OneBusAway.Model.Point UserLocation
        {
            get
            {
                return userLocation;
            }
            set
            {
                SetProperty(ref this.userLocation, value);
            }
        }

        public void ResetZoomLevel()
        {
            this.ZoomLevel = ViewModelConstants.DefaultMapZoom;
        }
    }
}
