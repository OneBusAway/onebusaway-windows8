using OneBusAway.Model;
using OneBusAway.PageControls;
using OneBusAway.Pages;
using OneBusAway.ViewModels;
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Search;
using Windows.UI.Popups;
using Windows.UI.StartScreen;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

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
        /// This is a list of proxy objects that are registered with us.
        /// </summary>
        private List<WeakReference<NavigationControllerProxy>> proxies;

        /// <summary>
        /// A list of proxy callbacks.
        /// </summary>
        private ConditionalWeakTable<NavigationControllerProxy, PropertyChangedEventHandler> callbackTable;
        
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
        /// Command used to pin a stop to the start screen.
        /// </summary>
        private ObservableCommand pinStopToStartPageCommand;

        /// <summary>
        /// Command used to unpin a stop from the start screen.
        /// </summary>
        private ObservableCommand unpinStopToStartPageCommand;

        /// <summary>
        /// A list of all the commands.
        /// </summary>
        private List<ObservableCommand> allCommands;

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
        /// True when the current page control is pinned.
        /// </summary>
        private bool isCurrentControlPinned;

        /// <summary>
        /// This cancellation token source is used to cancel an existing refresh loop so that we can start a new one.
        /// </summary>
        private CancellationTokenSource refreshLoopCancelationToken;

        /// <summary>
        /// Before we can start a new refresh loop, wait for the old one to exit.
        /// </summary>
        private Task refreshLoopTask;
        
        /// <summary>
        /// Creates the controller.
        /// </summary>
        private NavigationController()
        {
            this.allCommands = new List<ObservableCommand>();
            this.proxies = new List<WeakReference<NavigationControllerProxy>>();
            this.callbackTable = new ConditionalWeakTable<NavigationControllerProxy, PropertyChangedEventHandler>();

            this.GoBackCommand = new ObservableCommand();
            this.GoBackCommand.Executed += OnGoBackCommandExecuted;
            this.allCommands.Add(this.GoBackCommand);

            this.GoToFavoritesPageCommand = new ObservableCommand();
            this.GoToFavoritesPageCommand.Executed += OnGoToFavoritesPageCommandExecuted;
            this.allCommands.Add(this.GoToFavoritesPageCommand);

            this.GoToRealTimePageCommand = new ObservableCommand();
            this.GoToRealTimePageCommand.Executed += OnGoToRealTimePageCommandExecuted;
            this.allCommands.Add(this.GoToRealTimePageCommand);

            this.GoToHelpPageCommand = new ObservableCommand();
            this.GoToHelpPageCommand.Executed += OnGoToHelpPageCommandExecuted;
            this.allCommands.Add(this.GoToHelpPageCommand);

            this.GoToSearchPageCommand = new ObservableCommand();
            this.GoToSearchPageCommand.Executed += OnGoToSearchPageCommandExecuted;
            this.allCommands.Add(this.GoToSearchPageCommand);

            this.GoToTimeTablePageCommand = new ObservableCommand();
            this.GoToTimeTablePageCommand.Executed += OnGoToTimeTablePageCommandExecuted;
            this.allCommands.Add(this.GoToTimeTablePageCommand);

            this.AddToFavoritesCommand = new ObservableCommand();
            this.AddToFavoritesCommand.Executed += OnAddToFavoritesCommandExecuted;
            this.allCommands.Add(this.AddToFavoritesCommand);

            this.FilterByRouteCommand = new ObservableCommand();
            this.FilterByRouteCommand.Executed += OnFilterByRouteCommandExecuted;
            this.allCommands.Add(this.FilterByRouteCommand);

            this.GoToTripDetailsPageCommand = new ObservableCommand();
            this.GoToTripDetailsPageCommand.Executed += OnGoToTripDetailsPageCommandExecuted;
            this.allCommands.Add(this.GoToTripDetailsPageCommand);

            this.RefreshCommand = new ObservableCommand();
            this.RefreshCommand.Executed += OnRefreshCommandExecuted;
            this.allCommands.Add(this.RefreshCommand);

            this.GoToUsersLocationCommand = new ObservableCommand();
            this.GoToUsersLocationCommand.Executed += OnGoToUsersLocationCommandExecuted;
            this.allCommands.Add(this.GoToUsersLocationCommand);

            this.PinStopToStartPageCommand = new ObservableCommand();
            this.PinStopToStartPageCommand.Executed += OnPinStopToStartPageCommandExecuted;
            this.allCommands.Add(this.PinStopToStartPageCommand);

            this.UnPinStopToStartPageCommand = new ObservableCommand();
            this.UnPinStopToStartPageCommand.Executed += OnUnPinStopToStartPageCommandExecuted;
            this.allCommands.Add(this.UnPinStopToStartPageCommand);

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
        /// True when the current control is pinned to start.
        /// </summary>
        public bool IsCurrentControlPinned
        {
            get
            {
                return this.isCurrentControlPinned;
            }
            set
            {
                SetProperty(ref this.isCurrentControlPinned, value);
            }
        }

        /// <summary>
        /// Gets / sets the current page control.
        /// </summary>
        public IPageControl CurrentPageControl
        {
            get
            {
                return this.currentPageControl;
            }
            set
            {
                SetProperty(ref this.currentPageControl, value);
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

        /// <summary>
        /// Returns the pinStopToStartPageCommand.
        /// </summary>
        public ObservableCommand PinStopToStartPageCommand
        {
            get
            {
                return this.pinStopToStartPageCommand;
            }
            set
            {
                SetProperty(ref this.pinStopToStartPageCommand, value);
            }
        }

        /// <summary>
        /// Returns the unpinStopToStartPageCommand.
        /// </summary>
        public ObservableCommand UnPinStopToStartPageCommand
        {
            get
            {
                return this.unpinStopToStartPageCommand;
            }
            set
            {
                SetProperty(ref this.unpinStopToStartPageCommand, value);
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
        /// Registers a navigation proxy with the controller. This is a weak reference.
        /// </summary>
        /// <param name="proxy">The proxy to register</param>
        public void RegisterProxy(NavigationControllerProxy proxy, PropertyChangedEventHandler callback)
        {
            this.proxies.Add(new WeakReference<NavigationControllerProxy>(proxy));
            this.callbackTable.Add(proxy, callback);
        }

        /// <summary>
        /// Navigates to a page control.
        /// </summary>
        public async Task<T> NavigateToPageControlAsync<T>(object parameter)
            where T : IPageControl, new()
        {
            T newPageControl = Activator.CreateInstance<T>();
            await NavigateToPageControlAsync(newPageControl, parameter);
            return newPageControl;
        }

        /// <summary>
        /// This overload takes a specific page control. Used in cases where we are activated via a URL.
        /// </summary>
        public async Task NavigateToPageControlAsync(IPageControl newPageControl, object parameter)
        {
            if (this.CurrentPageControl != null)
            {
                newPageControl.ViewModel.MapControlViewModel.CopyFrom(this.CurrentPageControl.ViewModel.MapControlViewModel);
            }
                        
            NavigationController.MainPage.SetPageView(newPageControl);

            if (this.CurrentPageControl != null)
            {
                this.pageControls.Push(this.CurrentPageControl);
            }

            this.CurrentPageControl = newPageControl;
            this.FirePropertyChanged("CanGoBack");

            await newPageControl.InitializeAsync(parameter);
            await this.UpdateIsPinnableAsync(newPageControl);
            await this.RestartRefreshLoopAsync();            
        }

        /// <summary>
        /// Checks to see whether the new page control is pinned to start or not.
        /// </summary>
        public async Task UpdateIsPinnableAsync(IPageControl pageControl)
        {
            // If this is a pinnable page control, see if we're pinned or not:
            IPinablePageControl pinnablePageControl = pageControl as IPinablePageControl;
            if (pinnablePageControl != null)
            {
                // See if we're pinned to start:
                var query = from tile in await SecondaryTile.FindAllAsync()
                            where string.Equals(pinnablePageControl.TileId, tile.TileId, StringComparison.OrdinalIgnoreCase)
                            select tile;

                this.IsCurrentControlPinned = query.Count() > 0;
            }
            else
            {
                this.IsCurrentControlPinned = false;
            }
        }   

        /// <summary>
        /// Overrides the FirePropertyChanged method to fire the weakly held proxy events.
        /// </summary>
        protected override void FirePropertyChanged(string propName)
        {
            base.FirePropertyChanged(propName);

            bool shouldScavenge = false;
            for (int i = this.proxies.Count - 1; i >= 0; i--)
            {
                NavigationControllerProxy proxy = null;
                PropertyChangedEventHandler callback = null;
                WeakReference<NavigationControllerProxy> weakReference = this.proxies[i];

                if (weakReference.TryGetTarget(out proxy) && this.callbackTable.TryGetValue(proxy, out callback))
                {
                    callback(this, new PropertyChangedEventArgs(propName));
                }
                else
                {
                    this.proxies.RemoveAt(i);
                    shouldScavenge = true;
                }
            }

            // Remove all of the commands that have been GCed:
            if (shouldScavenge)
            {
                foreach (ObservableCommand command in this.allCommands)
                {
                    command.CleanupUnheldProxies();
                }
            }
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
                    if (this.CurrentPageControl != null)
                    {
                        await this.CurrentPageControl.RefreshAsync();
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
                this.refreshLoopCancelationToken.Dispose();
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

            this.CurrentPageControl = previousPageControl;
            MainPage.SetPageView(previousPageControl);
            await this.UpdateIsPinnableAsync(this.CurrentPageControl);
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
            if (this.CurrentPageControl != null)
            {
                await this.RestartRefreshLoopAsync();
                await this.CurrentPageControl.RefreshAsync();
            }
        }

        /// <summary>
        /// Called when the user wants to go to their current location.
        /// </summary>
        private async Task OnGoToUsersLocationCommandExecuted(object arg1, object arg2)
        {
            if (this.CurrentPageControl != null)
            {
                if (!await this.CurrentPageControl.ViewModel.MapControlViewModel.TryFindUserLocationAsync())
                {
                    try
                    {
                        var messageDialog = new MessageDialog("OneBusAway does not have permission to access your location. You can change this in the Permissions section in the Settings pane.", "oh no");
                        messageDialog.DefaultCommandIndex = 0;
                        await messageDialog.ShowAsync().AsTask();
                    }
                    catch
                    {
                        // In some cases ShowAsync throws an exception. Not sure why, can't repro it. But there's
                        // no reason to fail the app.
                    }
                }
            }
        }        

        /// <summary>
        /// Called when the pin stop to start page command is executed.
        /// </summary>
        private async Task OnPinStopToStartPageCommandExecuted(object arg1, object arg2)
        {
            IPinablePageControl pinnablePageControl = arg2 as IPinablePageControl;

            if (pinnablePageControl != null)
            {
                Uri logoUri = new Uri("ms-appx:///Assets/Logo.scale-100.png");
                Uri smallLogo = new Uri("ms-appx:///Assets/SmallLogo.scale-100.png");
                Uri wideLogoUri = new Uri("ms-appx:///Assets/WideLogo.scale-100.png");

                SecondaryTile secondaryTile = new SecondaryTile()
                {
                    TileId = pinnablePageControl.TileId,
                    ShortName = pinnablePageControl.TileName,
                    DisplayName = pinnablePageControl.TileName,
                    TileOptions = TileOptions.ShowNameOnLogo,
                    Arguments = pinnablePageControl.GetParameters().ToQueryString(),
                    Logo = logoUri,
                    WideLogo = wideLogoUri,
                    SmallLogo = smallLogo,
                    ForegroundText = ForegroundText.Light,
                };

                if (await secondaryTile.RequestCreateAsync())
                {
                    await pinnablePageControl.UpdateTileAsync(true);
                    this.IsCurrentControlPinned = true;
                }
            }
        }

        /// <summary>
        /// Unpins a secondary tile from the start screen.
        /// </summary>
        private async Task OnUnPinStopToStartPageCommandExecuted(object arg1, object arg2)
        {
            IPinablePageControl pinnablePageControl = arg2 as IPinablePageControl;
            if (pinnablePageControl != null)
            {
                var secondaryTile = (from tile in await SecondaryTile.FindAllAsync()
                                     where string.Equals(pinnablePageControl.TileId, tile.TileId, StringComparison.OrdinalIgnoreCase)
                                     select tile).FirstOrDefault();

                if (secondaryTile != null)
                {
                    if (await secondaryTile.RequestDeleteAsync())
                    {
                        await pinnablePageControl.UpdateTileAsync(false);
                        this.IsCurrentControlPinned = false;
                    }
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
