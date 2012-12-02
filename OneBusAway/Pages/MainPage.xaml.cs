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
        private MainPageViewModel mainPageViewModel;

        public MainPage()
        {
            this.InitializeComponent();

            this.mainPageViewModel = (MainPageViewModel)this.DataContext; 

            mainPageMap.ViewChangeEnded += mainPageMap_ViewChangeEnded;            

        }

        void mainPageMap_ViewChangeEnded(object sender, ViewChangeEndedEventArgs e)
        {
            try
            {
                mainPageViewModel.RefreshStopsForLocationAsync(mainPageMap.MapCenter.Latitude, mainPageMap.MapCenter.Longitude, mainPageMap.Bounds.Height, mainPageMap.Bounds.Width);
            }
            catch (ObaException)
            {
                // One Bus Away barfed for some reason.  We could be pinging them too frequently.
            }
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            OneBusAway.Model.Point mapCenter = new OneBusAway.Model.Point();

            OneBusAway.Model.Point userLocation = new OneBusAway.Model.Point();

            if (App.UserLocation != null)
            {
                userLocation.Latitude = App.UserLocation.Coordinate.Latitude;
                userLocation.Longitude = App.UserLocation.Coordinate.Longitude;
            }
            else
            {
                // TODO: Raise an error bar informing user that his location could not be found
                userLocation = null;
            }

            if (NavigationController.Instance.PersistedStates.Count > 0)
            {
                Dictionary<string, object> previousState = NavigationController.Instance.PersistedStates.Pop();

                mapCenter.Latitude = ((OneBusAway.Model.Point)previousState["location"]).Latitude;
                mapCenter.Longitude = ((OneBusAway.Model.Point)previousState["location"]).Longitude;
                mainPageViewModel.ZoomLevel = (double)previousState["zoom"];
            }
            else
            {
                mapCenter = userLocation;
                mainPageViewModel.ZoomLevel = Constants.DefaultMapZoom;
            }                        

            mainPageViewModel.MapCenter = mapCenter;
            mainPageViewModel.UserLocation = userLocation;
            

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
                {"location", mainPageMap.MapCenter},
                {"zoom", this.mainPageMap.ZoomLevel}
            });

            base.OnNavigatedFrom(e);
        }
    }
}
