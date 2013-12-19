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
