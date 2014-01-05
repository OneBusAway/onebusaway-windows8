using System;
using System.ComponentModel;
using System.Windows;
using Microsoft.Phone.Controls;
using OneBusAway.PageControls;
using OneBusAway.Pages;
using OneBusAway.ViewModels;
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
            // The OBA app should only ever have one main page, and that should
            // always be this instance.
            NavigationController.Instance.MainPage = this;

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
                return this.DataContext as PageViewModelBase;;
            }
        }

        /// <summary>
        /// Navigates to a page control.
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
            }

            // Important: to make sure we don't time out opening OBA on ARM devices, load the page control when we idle.
            var ignored = this.Dispatcher.RunIdleAsync(async () =>
            {
                await NavigationController.Instance.NavigateToPageControlAsync(pageControl, parameter);
            });
        }

        /// <summary>
        /// Sets a page view.
        /// </summary>
        public void SetPageView(IPageControl pageControl)
        {
            this.scrollViewer.Content = pageControl;
            this.DataContext = pageControl.ViewModel;
        }

        /// <summary>
        /// Shows the help flyout.
        /// </summary>
        public void ShowHelpFlyout(bool calledFromSettings)
        {
            // TODO:
            throw new NotImplementedException();
        }

        /// <summary>
        /// Shows the search pane.
        /// </summary>
        public void ShowSearchPane()
        {
            // TODO:
            throw new NotImplementedException();
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