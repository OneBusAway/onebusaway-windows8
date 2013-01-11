using Bing.Maps;
using OneBusAway.Controls;
using OneBusAway.DataAccess;
using OneBusAway.Model;
using OneBusAway.PageControls;
using OneBusAway.Utilities;
using OneBusAway.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace OneBusAway.Pages
{
    /// <summary>
    /// Main Page of the OBA app
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            // We should only get a parameter if we've been invoked from the start screen to search.
            // In that case, don't navigate to the favorites page control.
            if (e.NavigationMode == NavigationMode.New && string.IsNullOrEmpty(e.Parameter as string))
            {
                await NavigationController.Instance.NavigateToPageControlAsync<FavoritesPageControl>(null);
            }

            base.OnNavigatedTo(e);
        }

        /// <summary>
        /// Sets the page view.  All we need to do here is replace the content in the 
        /// scroll viewer and set the data context.
        /// </summary>
        public void SetPageView(IPageControl pageControl)
        {
            this.scrollViewer.Content = pageControl;
            this.DataContext = pageControl.ViewModel;
        }

        /// <summary>
        /// Called when the size of the page changes.
        /// </summary>
        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            NavigationController.Instance.IsSnapped = (ApplicationView.Value == ApplicationViewState.Snapped);
        }
    }
}
