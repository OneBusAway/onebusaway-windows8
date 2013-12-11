/* Copyright 2013 Michael Braude and individual contributors.
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
using OneBusAway.DataAccess;
using OneBusAway.DataAccess.ObaService;
using OneBusAway.Model;
using OneBusAway.Shared.Services;
using OneBusAway.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneBusAway.ViewModels.Controls
{
    /// <summary>
    /// View model for the map control
    /// </summary>
    public class MapControlViewModel : ViewModelBase
    {
        private OneBusAway.Model.Point userLocation;
        private MapView mapView;
        private BusStopList busStops;        
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
            this.selectedBusStop = other.selectedBusStop;
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

        public BusStopList BusStops
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
                this.UserLocation = MapView.Current.MapCenter;
                this.MapView = new MapView(this.UserLocation, ViewModelConstants.DefaultMapZoom);

                try
                {
                    var position = await ServiceRepository.GeoLocationService.FindUserLocationAsync();
                    this.UserLocation = new Point(position.Latitude, position.Longitude);
                    this.MapView = new MapView(this.UserLocation, ViewModelConstants.ZoomedInMapZoom);
                }
                catch
                {
                    // geolocator failed for some other reason.
                    // [Ghulam] Apparently on server 2102 GetGeopositionAsync throws FileNotFoundException.
                }
            }
        }

        /// <summary>
        /// Finds the users location asynchronously using the Geolocator.
        /// </summary>
        public async Task<bool> TryFindUserLocationAsync()
        {
            try
            {
                var position = await ServiceRepository.GeoLocationService.FindUserLocationAsync();

                this.UserLocation = new Point(position.Latitude, position.Longitude);
                this.MapView = new MapView(this.UserLocation, this.MapView.ZoomLevel, true);
                return true;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
            catch (Exception)
            {
                // geolocator failed for some other reason.
                // [Ghulam] Apparently on server 2102 GetGeopositionAsync throws FileNotFoundException.
                return false;
            }
        }

        /// <summary>
        /// Finds the closest stop in the bus stop list.
        /// </summary>
        public Stop TryFindClosestStop()
        {
            return (from stop in this.busStops
                    where stop.IsClosestStop
                    select stop).FirstOrDefault();
        }

        /// <summary>
        /// Unselects the closest stop.
        /// </summary>
        public void UnselectClosestStop()
        {
            this.SelectClosestStop(null);
        }

        /// <summary>
        /// Selects the closest stop.
        /// </summary>
        public void SelectClosestStop(string closestStopId)
        {
            var closestStop = this.TryFindClosestStop();
            if (closestStop != null)
            {
                closestStop.IsClosestStop = false;
            }

            foreach (var busStop in this.BusStops)
            {
                busStop.IsClosestStop = !string.IsNullOrEmpty(closestStopId) && string.Equals(closestStopId, busStop.StopId, StringComparison.OrdinalIgnoreCase);
            }
        }
        
        public async Task RefreshStopsForLocationAsync()
        {
            this.BusStops = new BusStopList(await ObaDataAccess.Create().GetStopsForLocationAsync(
                mapView.MapCenter.Latitude,
                mapView.MapCenter.Longitude,
                mapView.BoundsHeight,
                mapView.BoundsWidth));
        }

        /// <summary>
        /// Sets the map view and refreshes stops around that view asynchronously.
        /// </summary>
        public async Task RefreshStopsForLocationAsync(MapView mapView)
        {
            SetProperty(ref this.mapView, mapView);
            MapView.Current = mapView;

            this.BusStops = new BusStopList(await ObaDataAccess.Create().GetStopsForLocationAsync(mapView.MapCenter.Latitude, mapView.MapCenter.Longitude));
        }

        /// <summary>
        /// Unselects the currently selected stop.
        /// </summary>
        public void UnSelectStop()
        {
            if (this.SelectedBusStop != null)
            {
                this.SelectedBusStop.IsSelected = false;
                this.SelectedBusStop.ZIndex = 0;
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
                this.UnSelectStop();
                this.SelectedBusStop = busStopViewModel;
                this.SelectedBusStop.ZIndex = 100;

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
        public void SelectStop(Stop stop, bool zoomToStop = false)
        {
            this.SelectedBusStop = new BusStopControlViewModel(stop);
            if (zoomToStop)
            {
                this.MapView = new MapView(new Point(stop.Latitude, stop.Longitude), this.mapView.ZoomLevel, true);
            }
        }

        /// <summary>
        /// Selects a stop based on the stop Id.
        /// </summary>
        public void SelectStop(string stopId, bool zoomToStop = false)
        {
            // Find the selected bus stop:
            var selectedStop = (from busStop in this.busStops
                                where string.Equals(stopId, busStop.StopId, StringComparison.OrdinalIgnoreCase)
                                select busStop).FirstOrDefault();

            if (selectedStop != null)
            {
                this.SelectStop(selectedStop, zoomToStop);
            }
        }

        /// <summary>
        /// Finds the shape of a route by its id and current stopId and then displays it.
        /// </summary>
        public async Task FindRouteShapeAsync(string routeId, string stopId)
        {
            var routeData = await ObaDataAccess.Create().GetRouteDataAsync(routeId, stopId);
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