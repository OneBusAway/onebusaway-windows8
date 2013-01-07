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
        /// Copies data from another map control view model.
        /// </summary>
        public void CopyFrom(MapControlViewModel other)
        {
            this.userLocation = other.userLocation;
            this.busStops = other.busStops;
            this.mapView = other.mapView;
            this.shapes = other.shapes;
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
                    var ignored = RefreshStopsForLocationAsync();
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
            // If we already have a location, don't get it again:
            if (this.UserLocation == null)
            {
                try
                {
                    Geolocator geolocator = new Geolocator();
                    var position = await geolocator.GetGeopositionAsync();

                    var userLocation = new Point(position.Coordinate.Latitude, position.Coordinate.Longitude);

                    this.UserLocation = userLocation;
                    this.MapView = new MapView(userLocation, ViewModelConstants.DefaultMapZoom);
                }
                catch (UnauthorizedAccessException)
                {
                    // the user didn't give us permission to use their location. Nothing we can do here
                    // so just zoom out to show the whole freakin' world =)
                }
            }
        }

        public async Task RefreshStopsForLocationAsync()
        {
            this.BusStops = (await this.obaDataAccess.GetStopsForLocationAsync(
                mapView.MapCenter.Latitude,
                mapView.MapCenter.Longitude,
                mapView.BoundsHeight,
                mapView.BoundsWidth)).ToList();
        }

        /// <summary>
        /// Sets the map view and refreshes stops around that view asynchronously.
        /// </summary>
        public async Task RefreshStopsForLocationAsync(MapView mapView)
        {
            SetProperty(ref this.mapView, mapView);
            MapView.Current = mapView;

            this.BusStops = (await this.obaDataAccess.GetStopsForLocationAsync(mapView.MapCenter.Latitude, mapView.MapCenter.Longitude)).ToList();
        }

        /// <summary>
        /// Unselects the currently selected stop.
        /// </summary>
        public void UnSelectStop()
        {
            if (this.SelectedBusStop != null)
            {
                this.SelectedBusStop.IsSelected = false;
                this.SelectedBusStop = null;
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
                    busStopViewModel.Direction,
                    busStopViewModel.Latitude,
                    busStopViewModel.Longitude));
            }
        }

        /// <summary>
        /// Selects a bus stop by a stop object. To do this we need to go through all
        /// of the map controls and find the one whose view model stop id matches the stop.
        /// </summary>
        public void SelectStop(Stop stop)
        {
            this.SelectedBusStop = new BusStopControlViewModel(stop);
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

        /// <summary>
        /// Zoomns the map to show the entire shape of the route.
        /// </summary>
        public void ZoomToRouteShape()
        {
            // No shapes set - there's nothing we can do here.
            if (this.shapes == null || this.shapes.Count == 0)
            {
                return;
            }

            double maxWest = double.MinValue;
            double maxEast = double.MaxValue;
            double maxNorth = double.MinValue;
            double maxSouth = double.MaxValue;

            foreach (var shape in this.shapes)
            {
                foreach (var point in shape.Points)
                {
                    if (maxWest < point.Longitude)
                    {
                        maxWest = point.Longitude;
                    }

                    if (maxEast > point.Longitude)
                    {
                        maxEast = point.Longitude;
                    }

                    if (maxNorth < point.Latitude)
                    {
                        maxNorth = point.Latitude;
                    }

                    if (maxSouth > point.Latitude)
                    {
                        maxSouth = point.Latitude;
                    }
                }
            }

            // Calculate the span of the view, plus a little fudge factor as a border:
            maxNorth += .01;
            maxSouth -= .01;
            maxWest += .01;
            maxEast -= .01;

            double northSouthSpan = (maxNorth - maxSouth);
            double eastWestSpan = (maxWest - maxEast);

            // Determine the origin:
            double originX = maxSouth + (northSouthSpan / 2.0);
            double originY = maxEast + (eastWestSpan / 2.0);

            this.MapView = new MapView(
                new Point(originX, originY),
                this.mapView.ZoomLevel,
                northSouthSpan,
                eastWestSpan,
                true);
        }
    }
}
