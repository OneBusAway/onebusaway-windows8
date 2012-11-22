using OneBusAway.DataAccess.BingService;
using OneBusAway.Model.BingService;
using OneBusAway.Utilities;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace OneBusAway.Tests
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        Windows.Devices.Geolocation.Geolocator geolocator = new Windows.Devices.Geolocation.Geolocator();
        Windows.Devices.Geolocation.Geoposition userPosition;

        public MainPage()
        {
            this.InitializeComponent();

            geolocator.PositionChanged += geolocator_PositionChanged;
        }

        void geolocator_PositionChanged(Windows.Devices.Geolocation.Geolocator sender, Windows.Devices.Geolocation.PositionChangedEventArgs args)
        {
            userPosition = args.Position;

            TestBingServiceApi();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

        }

        public async void TestBingServiceApi()
        {
            #region BingDataAccessTests

            List<Location> locations = await BingMapsServiceHelper.GetLocationByQuery("5th & Pine", Utilities.Confidence.High, userPosition);
            List<Location> locations1 = await BingMapsServiceHelper.GetLocationByQuery("5th & Pine, seattle", Confidence.High, userPosition);
            List<Location> locations2 = await BingMapsServiceHelper.GetLocationByQuery("Space Needle", Confidence.High);
            
            #endregion
        }
    }
}
