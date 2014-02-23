/* Copyright 2014 Michael Braude and individual contributors.
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
using System.Diagnostics;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using OneBusAway.Platforms.WindowsPhone;
using OneBusAway.Services;
using OneBusAway.WindowsPhone.PageControls;
using OneBusAway.WindowsPhone.Pages;

namespace OneBusAway
{
    public partial class App : Application
    {
        private PhoneApplicationFrame rootFrame;

        /// <summary>
        /// Constructor for the Application object.
        /// </summary>
        public App()
        {
            ServiceRepository.FileService = new FileService();
            ServiceRepository.GeoLocationService = new GeoLocationService();
            ServiceRepository.MessageBoxService = new MessageBoxService();
            ServiceRepository.PageControlService = new PageControlService();
            ServiceRepository.SettingsService = new SettingsService();
            ServiceRepository.TileService = new TileService();

            this.UnhandledException += OnUnhandledException;

            InitializeComponent();

            // Create the frame but don't set it as RootVisual yet; this allows the splash
            // screen to remain active until the application is ready to render.
            this.rootFrame = new PhoneApplicationFrame();
            this.rootFrame.Navigated += this.OnRootFrameNavigated;
            this.rootFrame.Language = XmlLanguage.GetLanguage("en-us");
            this.rootFrame.FlowDirection = FlowDirection.LeftToRight;

            // Show graphics profiling information while debugging.
            if (Debugger.IsAttached)
            {
                // Prevent the screen from turning off while under the debugger by disabling
                // the application's idle detection.
                PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
            }
        }

        private void OnApplicationLaunching(object sender, LaunchingEventArgs e)
        {
        }

        private void OnApplicationActivated(object sender, ActivatedEventArgs e)
        {
        }

        private void OnApplicationDeactivated(object sender, DeactivatedEventArgs e)
        {
        }

        private void OnApplicationClosing(object sender, ClosingEventArgs e)
        {
        }

        // Code to execute on Unhandled Exceptions
        private void OnUnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                Debugger.Break();
            }
        }

        /// <summary>
        /// Sets the root visual after the splash screen has loaded.
        /// </summary>
        private void OnRootFrameNavigated(object sender, NavigationEventArgs e)
        {
            if (this.RootVisual != this.rootFrame)
            {
                this.RootVisual = this.rootFrame;
            }

            MainPage mainPage = e.Content as MainPage;
            if (mainPage != null)
            {
                mainPage.NavigateToPageControlByArguments(null);
            }
        }
    }
}