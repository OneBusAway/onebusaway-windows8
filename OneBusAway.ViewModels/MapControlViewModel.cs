using OneBusAway.DataAccess;
using OneBusAway.Model;
using OneBusAway.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace OneBusAway.ViewModels
{
    /// <summary>
    /// View model for the map control
    /// </summary>
    public class MapControlViewModel : ViewModelBase
    {
        private ObaDataAccess obaDataAccess;
        private OneBusAway.Model.Point userLocation;
        private MapView mapView;
        private List<Stop> busStops;
        private List<Shape> shapes;
        private bool refreshBusStopsOnMapViewChanged;
        private BusStopControlViewModel selectedBusStop;

        public event EventHandler<StopSelectedEventArgs> StopSelected;

        /// <summary>
        /// Creates the map control view model.
        /// </summary>
        public MapControlViewModel()
        {
            this.mapView = MapView.Current;
            this.obaDataAccess = new ObaDataAccess();
            this.RefreshBusStopsOnMapViewChanged = true;
        }

        /// <summary>
        /// When true, the control will refresh bus stops when the map moves. When false, it will use a static list.
        /// </summary>
        public bool RefreshBusStopsOnMapViewChanged
        {
            get
            {
                return this.refreshBusStopsOnMapViewChanged;
            }
            set
            {
                SetProperty(ref this.refreshBusStopsOnMapViewChanged, value);
            }
        }

        /// <summary>
        /// Gets / sets the selected bus stop.
        /// </summary>
        public BusStopControlViewModel SelectedBusStop
        {
            get
            {
                return this.selectedBusStop;
            }
            set
            {
                SetProperty(ref this.selectedBusStop, value);
            }
        }

        /// <summary>
        /// Gets an object that describes the state of the map at any point of time. This property should be bound to the MapControl's MapView property in xaml.
        /// This property is updated whenever the map view changes (either zoom or the center of the map)
        /// </summary>
        public MapView MapView
        {
            get
            {
                return mapView;
            }
            set
            {
                SetProperty(ref mapView, value);
                MapView.Current = value;

                if (this.RefreshBusStopsOnMapViewChanged && value.ZoomLevel > UtilitiesConstants.MinBusStopVisibleZoom)
                {
                    RefreshStopsForLocationAsync();
                }
            }
        }

        public List<Stop> BusStops
        {
            get
            {
                return busStops;
            }
            set
            {
                SetProperty(ref busStops, value);
            }
        }

        public List<Shape> Shapes
        {
            get
            {
                return this.shapes;
            }
            set
            {
                SetProperty(ref this.shapes, value);
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

        /// <summary>
        /// Finds the users location asynchronously using the Geolocator.
        /// </summary>
        public async Task FindUserLocationAsync()
        {
            Geolocator geolocator = new Geolocator();
            var position = await geolocator.GetGeopositionAsync();

            var userLocation = new Point(position.Coordinate.Latitude, position.Coordinate.Longitude);
            
            this.UserLocation = userLocation;
            this.MapView = new MapView(userLocation, ViewModelConstants.DefaultMapZoom);
        }

        public async void RefreshStopsForLocationAsync()
        {
            try
            {
                var output = await this.obaDataAccess.GetStopsForLocationAsync(mapView.MapCenter.Latitude, mapView.MapCenter.Longitude, mapView.BoundsHeight, mapView.BoundsWidth);

                BusStops = output.ToList();
            }
            catch (Exception)
            {
                // TODO
            }
        }
        
        /// <summary>
        /// Called when a stop is selected.
        /// </summary>
        public void SelectStop(BusStopControlViewModel busStopViewModel)
        {
            var stopSelected = this.StopSelected;
            if (stopSelected != null)
            {
                this.SelectedBusStop = busStopViewModel;
                stopSelected(this, new StopSelectedEventArgs(busStopViewModel.StopName, 
                    busStopViewModel.StopId,
                    busStopViewModel.Direction));
            }
        }

        /// <summary>
        /// Selects a bus stop by a stop object. To do this we need to go through all
        /// of the map controls and find the one whose view model stop id matches the stop.
        /// </summary>
        public void SelectStop(Stop stop)
        {
            this.SelectedBusStop = new BusStopControlViewModel(this, stop);
        }

        /// <summary>
        /// Selects a stop based on the stop Id.
        /// </summary>
        public void SelectStop(string stopId)
        {
            // Find the selected bus stop:
            var selectedStop = (from busStop in this.busStops
                                where string.Equals(stopId, busStop.StopId, StringComparison.OrdinalIgnoreCase)
                                select busStop).FirstOrDefault();

            if (selectedStop != null)
            {
                SelectStop(selectedStop);
            }
        }

        /// <summary>
        /// Finds the shape of a route by its id and headsign and then displays it.
        /// </summary>
        public async Task FindRouteShapeAsync(string routeId, string tripHeadsign)
        {
            var routeData = await this.obaDataAccess.GetRouteDataAsync(routeId, tripHeadsign);
            this.Shapes = routeData.Shapes.ToList();
        }
    }
}
