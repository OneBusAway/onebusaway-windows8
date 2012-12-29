using OneBusAway.Model;
using OneBusAway.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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

namespace OneBusAway.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TimeTablePage : Page
    {
        /// <summary>
        /// The view model for the time table page.
        /// </summary>
        private TimeTablePageViewModel timeTablePageViewModel;

        /// <summary>
        /// Creates the page.
        /// </summary>
        public TimeTablePage()
        {
            this.InitializeComponent();
            this.timeTablePageViewModel = (TimeTablePageViewModel)this.DataContext;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if (NavigationController.Instance.PersistedStates.Count > 0 && e.NavigationMode == NavigationMode.Back)
            {
                Dictionary<string, object> previousState = NavigationController.Instance.PersistedStates.Pop();
                this.timeTablePageViewModel = (TimeTablePageViewModel)previousState["timeTablePageViewModel"];
                this.DataContext = this.timeTablePageViewModel;
            }
            else 
            {
                TrackingData trackingData = e.Parameter as TrackingData;
                if (trackingData != null)
                {
                    await this.timeTablePageViewModel.SetRouteAndStopData(trackingData);
                    await this.timeTablePageViewModel.GetRouteData(trackingData);
                }
            }

            base.OnNavigatedTo(e);
        }

        /// <summary>
        /// Save state so that we can revive ourselves later.
        /// </summary>
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            // Persist the state for later:
            if (e.NavigationMode != NavigationMode.Back)
            {
                NavigationController.Instance.PersistedStates.Push(new Dictionary<string, object>()
                {
                    {"timeTablePageViewModel", this.timeTablePageViewModel}
                });
            }

            base.OnNavigatedFrom(e);
        }
    }
}
