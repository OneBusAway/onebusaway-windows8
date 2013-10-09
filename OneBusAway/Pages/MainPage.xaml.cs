/* Copyright 2013 Michael Braude and individual contributors.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using Bing.Maps;
using OneBusAway.Backgrounding;
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
using Windows.ApplicationModel.Background;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ApplicationSettings;
using Windows.UI.Core;
using Windows.UI.Popups;
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

            // Add settings options:
            SettingsPane.GetForCurrentView().CommandsRequested += OnCommandsRequested;
        }

        /// <summary>
        /// Registers settings commands with the app.
        /// </summary>
        private void OnCommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            args.Request.ApplicationCommands.Add(new SettingsCommand("Privacy", "Privacy Policy", new UICommandInvokedHandler(OnPrivacyCommandExecuted)));
            args.Request.ApplicationCommands.Add(new SettingsCommand("Help", "Help", new UICommandInvokedHandler(OnHelpCommandExecuted)));
        }

        /// <summary>
        /// Called when the privacy command is invoked.
        /// </summary>
        private async void OnPrivacyCommandExecuted(IUICommand command)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri("http://onebusaway.azurewebsites.net/PrivacyPolicy.html"));
        }

        /// <summary>
        /// Called when the help command is invoked.
        /// </summary>
        private void OnHelpCommandExecuted(IUICommand command)
        {
            NavigationController.Instance.GoToHelpPageCommand.Execute(null);
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            NavigateToPageControlByArguments(e.Parameter as string);
            base.OnNavigatedTo(e);
        }

        /// <summary>
        /// Navigates to a page control by parsing an argument string. If the argument string is null or empty,
        /// then we default to the favorites page. If the string begins with "?" then we try and parse it as a 
        /// deep link. If the string is anything else then we assume it's a search.
        /// </summary>
        public void NavigateToPageControlByArguments(string arguments)
        {
            IPageControl pageControl = null;
            object parameter = arguments;

            if (string.IsNullOrEmpty(arguments))
            {
                pageControl = new FavoritesPageControl();
            }
            else
            {
                PageInitializationParameters parameters;
                if (PageInitializationParameters.TryCreate(arguments, out parameters))
                {
                    string pageControlName = parameters.GetParameter<string>("pageControl");
                    if (!string.IsNullOrEmpty(pageControlName))
                    {
                        // Make sure the type is a valid page control:
                        Type pageControlType = Type.GetType(pageControlName, false);
                        if (pageControlType != null)
                        {
                            pageControl = Activator.CreateInstance(pageControlType) as IPageControl;
                            parameter = parameters;
                        }
                    }
                }

                // We have a query string, but it's not structured in a way we expect, so let's assume it's a search query:
                if (pageControl == null)
                {
                    pageControl = new SearchResultsPageControl();
                }
            }

            // Important: to make sure we don't time out opening OBA on ARM devices, load the page control when we idle.
            var ignored = this.Dispatcher.RunIdleAsync(async cb =>
                {
                    await NavigationController.Instance.NavigateToPageControlAsync(pageControl, parameter);
                    await this.TryRegisterBackgroundTask();
                });
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
        /// Displays the help flyout.
        /// </summary>
        public void ShowHelpFlyout()
        {
            var helpFlyoutControl = new HelpFlyoutControl();
            helpFlyoutControl.Show();
        }

        /// <summary>
        /// Called when the size of the page changes.
        /// </summary>
        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            double width = e.NewSize.Width;
            NavigationController.Instance.IsSnapped = (width <= 520);
            NavigationController.Instance.IsPortrait  = (520 < width && width < 1376);
            NavigationController.Instance.IsFullScreen  = (1376 <= width);
        }

        /// <summary>
        /// Attempts to register a background task.
        /// </summary>
        private async Task<bool> TryRegisterBackgroundTask()
        {
            try
            {
                // We should always register these
                BackgroundTaskRegistrar.RegisterBackgroundTask(
                    typeof(AddedToLockScreenBackgroundTask),
                    new SystemTrigger(SystemTriggerType.LockScreenApplicationAdded, false),
                    new SystemCondition(SystemConditionType.InternetAvailable));

                BackgroundTaskRegistrar.RegisterBackgroundTask(
                    typeof(RemovedFromLockScreenBackgroundTask),
                    new SystemTrigger(SystemTriggerType.LockScreenApplicationRemoved, false));

                // Try and register the rest of our background tasks. The user could say no!
                var access = await BackgroundExecutionManager.RequestAccessAsync();
                if (access == BackgroundAccessStatus.AllowedMayUseActiveRealTimeConnectivity ||
                    access == BackgroundAccessStatus.AllowedWithAlwaysOnRealTimeConnectivity)
                {
                    BackgroundTaskRegistrar.TryRegisterAllBackgroundTasks();
                }
            }
            catch
            {
            }

            return false;
        }
    }
}
