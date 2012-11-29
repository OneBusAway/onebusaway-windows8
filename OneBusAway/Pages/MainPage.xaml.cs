using Bing.Maps;
using OneBusAway.Controls;
using OneBusAway.Model;
using OneBusAway.Utilities;
using OneBusAway.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace OneBusAway.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        Geolocator geolocator = new Geolocator();
        Geoposition userPosition;
        UserLocationIcon userLocationIcon;
        MainPageViewModel mainPageViewModel;

        public MainPage()
        {
            this.InitializeComponent();

            #region StuffForTheMap            

            if (NavigationController.Instance.MapCenter != null)
            {
                mainPageMap.Center = NavigationController.Instance.MapCenter;
                mainPageMap.Bounds.Width = NavigationController.Instance.BoundsWidth;
                mainPageMap.Bounds.Height = NavigationController.Instance.BoundsHeight;
                mainPageMap.ZoomLevel = NavigationController.Instance.ZoomLevel;
            }

            geolocator.PositionChanged += geolocator_PositionChanged;
            mainPageMap.ViewChangeEnded += mainPageMap_ViewChangeEnded;
            userLocationIcon = new UserLocationIcon();
                        
            #endregion

            mainPageViewModel = this.DataContext as MainPageViewModel;            
        }

        async void mainPageMap_ViewChangeEnded(object sender, ViewChangeEndedEventArgs e)
        {
            Location mapCenter = mainPageMap.Center;
            double boundsWidth = mainPageMap.Bounds.Width;
            double boundsHeight = mainPageMap.Bounds.Height;
            double zoomLevel = mainPageMap.ZoomLevel;

            // Save state for later:
            NavigationController.Instance.MapCenter = mapCenter;
            NavigationController.Instance.BoundsWidth = boundsWidth;
            NavigationController.Instance.BoundsHeight = boundsHeight;
            NavigationController.Instance.ZoomLevel = zoomLevel;

            mainPageMap.Children.Clear();
            if (userPosition != null)
            {
                mainPageMap.Children.Add(userLocationIcon);
                MapLayer.SetPosition(userLocationIcon, new Location(userPosition.Coordinate.Latitude, userPosition.Coordinate.Longitude));
            }

            if (zoomLevel > Constants.MinBusStopVisibleZoom)
            {
                var stops = await mainPageViewModel.GetStopsForLocation(mapCenter.Latitude, mapCenter.Longitude, boundsHeight, boundsWidth);

                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    foreach (var stop in stops)
                    {
                        BusStop stopLocation = new BusStop(stop.Id, stop.Direction);
                        mainPageMap.Children.Add(stopLocation);
                        MapLayer.SetPosition(stopLocation, new Location(stop.Latitude, stop.Longitude));
                    }
                });
            }
        }

        async void geolocator_PositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {
            userPosition = args.Position;
            double latitude = userPosition.Coordinate.Latitude;
            double longitude = userPosition.Coordinate.Longitude;            

            await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    Location location = new Location(latitude, longitude);
                    MapLayer.SetPosition(userLocationIcon, location);
                    mainPageMap.SetView(location, Constants.DefaultMapZoom);                    
                });
                   
            geolocator.PositionChanged -= geolocator_PositionChanged;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }
    }
}
