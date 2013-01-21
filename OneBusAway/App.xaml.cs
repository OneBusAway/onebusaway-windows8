using OneBusAway.PageControls;
using OneBusAway.Pages;
using OneBusAway.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234227

namespace OneBusAway
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
            
            // Queue a task to refresh the search cache:
            Task.Run(() => AllRoutesCache.EnsureUpToDateAsync());
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                if (!rootFrame.Navigate(typeof(MainPage), args.Arguments))
                {
                    throw new Exception("Failed to create initial page");
                }
            }
            // Ensure the current window is active
            Window.Current.Activate();
        }

        protected override void OnWindowCreated(WindowCreatedEventArgs args)
        {
            base.OnWindowCreated(args);

            var pane = Windows.ApplicationModel.Search.SearchPane.GetForCurrentView();
            pane.ShowOnKeyboardInput = true;
            pane.QuerySubmitted += App_QuerySubmitted;
            pane.ResultSuggestionChosen += pane_ResultSuggestionChosen;
            pane.SuggestionsRequested += pane_SuggestionsRequested;            
        }

        async void pane_SuggestionsRequested(Windows.ApplicationModel.Search.SearchPane sender, Windows.ApplicationModel.Search.SearchPaneSuggestionsRequestedEventArgs args)
        {
            var frame = Window.Current.Content as Frame;
            if (frame != null)
            {
                var searchResultsPage = frame.Content as MainPage;
                if (searchResultsPage != null)
                {
                    var viewModel = searchResultsPage.DataContext as SearchResultsPageControlViewModel;
                    if (viewModel != null)
                    {
                        args.Request.SearchSuggestionCollection.AppendQuerySuggestions(await viewModel.GetSuggestionsAsync(args.QueryText));
                    }
                }
            }
        }

        void pane_ResultSuggestionChosen(Windows.ApplicationModel.Search.SearchPane sender, Windows.ApplicationModel.Search.SearchPaneResultSuggestionChosenEventArgs args)
        {
        }

        async void App_QuerySubmitted(Windows.ApplicationModel.Search.SearchPane sender, Windows.ApplicationModel.Search.SearchPaneQuerySubmittedEventArgs args)
        {
            var frame = Window.Current.Content as Frame;

            if (frame != null)
            {
                var searchResultsPage = frame.Content as MainPage;
                if (searchResultsPage != null)
                {
                    var viewModel = searchResultsPage.DataContext as SearchResultsPageControlViewModel;
                    if (viewModel != null)
                    {
                        await viewModel.SearchAsync(args.QueryText);
                    }
                    else
                    {
                        await NavigationController.Instance.NavigateToPageControlAsync<SearchResultsPageControl>(args.QueryText);
                    }
                }                
            }
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }

        /// <summary>
        /// Invoked when the application is activated to display search results.
        /// </summary>
        /// <param name="args">Details about the activation request.</param>
        protected async override void OnSearchActivated(Windows.ApplicationModel.Activation.SearchActivatedEventArgs args)
        {
            // TODO: Register the Windows.ApplicationModel.Search.SearchPane.GetForCurrentView().QuerySubmitted
            // event in OnWindowCreated to speed up searches once the application is already running
            
            // If the Window isn't already using Frame navigation, insert our own Frame
            var previousContent = Window.Current.Content;
            var frame = previousContent as Frame;

            // If the app does not contain a top-level frame, it is possible that this 
            // is the initial launch of the app. Typically this method and OnLaunched 
            // in App.xaml.cs can call a common method.
            if (frame == null)
            {
                // Create a Frame to act as the navigation context and associate it with
                // a SuspensionManager key
                frame = new Frame();
                OneBusAway.Common.SuspensionManager.RegisterFrame(frame, "AppFrame");

                if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // Restore the saved session state only when appropriate
                    try
                    {
                        await OneBusAway.Common.SuspensionManager.RestoreAsync();
                    }
                    catch (OneBusAway.Common.SuspensionManagerException)
                    {
                        //Something went wrong restoring state.
                        //Assume there is no state and continue
                    }
                }
            }

            frame.Navigate(typeof(MainPage), args.QueryText);

            Window.Current.Content = frame;
            await NavigationController.Instance.NavigateToPageControlAsync<SearchResultsPageControl>(args.QueryText);

            // Ensure the current window is active
            Window.Current.Activate();
        }
    }
}
