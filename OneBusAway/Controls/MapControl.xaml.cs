using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Bing.Maps;
using Windows.Devices.Geolocation;
using OneBusAway.Utilities;
using OneBusAway.Model;

namespace OneBusAway.Controls
{
    public partial class MapControl : UserControl
    {
        private UserLocationIcon userLocationIcon;
        private bool centerOnUserLocation;
        private Location userLocation;
        private List<Stop> busStops = new List<Stop>();

        public MapControl()
        {
            this.InitializeComponent();

            this.userLocationIcon = new UserLocationIcon();

            map.ViewChangeEnded += map_ViewChangeEnded;            
        }

        public string BingMapCredentials
        {
            get
            {
                return Utilities.UtilitiesConstants.BingMapCredentials;
            }
        }

        #region DependencyProperty backed CLR Properties

        public double BoundsWidth
        {
            get
            {
                return map.Bounds.Width;
            }
            set
            {
                map.Bounds.Width = value;
            }
        }

        public double BoundsHeight
        {
            get
            {
                return map.Bounds.Height;
            }
            set
            {
                map.Bounds.Height = value;
            }
        }

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
                    map.Center = new Location(UtilitiesConstants.DefaultLatitude, UtilitiesConstants.DefaultLongitude);

                    map.SetView(map.Center, ZoomLevel, new TimeSpan());
                }
            }
        }

        public OneBusAway.Model.Point UserLocation
        {
            get
            {
                return GetValue(UserLocationDP) as OneBusAway.Model.Point;
            }
            set
            {
                SetValue(UserLocationDP, value);
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
                return (double)GetValue(ZoomLevelDP);
            }
            set
            {
                SetValue(ZoomLevelDP, value);
            }
        }

        public List<Stop> BusStops
        {
            get
            {
                return GetValue(BusStopsDP) as List<Stop>;
            }
            set
            {
                SetValue(BusStopsDP, value);
            }
        }

        public MapView MapView
        {
            get
            {
                return GetValue(MapViewDP) as MapView;
            }
            set
            {
                SetValue(MapViewDP, value);
            }
        }

        #endregion

        #region Dependency Properties

        public static readonly DependencyProperty MapCenterDP = DependencyProperty.Register("MapCenter", typeof(OneBusAway.Model.Point), typeof(MapControl), new PropertyMetadata(null, MapCenterPropertyChanged));

        public static readonly DependencyProperty UserLocationDP = DependencyProperty.Register("UserLocation", typeof(OneBusAway.Model.Point), typeof(MapControl), new PropertyMetadata(null, UserLocationChanged));

        public static readonly DependencyProperty CnterOnUserLocationDP = DependencyProperty.Register("CenterOnUserLocation", typeof(bool), typeof(MapControl), new PropertyMetadata(null, CenterOnUserLocationChanged));

        public static readonly DependencyProperty ZoomLevelDP = DependencyProperty.Register("ZoomLevel", typeof(double), typeof(MapControl), new PropertyMetadata(null, ZoomLevelChanged));

        public static readonly DependencyProperty BusStopsDP = DependencyProperty.Register("BusStops", typeof(OneBusAway.Model.Stop), typeof(MapControl), new PropertyMetadata(null, BusStopsChanged));

        public static readonly DependencyProperty BoundsWidthDP = DependencyProperty.Register("BoundsWidth", typeof(double), typeof(MapControl), null);

        public static readonly DependencyProperty BoundsHeightDP = DependencyProperty.Register("BoundsHeight", typeof(double), typeof(MapControl), null);

        public static readonly DependencyProperty MapViewDP = DependencyProperty.Register("MapView", typeof(MapView), typeof(MapControl), new PropertyMetadata(null, MapViewChanged));

        #endregion

        #region Dependency Property Changed Callbacks

        private static void MapViewChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var mapControl = d as MapControl;
            var newValue = e.NewValue as MapView;

            if (newValue != null)
            {
                if (newValue.MapCenter.Latitude != mapControl.map.Center.Latitude ||
                    newValue.MapCenter.Longitude != mapControl.map.Center.Longitude)
                {
                    mapControl.map.Center = new Location(newValue.MapCenter.Latitude, newValue.MapCenter.Longitude);
                }

                if (newValue.ZoomLevel != mapControl.map.ZoomLevel)
                {
                    mapControl.map.ZoomLevel = newValue.ZoomLevel;
                }
            }
        }

        private static void MapCenterPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {            
            var mapControl = d as MapControl;

            mapControl.MapCenter = (OneBusAway.Model.Point)e.NewValue;
        }        

        private static void UserLocationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var mapControl = d as MapControl;
            var newValue = e.NewValue as OneBusAway.Model.Point;

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

        private static void CenterOnUserLocationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var mapControl = d as MapControl;

            mapControl.CenterOnUserLocation = (bool)e.NewValue;
        }

        private static void ZoomLevelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var mapControl = d as MapControl;
            var zoomLevel = (double)e.NewValue;

            if (mapControl.map.ZoomLevel != zoomLevel)
            {
                mapControl.map.ZoomLevel = zoomLevel;
            }
        }

        private static void BusStopsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var mapControl = d as MapControl;

            if (e.NewValue == null)
            {
                mapControl.busStops = new List<Stop>();
                mapControl.map.Children.Clear();

                mapControl.map.Children.Add(mapControl.userLocationIcon);
                MapLayer.SetPosition(mapControl.userLocationIcon, mapControl.userLocation);
            }
            else
            {
                var stops = e.NewValue as List<Stop>;
                foreach (var stop in stops)
                {
                    if (mapControl.busStops.Any(x => x.Code == stop.Code))
                    {
                        // don't do anything since the bus stop already exists
                    }
                    else
                    {
                        BusStop busStopIcon = new BusStop(stop.Code, stop.Direction);

                        mapControl.map.Children.Add(busStopIcon);
                        mapControl.busStops.Add(stop);

                        MapLayer.SetPosition(busStopIcon, new Location(stop.Latitude, stop.Longitude));
                    }
                }
            }
        }

        #endregion

        void map_ViewChangeEnded(object sender, ViewChangeEndedEventArgs e)
        {            
            try
            {
                map.ViewChangeEnded -= map_ViewChangeEnded;
                if (map.ZoomLevel > UtilitiesConstants.MinBusStopVisibleZoom)
                {
                    var mapCurrentView  = new MapView(new Model.Point(map.Center.Latitude, map.Center.Longitude), map.ZoomLevel, map.Bounds.Height, map.Bounds.Width);
                    SetValue(MapViewDP, mapCurrentView);
                }
                else
                {
                    SetValue(BusStopsDP, null);
                }
            }
            catch (Exception)
            {
                // TODO
            }
            finally
            {
                map.ViewChangeEnded += map_ViewChangeEnded;
            }
        }         
    }
}
