using System;
using System.ComponentModel;
using System.Windows;
using Microsoft.Phone.Controls;
using OneBusAway.PageControls;
using OneBusAway.Pages;
using OneBusAway.ViewModels.PageControls;
using Windows.Graphics.Display;

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
            double width = e.NewSize.Width;
            switch (DisplayProperties.ResolutionScale)
            {
                case ResolutionScale.Scale140Percent:
                    width *= 1.4;
                    break;

                case ResolutionScale.Scale150Percent:
                    width *= 1.5;
                    break;

                case ResolutionScale.Scale160Percent:
                    width *= 1.6;
                    break;

                case ResolutionScale.Scale180Percent:
                    width *= 1.8;
                    break;
            }

            NavigationController.Instance.IsSnapped = (width <= 480);
            NavigationController.Instance.IsPortrait = (480 < width && width < 1024);
            NavigationController.Instance.IsFullScreen = (1024 <= width);
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