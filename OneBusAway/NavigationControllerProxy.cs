using OneBusAway.Model;
using OneBusAway.PageControls;
using OneBusAway.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OneBusAway
{
    /// <summary>
    /// This is a proxy class that wraps the NavigationController singleton 
    /// so that we can bind to it's items in xaml.
    /// </summary>
    public class NavigationControllerProxy : INotifyPropertyChanged
    {
        /// <summary>
        /// This is the event that everybody registers.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Creates the proxy.
        /// </summary>
        public NavigationControllerProxy()
        {
            NavigationController.Instance.RegisterProxy(this, this.OnControllerPropertyChanged);
        }

        /// <summary>
        /// Returns the map view of the current map.
        /// </summary>
        public MapView MapView
        {
            get
            {
                return NavigationController.Instance.MapView;
            }
        }

        /// <summary>
        /// Returns the current page control.
        /// </summary>
        public IPageControl CurrentPageControl
        {
            get
            {
                return NavigationController.Instance.CurrentPageControl;
            }
        }

        /// <summary>
        /// Returns true if the user can go backwards.
        /// </summary>
        public bool CanGoBack
        {
            get
            {
                return NavigationController.Instance.CanGoBack;
            }
        }

        /// <summary>
        /// Returns true when the app is in a snapped state.
        /// </summary>
        public bool IsSnapped
        {
            get
            {
                return NavigationController.Instance.IsSnapped;
            }
        }

        /// <summary>
        /// Returns true when the app is in portrait mode.
        /// </summary>
        public bool IsPortrait
        {
            get
            {
                return NavigationController.Instance.IsPortrait;
            }
        }

        /// <summary>
        /// Returns true if the current control is pinned.
        /// </summary>
        public bool IsCurrentControlPinned
        {
            get
            {
                return NavigationController.Instance.IsCurrentControlPinned;
            }
        }

        /// <summary>
        /// Return the go back command.
        /// </summary>
        public ICommand GoBackCommand
        {
            get
            {
                return new ObservableCommandProxy(NavigationController.Instance.GoBackCommand);
            }
        }

        /// <summary>
        /// Returns the refresh command.
        /// </summary>
        public ICommand RefreshCommand
        {
            get
            {
                return new ObservableCommandProxy(NavigationController.Instance.RefreshCommand);
            }
        }

        /// <summary>
        /// Returns the go to users location command.
        /// </summary>
        public ICommand GoToUsersLocationCommand
        {
            get
            {
                return new ObservableCommandProxy(NavigationController.Instance.GoToUsersLocationCommand);
            }
        }

        /// <summary>
        /// Returns the pin stop to start page command.
        /// </summary>
        public ICommand PinStopToStartPageCommand
        {
            get
            {
                return new ObservableCommandProxy(NavigationController.Instance.PinStopToStartPageCommand);
            }
        }

        /// <summary>
        /// Returns the pin stop to start page command.
        /// </summary>
        public ICommand UnPinStopToStartPageCommand
        {
            get
            {
                return new ObservableCommandProxy(NavigationController.Instance.UnPinStopToStartPageCommand);
            }
        }

        /// <summary>
        /// Returns the go to favorites page command.
        /// </summary>
        public ICommand GoToFavoritesPageCommand
        {
            get
            {
                return new ObservableCommandProxy(NavigationController.Instance.GoToFavoritesPageCommand);
            }
        }

        /// <summary>
        /// Returns the real time page command.
        /// </summary>
        public ICommand GoToRealTimePageCommand
        {
            get
            {
                return new ObservableCommandProxy(NavigationController.Instance.GoToRealTimePageCommand);
            }
        }

        /// <summary>
        /// Returns the go to help page command.
        /// </summary>
        public ICommand GoToHelpPageCommand
        {
            get
            {
                return new ObservableCommandProxy(NavigationController.Instance.GoToHelpPageCommand);
            }
        }

        /// <summary>
        /// Returns the go to time table page command.
        /// </summary>
        public ICommand GoToTimeTablePageCommand
        {
            get
            {
                return new ObservableCommandProxy(NavigationController.Instance.GoToTimeTablePageCommand);
            }
        }

        /// <summary>
        /// Returns the go to search page command.
        /// </summary>
        public ICommand GoToSearchPageCommand
        {
            get
            {
                return new ObservableCommandProxy(NavigationController.Instance.GoToSearchPageCommand);
            }
        }

        /// <summary>
        /// Adds an item to the favorites list.
        /// </summary>
        public ICommand AddToFavoritesCommand
        {
            get
            {
                return new ObservableCommandProxy(NavigationController.Instance.AddToFavoritesCommand);
            }
        }

        /// <summary>
        /// Filters by route.
        /// </summary>
        public ICommand FilterByRouteCommand
        {
            get
            {
                return new ObservableCommandProxy(NavigationController.Instance.FilterByRouteCommand);
            }
        }

        /// <summary>
        /// Returns the go to trip details page command.
        /// </summary>
        public ICommand GoToTripDetailsPageCommand
        {
            get
            {
                return new ObservableCommandProxy(NavigationController.Instance.GoToTripDetailsPageCommand);
            }
        }

        /// <summary>
        /// Called when the navigation controller fires an event.
        /// </summary>
        private void OnControllerPropertyChanged(object sender, PropertyChangedEventArgs changedArgs)
        {
            var propChanged = this.PropertyChanged;
            if (propChanged != null)
            {
                propChanged(sender, changedArgs);
            }
        }
    }
}
