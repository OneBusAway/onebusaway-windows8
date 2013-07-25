﻿using OneBusAway.PageControls;
using OneBusAway.Pages;
using OneBusAway.ViewModels;
using OneBusAway.ViewModels.PageControls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Search;
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

            if (rootFrame == null)
            {
                rootFrame = new Frame();
                Window.Current.Content = rootFrame;

                rootFrame.Navigate(typeof(MainPage), args.Arguments);
            }
            else
            {
                MainPage mainPage = rootFrame.Content as MainPage;
                mainPage.NavigateToPageControlByArguments(args.Arguments);
            }

            // Ensure the current window is active
            Window.Current.Activate();
        }

        protected override void OnWindowCreated(WindowCreatedEventArgs args)
        {
            base.OnWindowCreated(args);

            var pane = Windows.ApplicationModel.Search.SearchPane.GetForCurrentView();
            pane.ShowOnKeyboardInput = true;
            pane.QuerySubmitted += OnAppQuerySubmitted;
            pane.ResultSuggestionChosen += OnSearchPaneResultSuggestionChosen;
            pane.SuggestionsRequested += OnSearchPaneSuggestionsRequested;            
        }

        private async void OnSearchPaneSuggestionsRequested(SearchPane sender, SearchPaneSuggestionsRequestedEventArgs args)
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

        private async void OnSearchPaneResultSuggestionChosen(SearchPane sender, SearchPaneResultSuggestionChosenEventArgs args)
        {
            await HandleSearchQuery(args.Tag);
        }

        private async void OnAppQuerySubmitted(SearchPane sender, SearchPaneQuerySubmittedEventArgs args)
        {
            await HandleSearchQuery(args.QueryText);
        }

        private async Task HandleSearchQuery(string queryText)
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
                        await viewModel.SearchAsync(queryText);
                    }
                    else
                    {
                        await NavigationController.Instance.NavigateToPageControlAsync<SearchResultsPageControl>(queryText);
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
        protected override void OnSearchActivated(Windows.ApplicationModel.Activation.SearchActivatedEventArgs args)
        {   
            var previousContent = Window.Current.Content;
            var frame = previousContent as Frame;

            // If the app does not contain a top-level frame, it is possible that this 
            // is the initial launch of the app. Typically this method and OnLaunched 
            // in App.xaml.cs can call a common method.
            if (frame == null)
            {
                frame = new Frame();
                Window.Current.Content = frame;

                frame.Navigate(typeof(MainPage), args.QueryText);
            }
            else
            {
                MainPage mainPage = frame.Content as MainPage;
                mainPage.NavigateToPageControlByArguments(args.QueryText);
            }

            Window.Current.Activate();
        }
    }
}
