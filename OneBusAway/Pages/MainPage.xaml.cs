using Bing.Maps;
using OneBusAway.Controls;
using OneBusAway.DataAccess;
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
        private Geocoordinate userCoordinates;
        private UserLocationIcon userLocationIcon;
        private MainPageViewModel mainPageViewModel;

        public MainPage()
        {
            this.InitializeComponent();

            this.mainPageMap.ViewChangeEnded += OnMapViewChangeEnded;
            this.userLocationIcon = new UserLocationIcon();
            this.mainPageViewModel = (MainPageViewModel)this.DataContext;            
        }

        /// <summary>
        /// Called when the main pages map view changes.
        /// </summary>
        private async void OnMapViewChangeEnded(object sender, ViewChangeEndedEventArgs e)
        {
            mainPageMap.Children.Clear();
            if (mainPageMap.ZoomLevel > Constants.MinBusStopVisibleZoom)
            {
                try
                {
                    var stops = await mainPageViewModel.GetStopsForLocation(
                        mainPageMap.Center.Latitude,
                        mainPageMap.Center.Longitude,
                        mainPageMap.Bounds.Height,
                        mainPageMap.Bounds.Width);

                    foreach (var stop in stops)
                    {
                        BusStop stopLocation = new BusStop(stop.Id, stop.Direction);
                        mainPageMap.Children.Add(stopLocation);
                        MapLayer.SetPosition(stopLocation, new Location(stop.Latitude, stop.Longitude));
                    }
                }
                catch (ObaException)
                {
                    // One Bus Away barfed for some reason.  We could be pinging them too frequently.
                }
            }
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            Location location = null;
            double zoom = Constants.DefaultMapZoom;

            if (NavigationController.Instance.PersistedStates.Count > 0)
            {
                Dictionary<string, object> previousState = NavigationController.Instance.PersistedStates.Pop();

                if (previousState.ContainsKey("location"))
                {
                    location = (Location)previousState["location"];
                }

                if (previousState.ContainsKey("zoom"))
                {
                    zoom = (double)previousState["zoom"];
                }
            }
            else
            {
                // We don't have a previous state, so let's ask the Geolocator to tell us where we are:
                Geolocator geolocator = new Geolocator();
                var geoPosition = await geolocator.GetGeopositionAsync();
                this.userCoordinates = geoPosition.Coordinate;
                location = new Location(this.userCoordinates.Latitude, this.userCoordinates.Longitude);                
            }

            MapLayer.SetPosition(userLocationIcon, location);
            mainPageMap.SetView(location, zoom, new TimeSpan()); // timespan of 0 to kill the zoom animation

            base.OnNavigatedTo(e);
        }

        /// <summary>
        /// Invoked when the user navigates from this page.
        /// </summary>
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            // Persist the state for later:
            NavigationController.Instance.PersistedStates.Push(new Dictionary<string, object>()
            {
                {"location", this.mainPageMap.Center},
                {"zoom", this.mainPageMap.ZoomLevel}
            });

            base.OnNavigatedFrom(e);
        }
    }
}
