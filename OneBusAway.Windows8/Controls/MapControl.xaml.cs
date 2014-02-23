/* Copyright 2014 Michael Braude and individual contributors.
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

using System;
using System.Collections.Generic;
using System.Linq;
using Bing.Maps;
using OneBusAway.Model;
using OneBusAway.Platforms.Windows8;
using OneBusAway.Services;
using OneBusAway.ViewModels;
using OneBusAway.ViewModels.Controls;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace OneBusAway.Controls
{
    public partial class MapControl : UserControl
    {
        #region Member Variables
        
        private UserLocationIcon userLocationIcon;
        private bool centerOnUserLocation;
        private Location userLocation;
        private IUIHelper uiHelper;
        private HashSet<string> displayedBusStopLookup = new HashSet<string>();

        public static readonly DependencyProperty RefreshBusStopsOnMapViewChangedProperty = DependencyProperty.Register("RefreshBusStopsOnMapViewChanged", 
            typeof(bool),
            typeof(MapControl), 
            new PropertyMetadata(false));

        public static readonly DependencyProperty MapCenterProperty = DependencyProperty.Register("MapCenter", 
            typeof(OneBusAway.Model.Point), 
            typeof(MapControl), 
            new PropertyMetadata(null, MapCenterPropertyChanged));

        public static readonly DependencyProperty UserLocationProperty = DependencyProperty.Register("UserLocation", 
            typeof(OneBusAway.Model.Point),
            typeof(MapControl), 
            new PropertyMetadata(null, UserLocationChanged));

        public static readonly DependencyProperty CnterOnUserLocationProperty = DependencyProperty.Register("CenterOnUserLocation", 
            typeof(bool), 
            typeof(MapControl), 
            new PropertyMetadata(null, CenterOnUserLocationChanged));

        public static readonly DependencyProperty ZoomLevelProperty = DependencyProperty.Register("ZoomLevel", 
            typeof(double), 
            typeof(MapControl), 
            new PropertyMetadata(null, ZoomLevelChanged));

        public static readonly DependencyProperty BusStopsProperty = DependencyProperty.Register("BusStops", 
            typeof(BusStopList), 
            typeof(MapControl), 
            new PropertyMetadata(null, BusStopsChanged));

        public static readonly DependencyProperty BoundsWidthProperty = DependencyProperty.Register("BoundsWidth", 
            typeof(double), 
            typeof(MapControl), 
            null);

        public static readonly DependencyProperty BoundsHeightProperty = DependencyProperty.Register("BoundsHeight", 
            typeof(double), 
            typeof(MapControl),
            null);

        public static readonly DependencyProperty MapViewProperty = DependencyProperty.Register("MapView", 
            typeof(MapView), 
            typeof(MapControl), 
            new PropertyMetadata(null, MapViewChanged));

        public static readonly DependencyProperty ShapesProperty = DependencyProperty.Register("Shapes",
            typeof(List<OneBusAway.Model.Shape>), 
            typeof(MapControl), 
            new PropertyMetadata(null, ShapesChanged));

        public static readonly DependencyProperty ClearBusStopsOnZoomOutProperty = DependencyProperty.Register("ClearBusStopsOnZoomOut", 
            typeof(bool), 
            typeof(MapControl), 
            new PropertyMetadata(true));

        public static readonly DependencyProperty SelectedBusStopProperty = DependencyProperty.Register("SelectedBusStop", 
            typeof(BusStopControlViewModel), 
            typeof(MapControl), 
            new PropertyMetadata(null, new PropertyChangedCallback(SelectedBusStopChanged)));

        #endregion
        #region Constructor

        public MapControl()
        {
            this.InitializeComponent();

            this.userLocationIcon = new UserLocationIcon();

            // Setting MapCenter to null will center the map to Puget Sound and set the default Zoom level.
            this.MapCenter = null;

            map.ViewChangeEnded += OnMapViewChangeEnded;
            this.uiHelper = new DefaultUIHelper(this.Dispatcher);
        }

        #endregion
        #region Properties

        public OneBusAway.Model.Point MapCenter
        {
            get
            {
                return new OneBusAway.Model.Point()
                {
                    Latitude = map.Center.Latitude,
                    Longitude = map.Center.Longitude
                };
            }
            set
            {
                if (value != null)
                {
                    map.Center = new Location(value.Latitude, value.Longitude);
                    map.SetView(map.Center, ZoomLevel, new TimeSpan());
                }
                else
                {
                    map.Center = new Location(Constants.DefaultLatitude, Constants.DefaultLongitude);
                    map.SetView(map.Center, ZoomLevel, new TimeSpan());
                }
            }
        }

        public OneBusAway.Model.Point UserLocation
        {
            get
            {
                return GetValue(UserLocationProperty) as OneBusAway.Model.Point;
            }
            set
            {
                SetValue(UserLocationProperty, value);
            }
        }

        public bool RefreshBusStopsOnMapViewChanged
        {
            get
            {
                return (bool)GetValue(RefreshBusStopsOnMapViewChangedProperty);
            }
            set
            {
                SetValue(RefreshBusStopsOnMapViewChangedProperty, value);
            }
        }

        public bool CenterOnUserLocation
        {
            get
            {
                return centerOnUserLocation;
            }
            set
            {
                centerOnUserLocation = value;

                if (centerOnUserLocation && userLocation != null)
                {
                    map.SetView(userLocation, ZoomLevel);
                }
            }
        }

        public double ZoomLevel
        {
            get
            {
                return (double)GetValue(ZoomLevelProperty);
            }
            set
            {
                SetValue(ZoomLevelProperty, value);
            }
        }

        public BusStopList BusStops
        {
            get
            {
                return GetValue(BusStopsProperty) as BusStopList;
            }
            set
            {
                SetValue(BusStopsProperty, value);
            }
        }

        public List<Shape> Shapes
        {
            get
            {
                return GetValue(ShapesProperty) as List<Shape>;
            }
            set
            {
                SetValue(ShapesProperty, value);
            }
        }

        public MapView MapView
        {
            get
            {
                return GetValue(MapViewProperty) as MapView;
            }
            set
            {
                SetValue(MapViewProperty, value);
            }
        }

        public bool ClearBusStopsOnZoomOut
        {
            get
            {
                return (bool)GetValue(ClearBusStopsOnZoomOutProperty);
            }
            set
            {
                SetValue(ClearBusStopsOnZoomOutProperty, value);
            }
        }

        public BusStopControlViewModel SelectedBusStop
        {
            get
            {
                return GetValue(SelectedBusStopProperty) as BusStopControlViewModel;
            }
            set
            {
                SetValue(SelectedBusStopProperty, value);
            }
        }


        #endregion
        #region Callbacks

        private static void MapViewChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            var mapControl = dependencyObject as MapControl;
            var newValue = args.NewValue as MapView;

            if ((newValue == null) || 
                (newValue.MapCenter.Latitude == mapControl.map.Center.Latitude && newValue.MapCenter.Longitude == mapControl.map.Center.Longitude))
            {
                return;
            }

            var newLocation = new Location(newValue.MapCenter.Latitude, newValue.MapCenter.Longitude);

            if (newValue.AnimateChange)
            {
                if (newValue.BoundsWidth != 0 && newValue.BoundsHeight != 0)
                {
                    mapControl.map.SetView(new LocationRect(newLocation, newValue.BoundsWidth, newValue.BoundsHeight));
                    newValue.AnimateChange = false;
                }
                else
                {
                    mapControl.map.SetView(newLocation, newValue.ZoomLevel, MapAnimationDuration.Default);
                }

                newValue.AnimateChange = false;
            }
            else
            {
                mapControl.map.Center = newLocation;
                mapControl.map.ZoomLevel = newValue.ZoomLevel;
            }
        }

        /// <summary>
        /// Change the polylines on the map.
        /// </summary>
        private static void ShapesChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            MapControl mapControl = (MapControl)dependencyObject;

            if (args.OldValue != null)
            {
                // remove all old polylines from the map:
                mapControl.map.ShapeLayers.Clear();
            }

            if (args.NewValue != null)
            {
                MapShapeLayer routeLayer = new MapShapeLayer();

                List<Shape> shapes = (List<Shape>)args.NewValue;
                foreach (var shape in shapes)
                {
                    MapPolyline polyline = new MapPolyline();
                    polyline.Color = Color.FromArgb(255, 0x4f, 0x64, 0xba);
                    polyline.Width = 6;

                    LocationCollection locations = new LocationCollection();
                    foreach (var point in shape.Points)
                    {
                        locations.Add(new Location(point.Latitude, point.Longitude));
                    }

                    polyline.Locations = locations;
                    routeLayer.Shapes.Add(polyline);
                }

                mapControl.map.ShapeLayers.Add(routeLayer);
            }
        }

        private static void SelectedBusStopChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            BusStopControlViewModel lastSelected = args.OldValue as BusStopControlViewModel;
            if (lastSelected != null)
            {
                lastSelected.IsSelected = false;
            }

            BusStopControlViewModel newSelected = args.NewValue as BusStopControlViewModel;
            if (newSelected != null)
            {
                newSelected.IsSelected = true;

                // Make sure the new view model is bound to an existing control.
                // If it's not then we need to find the one that is and make it selected:                
                MapControl mapControl = (MapControl)dependencyObject;
                var busStop = (from child in mapControl.map.Children
                               let currentBusStop = child as BusStop
                               where currentBusStop != null
                               let busStopControlViewModel = currentBusStop.ViewModel
                               where busStopControlViewModel != null
                                 && string.Equals(busStopControlViewModel.StopId, newSelected.StopId, StringComparison.OrdinalIgnoreCase)
                                 && busStopControlViewModel != newSelected
                               select currentBusStop).FirstOrDefault();

                // This means we have a control that matches the selected control, but it is not
                // the same view model.
                if (busStop != null)
                {
                    busStop.ViewModel = newSelected;
                }
            }
        }

        private static void MapCenterPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            var mapControl = dependencyObject as MapControl;

            mapControl.MapCenter = (OneBusAway.Model.Point)args.NewValue;
        }

        private static void UserLocationChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            var mapControl = dependencyObject as MapControl;
            var newValue = args.NewValue as OneBusAway.Model.Point;

            if (newValue != null)
            {
                mapControl.userLocation = new Location(newValue.Latitude, newValue.Longitude);

                if (!mapControl.map.Children.Contains(mapControl.userLocationIcon))
                {
                    mapControl.map.Children.Add(mapControl.userLocationIcon);
                }

                MapLayer.SetPosition(mapControl.userLocationIcon, mapControl.userLocation);
            }
            else
            {
                mapControl.map.Children.Remove(mapControl.userLocationIcon);
            }
        }

        private static void CenterOnUserLocationChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            var mapControl = dependencyObject as MapControl;

            mapControl.CenterOnUserLocation = (bool)args.NewValue;
        }

        private static void ZoomLevelChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            var mapControl = dependencyObject as MapControl;
            var zoomLevel = (double)args.NewValue;

            if (mapControl.map.ZoomLevel != zoomLevel)
            {
                mapControl.map.ZoomLevel = zoomLevel;
            }
        }

        private static async void BusStopsChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            var mapControl = dependencyObject as MapControl;

            if (args.NewValue == null)
            {
                mapControl.displayedBusStopLookup.Clear();
                mapControl.map.Children.Clear();

                mapControl.map.Children.Add(mapControl.userLocationIcon);
                MapLayer.SetPosition(mapControl.userLocationIcon, mapControl.userLocation);
            }
            else
            {
                MapControlViewModel mapControlViewModel = (MapControlViewModel)mapControl.DataContext;
                var stops = args.NewValue as BusStopList;

                if (stops.ClearExistingStops)
                {
                    // If the clear existing stops property is set to true, 
                    // then we should clear the existing stops:
                    mapControl.map.Children.Clear();
                    mapControl.displayedBusStopLookup.Clear();
                }

                foreach (var stop in stops)
                {
                    // If we're not already displaying this bus stop then add it to the list:
                    if (!mapControl.displayedBusStopLookup.Contains(stop.StopId))
                    {
                        BusStopControlViewModel busStopControlViewModel = new BusStopControlViewModel(stop);

                        if (mapControlViewModel.SelectedBusStop != null)
                        {
                            busStopControlViewModel.IsSelected = string.Equals(mapControlViewModel.SelectedBusStop.StopId, stop.StopId, StringComparison.OrdinalIgnoreCase);
                            if (busStopControlViewModel.IsSelected)
                            {
                                mapControlViewModel.SelectedBusStop = busStopControlViewModel;
                            }
                        }

                        BusStop busStopIcon = new BusStop();
                        busStopIcon.ViewModel = busStopControlViewModel;

                        mapControl.map.Children.Add(busStopIcon);

                        // Wait for the UI to idle before we add another bus stop to make the UI more responsive:
                        await mapControl.uiHelper.WaitForIdleAsync();

                        mapControl.displayedBusStopLookup.Add(stop.StopId);

                        MapLayer.SetPosition(busStopIcon, new Location(stop.Latitude, stop.Longitude));
                    }
                }
            }
        }

        /// <summary>
        /// Called when the map view change ends.  Store the map view and if we zoom out far enough, 
        /// clear the bus stops.
        /// </summary>
        private void OnMapViewChangeEnded(object sender, ViewChangeEndedEventArgs e)
        {
            this.MapView = new MapView(new Model.Point(map.Center.Latitude, map.Center.Longitude),
                map.ZoomLevel,
                map.Bounds.Height,
                map.Bounds.Width);

            NavigationController.Instance.MapView = this.MapView;
        }

        #endregion        
    }
}
