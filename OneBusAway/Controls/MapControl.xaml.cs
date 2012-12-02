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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace OneBusAway.Controls
{
    public partial class MapControl : UserControl
    {
        private UserLocationIcon userLocationIcon;
        private bool centerOnUserLocation;
        private Location userLocation;
        private List<Stop> busStops = new List<Stop>();

        public event EventHandler<ViewChangeEndedEventArgs> ViewChangeEnded;

        public MapControl()
        {
            this.InitializeComponent();

            this.userLocationIcon = new UserLocationIcon();
            //map.Children.Add(userLocationIcon);

            map.ViewChangeEnded += map_ViewChangeEnded;
        }

        #region Properties

        public string BingMapCredentials
        {
            get
            {
                return Utilities.Constants.BingMapCredentials;
            }
        }

        public LocationRect Bounds
        {
            get
            {
                return map.Bounds;
            }
        }


        #endregion

        #region DependencyProperty backed CLR Properties

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
                var location = MapLayer.GetPosition(userLocationIcon);

                return new Model.Point(location.Latitude, location.Longitude);
            }
            set
            {
                if (value != null)
                {
                    userLocation = new Location(value.Latitude, value.Longitude);
                    map.Children.Add(userLocationIcon);
                    MapLayer.SetPosition(userLocationIcon, userLocation);
                }
                else
                {
                    map.Children.Remove(userLocationIcon);
                }
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
                return map.ZoomLevel;
            }
            set
            {
                map.ZoomLevel = value;
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
                if (value == null)
                {
                    busStops = new List<Stop>();
                    map.Children.Clear();

                    map.Children.Add(userLocationIcon);
                    MapLayer.SetPosition(userLocationIcon, userLocation);

                    return;
                }

                foreach (var stop in value)
                {
                    if (busStops.Any(x => x.Code == stop.Code))
                    {
                        // don't do anything since the bus stop already exists
                    }
                    else
                    {
                        BusStop busStopIcon = new BusStop(stop.Code, stop.Direction);

                        map.Children.Add(busStopIcon);
                        busStops.Add(stop);

                        MapLayer.SetPosition(busStopIcon, new Location(stop.Latitude, stop.Longitude));
                    }
                }
            }
        }

        #endregion

        #region Dependency Properties

        public static readonly DependencyProperty MapCenterDP = DependencyProperty.Register("MapCenter", typeof(OneBusAway.Model.Point), typeof(MapControl), new PropertyMetadata(null, MapCenterPropertyChanged));

        public static readonly DependencyProperty UserLocationDP = DependencyProperty.Register("UserLocation", typeof(OneBusAway.Model.Point), typeof(MapControl), new PropertyMetadata(null, UserLocationChanged));

        public static readonly DependencyProperty CnterOnUserLocationDP = DependencyProperty.Register("CenterOnUserLocation", typeof(bool), typeof(MapControl), new PropertyMetadata(null, CenterOnUserLocationChanged));

        public static readonly DependencyProperty ZoomLevelDP = DependencyProperty.Register("ZoomLevel", typeof(double), typeof(MapControl), new PropertyMetadata(null, ZoomLevelChanged));

        public static readonly DependencyProperty BusStopsDP = DependencyProperty.Register("BusStops", typeof(OneBusAway.Model.Stop), typeof(MapControl), new PropertyMetadata(null, BusStopsChanged));

        #endregion

        #region Dependency Property Changed Callbacks

        private static void MapCenterPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var mapControl = d as MapControl;

            mapControl.MapCenter = (OneBusAway.Model.Point)e.NewValue;
        }        

        private static void UserLocationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var mapControl = d as MapControl;

            mapControl.UserLocation = (OneBusAway.Model.Point)e.NewValue;
        }

        private static void CenterOnUserLocationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var mapControl = d as MapControl;

            mapControl.CenterOnUserLocation = (bool)e.NewValue;
        }

        private static void ZoomLevelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var mapControl = d as MapControl;

            mapControl.ZoomLevel = (double)e.NewValue;
        }

        private static void BusStopsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var mapControl = d as MapControl;

            if (mapControl != null)
            {
                mapControl.BusStops = (List<OneBusAway.Model.Stop>)e.NewValue;
            }
        }

        #endregion

        protected virtual void OnRaiseViewChangeEndedEvent(ViewChangeEndedEventArgs e)
        {
            EventHandler<ViewChangeEndedEventArgs> handler = ViewChangeEnded;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        void map_ViewChangeEnded(object sender, ViewChangeEndedEventArgs e)
        {
            if (ZoomLevel >= Constants.MinBusStopVisibleZoom)
            {
                OnRaiseViewChangeEndedEvent(e);
            }
            else
            {
                BusStops = null;
            }
        }           
        
    }
}
