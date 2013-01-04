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

namespace OneBusAway.Pages
{
    /// <summary>
    /// Main Page of the OBA app
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private MainPageViewModel mainPageViewModel;

        public MainPage()
        {
            this.InitializeComponent();

            this.mainPageViewModel = (MainPageViewModel)this.DataContext; 
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await Favorites.Initialize();

            if (NavigationController.TryRestoreViewModel(e.NavigationMode, ref mainPageViewModel))
            {
                this.DataContext = mainPageViewModel;
            } 
            else
            {
                StopSelectedEventArgs stopSelectedEventArgs = e.Parameter as StopSelectedEventArgs;

                if (stopSelectedEventArgs == null)
                {
                    await this.mainPageViewModel.MapControlViewModel.FindUserLocationAsync();
                    await this.mainPageViewModel.RoutesAndStopsViewModel.PopulateFavoritesAsync();
                }
                else
                {
                    await this.mainPageViewModel.NavigateDirectlyToStop(
                        stopSelectedEventArgs.Latitude,
                        stopSelectedEventArgs.Longitude,
                        stopSelectedEventArgs.SelectedStopId,
                        stopSelectedEventArgs.StopName,
                        stopSelectedEventArgs.Direction);
                }
            }

            base.OnNavigatedTo(e);
        }

        /// <summary>
        /// Invoked when the user navigates from this page.
        /// </summary>
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            NavigationController.TryPersistViewModel(e.NavigationMode, this.mainPageViewModel);
            base.OnNavigatedFrom(e);
        }
    }
}
