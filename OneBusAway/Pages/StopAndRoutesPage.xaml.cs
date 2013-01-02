using OneBusAway.Model;
using OneBusAway.ViewModels;
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

namespace OneBusAway.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class StopAndRoutesPage : Page
    {
        private StopAndRoutesPageViewModel viewModel;

        public StopAndRoutesPage()
        {
            this.InitializeComponent();
            this.viewModel = (StopAndRoutesPageViewModel)this.DataContext;
        }

        /// <summary>
        /// Tries and restores the view model from the navigation controller stack.
        /// </summary>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if (NavigationController.TryRestoreViewModel(e.NavigationMode, ref this.viewModel))
            {
                this.DataContext = this.viewModel;
            }
            else
            {
                this.viewModel.RouteTimelineControlViewModel.TrackingData = e.Parameter as TrackingData;
            }

            await this.viewModel.RouteTimelineControlViewModel.GetTripDetailsAsync();
            base.OnNavigatedTo(e);
        }

        /// <summary>
        /// Persist the view model.
        /// </summary>
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            NavigationController.TryPersistViewModel(e.NavigationMode, this.viewModel);
            base.OnNavigatedFrom(e);
        }
    }
}
