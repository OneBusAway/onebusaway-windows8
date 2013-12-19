using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using OneBusAway.PageControls;
using OneBusAway.Pages;
using OneBusAway.ViewModels.PageControls;

namespace OneBusAway.WindowsPhone.Pages
{
    public partial class MainPage : PhoneApplicationPage, IMainPage
    {
        /// <summary>
        /// Creates the main page.
        /// </summary>
        public MainPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Returns the current view model.
        /// </summary>
        public PageViewModelBase ViewModel
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// Navigates to a page control.
        /// </summary>
        public void NavigateToPageControlByArguments(string arguments)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sets a page view.
        /// </summary>
        public void SetPageView(IPageControl pageControl)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Shows the help flyout.
        /// </summary>
        public void ShowHelpFlyout(bool calledFromSettings)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Shows the search pane.
        /// </summary>
        public void ShowSearchPane()
        {
        }        
    }
}