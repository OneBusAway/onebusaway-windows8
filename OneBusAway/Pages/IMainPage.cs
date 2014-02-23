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
using OneBusAway.PageControls;
using OneBusAway.ViewModels.PageControls;
using System;
namespace OneBusAway.Pages
{
    /// <summary>
    /// Defines an abstraction that allows us to talk to the main page in the common assembly.
    /// </summary>
    public interface IMainPage
    {
        /// <summary>
        /// Navigates to a page control with arguments.
        /// </summary>
        void NavigateToPageControlByArguments(string arguments);

        /// <summary>
        /// Sets a page view on the main page.
        /// </summary>
        void SetPageView(IPageControl pageControl);

        /// <summary>
        /// Shows the help flyout.
        /// </summary>
        void ShowHelpFlyout(bool calledFromSettings);

        /// <summary>
        /// Shows the search page.
        /// </summary>
        void ShowSearchPane();

        /// <summary>
        /// Returns the current view model for the page.
        /// </summary>
        PageViewModelBase ViewModel
        {
            get;
        }
    }
}
