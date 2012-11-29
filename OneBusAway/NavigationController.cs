using Bing.Maps;
using OneBusAway.Model;
using OneBusAway.Pages;
using OneBusAway.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace OneBusAway
{
    /// <summary>
    /// This is a singleton that allows us to move from one page to another 
    /// by binding commands in xaml.
    /// </summary>
    public class NavigationController : BindableBase
    {
        /// <summary>
        /// The one & only instance.
        /// </summary>
        private static NavigationController instance = new NavigationController();

        /// <summary>
        /// Command to go to the main page.
        /// </summary>
        private ObservableCommand goToMainPageCommand;

        /// <summary>
        /// Command to go to the help page.
        /// </summary>
        private ObservableCommand goToHelpPageCommand;

        /// <summary>
        /// Command used to go to the search page.
        /// </summary>
        private ObservableCommand goToSearchPageCommand;

        /// <summary>
        /// Command fires the go back command.
        /// </summary>
        private ObservableCommand goBackCommand;
        
        /// <summary>
        /// Creates the controller.
        /// </summary>
        private NavigationController()
        {
            this.GoBackCommand = new ObservableCommand();
            this.GoBackCommand.Executed += OnGoBackCommandExecuted;

            this.GoToMainPageCommand = new ObservableCommand();
            this.GoToMainPageCommand.Executed += OnGoToMainPageCommandExecuted;

            this.GoToHelpPageCommand = new ObservableCommand();
            this.GoToHelpPageCommand.Executed += OnGoToHelpPageCommandExecuted;

            this.GoToSearchPageCommand = new ObservableCommand();
            this.GoToSearchPageCommand.Executed += OnGoToSearchPageCommandExecuted;
        }       

        /// <summary>
        /// Returns the instance of the controller.
        /// </summary>
        public static NavigationController Instance
        {
            get
            {
                return instance;
            }
        }

        /// <summary>
        /// Gets / sets the go back command.
        /// </summary>
        public ObservableCommand GoBackCommand
        {
            get
            {
                return this.goBackCommand;
            }
            set
            {
                SetProperty(ref this.goBackCommand, value);
            }
        }

        /// <summary>
        /// Returns the main page command.
        /// </summary>
        public ObservableCommand GoToMainPageCommand
        {
            get
            {
                return this.goToMainPageCommand;
            }
            set
            {
                SetProperty(ref this.goToMainPageCommand, value);
            }
        }        

        /// <summary>
        /// Returns the go to help page command.
        /// </summary>
        public ObservableCommand GoToHelpPageCommand
        {
            get
            {
                return this.goToHelpPageCommand;
            }
            set
            {
                SetProperty(ref this.goToHelpPageCommand, value);
            }
        }

        /// <summary>
        /// Returns the go to search page command.
        /// </summary>
        public ObservableCommand GoToSearchPageCommand
        {
            get
            {
                return this.goToSearchPageCommand;
            }
            set
            {
                SetProperty(ref this.goToSearchPageCommand, value);
            }
        }

        public Location MapCenter
        {
            get;
            set;
        }

        public double BoundsWidth
        {
            get;
            set;
        }

        public double BoundsHeight
        {
            get;
            set;
        }

        public double ZoomLevel
        {
            get;
            set;
        }

        /// <summary>
        /// Called when the user goes back
        /// </summary>
        private Task OnGoBackCommandExecuted(object arg1, object arg2)
        {
            var currentFrame = Window.Current.Content as Frame;
            if (currentFrame != null && currentFrame.CanGoBack)
            {
                currentFrame.GoBack();
            }

            return Task.FromResult<object>(null);
        }


        /// <summary>
        /// Called when the go to main page command is executed.
        /// </summary>
        private Task OnGoToMainPageCommandExecuted(object arg1, object arg2)
        {
            var currentFrame = Window.Current.Content as Frame;
            if (currentFrame != null)
            {
                currentFrame.Navigate(typeof(MainPage));
            }

            // to do - navigate pages.
            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Called when the go to help page command is executed.
        /// </summary>
        private Task OnGoToHelpPageCommandExecuted(object arg1, object arg2)
        {
            var currentFrame = Window.Current.Content as Frame;
            if (currentFrame != null)
            {
                currentFrame.Navigate(typeof(HelpPage));
            }

            // to do - navigate pages.
            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Called when the go to search page command is executed.
        /// </summary>
        private Task OnGoToSearchPageCommandExecuted(object arg1, object arg2)
        {
            var pane = Windows.ApplicationModel.Search.SearchPane.GetForCurrentView();
            pane.Show();            

            return Task.FromResult<object>(null);
        }
    }
}
