using Bing.Maps;
using OneBusAway.Model;
using OneBusAway.PageControls;
using OneBusAway.Pages;
using OneBusAway.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Search;
using Windows.UI.ApplicationSettings;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace OneBusAway
{
    /// <summary>
    /// This is a singleton that allows us to move from one page to another 
    /// by binding commands in xaml.
    /// </summary>
    public class NavigationController : BindableBase
    {
        /// <summary>
        /// The one & only instance.
        /// </summary>
        private static NavigationController instance = new NavigationController();
        
        /// <summary>
        /// Command to go to the favorites page.
        /// </summary>
        private ObservableCommand goToFavoritesPageCommand;

        /// <summary>
        /// Command to go to the real time page.
        /// </summary>
        private ObservableCommand goToRealTimePageCommand;

        /// <summary>
        /// Command to go to the help page.
        /// </summary>
        private ObservableCommand goToHelpPageCommand;

        /// <summary>
        /// Command used to go to the search page.
        /// </summary>
        private ObservableCommand goToSearchPageCommand;

        /// <summary>
        /// Command used to go to the time table page.
        /// </summary>
        private ObservableCommand goToTimeTablePageCommand;

        /// <summary>
        /// Command fires the go back command.
        /// </summary>
        private ObservableCommand goBackCommand;

        /// <summary>
        /// Tells the current PageControl to refreshes its data.
        /// </summary>
        private ObservableCommand refreshCommand;

        /// <summary>
        /// Tells the current PageControl to go to the users location.
        /// </summary>
        private ObservableCommand goToUsersLocationCommand;
                
        /// <summary>
        /// Adds a favorite.
        /// </summary>
        private ObservableCommand addToFavoritesCommand;

        /// <summary>
        /// Filters the realtime data by a given route.
        /// </summary>
        private ObservableCommand filterByRouteCommand;

        /// <summary>
        /// Command used to go to the stop and routes page.
        /// </summary>
        private ObservableCommand goToStopAndRoutesPageCommand;

        /// <summary>
        /// This is the page control that is currently on display.  It is NOT kept in the stack because
        /// we only keep page controls in the stack that we have to go back to, and the current control
        /// can only be navigated back to if we go to another page first.
        /// </summary>
        private IPageControl currentPageControl;

        /// <summary>
        /// This is a stack of page controls that we have navigated to so far.
        /// </summary>
        private Stack<IPageControl> pageControls;

        /// <summary>
        /// The current map view.
        /// </summary>
        private MapView mapView;

        /// <summary>
        /// True when the app is in a snapped state.
        /// </summary>
        private bool isSnapped;

        /// <summary>
        /// True when the app is in portrait mode.
        /// </summary>
        private bool isPortrait;

        /// <summary>
        /// This cancellation token source is used to cancel an existing refresh loop so that we can start a new one.
        /// </summary>
        private CancellationTokenSource refreshLoopCancelationToken;

        /// <summary>
        /// Before we can start a new refresh loop, wait for the old one to exit.
        /// </summary>
        private Task refreshLoopTask;

        /// <summary>
        /// This is the region where the user currently resides.
        /// </summary>
        private string region;

        /// <summary>
        /// Ths is the region's default laitude.
        /// </summary>
        private double regionDefaultLat;

        /// <summary>
        /// This is the region's default longitude.
        /// </summary>
        private double regionDefaultLon;
        
        /// <summary>
        /// Creates the controller.
        /// </summary>
        private NavigationController()
        {
            this.GoBackCommand = new ObservableCommand();
            this.GoBackCommand.Executed += OnGoBackCommandExecuted;

            this.GoToFavoritesPageCommand = new ObservableCommand();
            this.GoToFavoritesPageCommand.Executed += OnGoToFavoritesPageCommandExecuted;

            this.GoToRealTimePageCommand = new ObservableCommand();
            this.GoToRealTimePageCommand.Executed += OnGoToRealTimePageCommandExecuted;

            this.GoToHelpPageCommand = new ObservableCommand();
            this.GoToHelpPageCommand.Executed += OnGoToHelpPageCommandExecuted;

            this.GoToSearchPageCommand = new ObservableCommand();
            this.GoToSearchPageCommand.Executed += OnGoToSearchPageCommandExecuted;

            this.GoToTimeTablePageCommand = new ObservableCommand();
            this.GoToTimeTablePageCommand.Executed += OnGoToTimeTablePageCommandExecuted;

            this.AddToFavoritesCommand = new ObservableCommand();
            this.AddToFavoritesCommand.Executed += OnAddToFavoritesCommandExecuted;

            this.FilterByRouteCommand = new ObservableCommand();
            this.FilterByRouteCommand.Executed += OnFilterByRouteCommandExecuted;

            this.GoToTripDetailsPageCommand = new ObservableCommand();
            this.GoToTripDetailsPageCommand.Executed += OnGoToTripDetailsPageCommandExecuted;

            this.RefreshCommand = new ObservableCommand();
            this.RefreshCommand.Executed += OnRefreshCommandExecuted;

            this.GoToUsersLocationCommand = new ObservableCommand();
            this.GoToUsersLocationCommand.Executed += OnGoToUsersLocationCommandExecuted;

            this.pageControls = new Stack<IPageControl>();
        }        

        /// <summary>
        /// Returns the main page.  Since we only have one page in the app, this is OK.
        /// </summary>
        private static MainPage MainPage
        {
            get
            {
                var currentFrame = Window.Current.Content as Frame;
                return (MainPage)currentFrame.Content;
            }
        }

        /// <summary>
        /// Returns the instance of the controller.
        /// </summary>
        public static NavigationController Instance
        {
            get
            {
                return instance;
            }
        }

        /// <summary>
        /// True when the app is in a snapped state.
        /// </summary>
        public bool IsSnapped
        {
            get
            {
                return this.isSnapped;
            }
            set
            {
                SetProperty(ref this.isSnapped, value);
            }
        }

        /// <summary>
        /// True when we are in portrait mode.
        /// </summary>
        public bool IsPortrait
        {
            get
            {
                return this.isPortrait;
            }
            set
            {
                SetProperty(ref this.isPortrait, value);
            }
        }

        /// <summary>
        /// Returns the map view of the current map.
        /// </summary>
        public MapView MapView
        {
            get
            {
                return this.mapView;
            }
            set
            {
                SetProperty(ref this.mapView, value);
            }
        }

        /// <summary>
        /// Returns the canGoBack bool.
        /// </summary>
        public bool CanGoBack
        {
            get
            {
                return (this.pageControls.Count > 0);
            }
        }

        /// <summary>
        /// Gets / sets the go back command.
        /// </summary>
        public ObservableCommand GoBackCommand
        {
            get
            {
                return this.goBackCommand;
            }
            set
            {
                SetProperty(ref this.goBackCommand, value);
            }
        }

        /// <summary>
        /// Command goes to the favorites page.
        /// </summary>
        public ObservableCommand GoToFavoritesPageCommand
        {
            get
            {
                return this.goToFavoritesPageCommand;
            }
            set
            {
                SetProperty(ref this.goToFavoritesPageCommand, value);
            }
        }

        /// <summary>
        /// Returns the real time command.
        /// </summary>
        public ObservableCommand GoToRealTimePageCommand
        {
            get
            {
                return this.goToRealTimePageCommand;
            }
            set
            {
                SetProperty(ref this.goToRealTimePageCommand, value);
            }
        }        

        /// <summary>
        /// Returns the go to help page command.
        /// </summary>
        public ObservableCommand GoToHelpPageCommand
        {
            get
            {
                return this.goToHelpPageCommand;
            }
            set
            {
                SetProperty(ref this.goToHelpPageCommand, value);
            }
        }

        /// <summary>
        /// Returns the go to search page command.
        /// </summary>
        public ObservableCommand GoToSearchPageCommand
        {
            get
            {
                return this.goToSearchPageCommand;
            }
            set
            {
                SetProperty(ref this.goToSearchPageCommand, value);
            }
        }

        /// <summary>
        /// Returns the go to time table command.
        /// </summary>
        public ObservableCommand GoToTimeTablePageCommand
        {
            get
            {
                return this.goToTimeTablePageCommand;
            }
            set 
            {
                SetProperty(ref this.goToTimeTablePageCommand, value);
            }
        }

        /// <summary>
        /// Returns the refresh command.
        /// </summary>
        public ObservableCommand RefreshCommand
        {
            get
            {
                return this.refreshCommand;
            }
            set
            {
                SetProperty(ref this.refreshCommand, value);
            }
        }

        /// <summary>
        /// Returns the go to users location command.
        /// </summary>
        public ObservableCommand GoToUsersLocationCommand
        {
            get
            {
                return this.goToUsersLocationCommand;
            }
            set
            {
                SetProperty(ref this.goToUsersLocationCommand, value);
            }
        }

        public ObservableCommand AddToFavoritesCommand
        {
            get
            {
                return this.addToFavoritesCommand;
            }
            set
            {
                SetProperty(ref this.addToFavoritesCommand, value);
            }
        }

        public ObservableCommand FilterByRouteCommand
        {
            get
            {
                return this.filterByRouteCommand;
            }
            set
            {
                SetProperty(ref this.filterByRouteCommand, value);
            }
        }

        /// <summary>
        /// Returns the trip details command.
        /// </summary>
        public ObservableCommand GoToTripDetailsPageCommand
        {
            get
            {
                return this.goToStopAndRoutesPageCommand;
            }
            set
            {
                SetProperty(ref this.goToStopAndRoutesPageCommand, value);
            }
        }

        /// <summary>
        /// Navigates to a page control.
        /// </summary>
        public async Task<T> NavigateToPageControlAsync<T>(object parameter)
            where T : IPageControl, new()
        {
            T newPageControl = Activator.CreateInstance<T>();

            if (this.currentPageControl != null)
            {
                newPageControl.ViewModel.MapControlViewModel.CopyFrom(this.currentPageControl.ViewModel.MapControlViewModel);
            }
                        
            NavigationController.MainPage.SetPageView(newPageControl);            

            if (this.currentPageControl != null)
            {
                this.pageControls.Push(currentPageControl);
            }

            this.currentPageControl = newPageControl;
            this.FirePropertyChanged("CanGoBack");

            await newPageControl.InitializeAsync(parameter);
            await this.RestartRefreshLoopAsync();            
            return newPageControl;
        }       
        
        /// <summary>
        /// Refreshes the active page control every 30 seconds. If we navigate to another page, then this
        /// loop will be cancelled. The NavigateToPageControlAsync method will cancel this task and wait for 
        /// it to exit before it kicks off another one. That way, we can't navigate to a page and have it refresh 
        /// immediately - there will always be 30 seconds between a page navigation and a page refresh.
        /// </summary>
        private async Task RefreshLoopAsync()
        {
            try
            {
                while (true)
                {
                    await Task.Delay(30000, this.refreshLoopCancelationToken.Token);
                    this.refreshLoopCancelationToken.Token.ThrowIfCancellationRequested();

                    // Call refresh asyn directly here to make sure we don't have a loop:
                    if (this.currentPageControl != null)
                    {
                        await this.currentPageControl.RefreshAsync();
                    }
                }
            }
            catch (OperationCanceledException)
            {
            }
        }

        /// <summary>
        /// Kills the existing refresh loop and restarts it with a fresh timer.
        /// </summary>
        private async Task RestartRefreshLoopAsync()
        {
            if (this.refreshLoopTask != null)
            {
                this.refreshLoopCancelationToken.Cancel();
                await this.refreshLoopTask;
            }

            // Start the refresh loop:
            this.refreshLoopCancelationToken = new CancellationTokenSource();
            this.refreshLoopTask = this.RefreshLoopAsync();
        }

        /// <summary>
        /// Called when the user goes back
        /// </summary>
        private async Task OnGoBackCommandExecuted(object arg1, object arg2)
        {
            var previousPageControl = this.pageControls.Pop();
            this.FirePropertyChanged("CanGoBack");
            await previousPageControl.RestoreAsync();

            this.currentPageControl = previousPageControl;
            MainPage.SetPageView(previousPageControl);            
        }

        /// <summary>
        /// Called when the go to favorites page is exeucted.
        /// </summary>
        private async Task OnGoToFavoritesPageCommandExecuted(object arg1, object arg2)
        {
            await this.NavigateToPageControlAsync<FavoritesPageControl>(arg2);
        }

        /// <summary>
        /// Called when the go to main page command is executed.
        /// </summary>
        private async Task OnGoToRealTimePageCommandExecuted(object arg1, object arg2)
        {
            await this.NavigateToPageControlAsync<RealTimePageControl>(arg2);
        }

        /// <summary>
        /// Called when the go to help page command is executed.
        /// </summary>
        private Task OnGoToHelpPageCommandExecuted(object arg1, object arg2)
        {
            NavigationController.MainPage.ShowHelpFlyout();
            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Called when the go to search page command is executed.
        /// </summary>
        private async Task OnGoToSearchPageCommandExecuted(object arg1, object arg2)
        {
            if (!this.IsSnapped || ApplicationView.TryUnsnap())
            {
                // Make sure we're idiling before we try this. TryUnsnap may cause the UI to refresh
                // before we can try this:
                var helper = new DefaultUIHelper(NavigationController.MainPage.Dispatcher);
                await helper.WaitForIdleAsync();

                var pane = SearchPane.GetForCurrentView();
                pane.Show();
            }
        }

        /// <summary>
        /// Called when we go to the time table page.
        /// </summary>
        private async Task OnGoToTimeTablePageCommandExecuted(object arg1, object arg2)
        {            
            await this.NavigateToPageControlAsync<TimeTablePageControl>(arg2);
        }

        /// <summary>
        /// Called when we go to the stop and routes page.
        /// </summary>
        private async Task OnGoToTripDetailsPageCommandExecuted(object arg1, object arg2)
        {
            TrackingData trackingData = arg2 as TrackingData;
            if (trackingData != null)
            {
                if (trackingData.IsNoData)
                {
                    await this.NavigateToPageControlAsync<TimeTablePageControl>(trackingData);
                }
                else
                {
                    await this.NavigateToPageControlAsync<TripDetailsPageControl>(trackingData);
                }
            }
        }

        /// <summary>
        /// Refreshes the current page control.
        /// </summary>
        private async Task OnRefreshCommandExecuted(object arg1, object arg2)
        {
            if (this.currentPageControl != null)
            {
                await this.RestartRefreshLoopAsync(); 
                await this.currentPageControl.RefreshAsync();
            }
        }

        /// <summary>
        /// Called when the user wants to go to their current location.
        /// </summary>
        private async Task OnGoToUsersLocationCommandExecuted(object arg1, object arg2)
        {
            if (this.currentPageControl != null)
            {
                if (!await this.currentPageControl.ViewModel.MapControlViewModel.TryFindUserLocationAsync())
                {
                    var messageDialog = new MessageDialog("OneBusAway does not have permission to access your location. You can change this in the Permissions section in the Settings pane.", "oh no");
                    messageDialog.DefaultCommandIndex = 0;
                    await messageDialog.ShowAsync().AsTask();
                }
            }
        }        

        private async Task OnAddToFavoritesCommandExecuted(object arg1, object arg2)
        {
            TrackingData trackingData = (TrackingData)arg2;
            StopAndRoutePair stopAndRoute = trackingData.StopAndRoute;
            
            var trackingDataViewModel = NavigationController.MainPage.DataContext as ITrackingDataViewModel;
            if (trackingDataViewModel != null)
            {
                if (trackingData.IsFavorite)
                {
                    // Un-favorite all routes that match this tracking data:
                    foreach (var currentTrackingData in trackingDataViewModel.RealTimeData)
                    {
                        if (string.Equals(trackingData.RouteId, currentTrackingData.RouteId, StringComparison.OrdinalIgnoreCase) &&
                            string.Equals(trackingData.StopId, currentTrackingData.StopId, StringComparison.OrdinalIgnoreCase))
                        {
                            currentTrackingData.IsFavorite = false;
                        }
                    }

                    await Favorites.RemoveAsync(stopAndRoute);
                }
                else
                {
                    foreach (var currentTrackingData in trackingDataViewModel.RealTimeData)
                    {
                        if (string.Equals(trackingData.RouteId, currentTrackingData.RouteId, StringComparison.OrdinalIgnoreCase) &&
                            string.Equals(trackingData.StopId, currentTrackingData.StopId, StringComparison.OrdinalIgnoreCase))
                        {
                            currentTrackingData.IsFavorite = true;
                        }
                    }

                    await Favorites.AddAsync(stopAndRoute);
                }

                await Favorites.PersistAsync();
            }        
        }

        private Task OnFilterByRouteCommandExecuted(object arg1, object arg2)
        {
            Route route = (Route)arg2;

            var currentFrame = Window.Current.Content as Frame;
            if (currentFrame != null)
            {
                if (currentFrame.CurrentSourcePageType == typeof(MainPage))
                {
                    var page = currentFrame.Content as MainPage;

                    if (page != null)
                    {
                        if (page.DataContext is RealTimePageControlViewModel)
                        {
                            RealTimePageControlViewModel viewModel = (RealTimePageControlViewModel)page.DataContext;
                            viewModel.RoutesAndStopsViewModel.ToggleFilterByRoute(route);
                        }
                        else if (page.DataContext is FavoritesPageControlViewModel)
                        {
                            FavoritesPageControlViewModel viewModel = (FavoritesPageControlViewModel)page.DataContext;
                            viewModel.RoutesAndStopsViewModel.ToggleFilterByRoute(route);
                        }
                        else
                        {
                            throw new Exception("NavigationController.FilterByRouteCommandExecuted: shouldn't get here!");
                        }
                    }
                }
            }

            return Task.FromResult<object>(null);
        }
    }
}
