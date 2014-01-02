using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private NavigationControllerProxy proxy;

        /// <summary>
        /// Creates the main page.
        /// </summary>
        public MainPage()
        {
            InitializeComponent();
            this.proxy = (NavigationControllerProxy)this.Resources["navigationProxy"];
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

        /// <summary>
        /// Move to different visual states based on the dimensions of the app.
        /// </summary>
        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {

        }

        /// <summary>
        /// Handle back navigation manually.
        /// </summary>
        private void OnBackButtonPressed(object sender, CancelEventArgs e)
        {
            if (proxy.CanGoBack)
            {
                proxy.GoBackCommand.Execute(null);
                e.Cancel = true;
            }
        }   

        private void OnGoToSearchButtonClicked(object sender, EventArgs e)
        {
            this.proxy.GoToSearchPageCommand.Execute(null);
        }

        private void OnGoToFavoritesButtonClicked(object sender, EventArgs e)
        {
            this.proxy.GoToFavoritesPageCommand.Execute(null);
        }

        private void OnRefreshButtonClicked(object sender, EventArgs e)
        {
            this.proxy.RefreshCommand.Execute(null);
        }

        private void OnGoToUsersLocationButtonClicked(object sender, EventArgs e)
        {
            this.proxy.GoToUsersLocationCommand.Execute(null);
        }     
    }
}