using OneBusAway.Model;
using OneBusAway.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneBusAway
{
    /// <summary>
    /// This is a proxy class that wraps the NavigationController singleton 
    /// so that we can bind to it's items in xaml.
    /// </summary>
    public class NavigationControllerProxy : INotifyPropertyChanged
    {
        /// <summary>
        /// Creates the proxy.
        /// </summary>
        public NavigationControllerProxy()
        {
        }

        /// <summary>
        /// Make the property changed handler just route to the NavigationController singleton.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged
        {
            add
            {
                NavigationController.Instance.PropertyChanged += value;
            }
            remove
            {
                NavigationController.Instance.PropertyChanged -= value;
            }
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
            set
            {
                NavigationController.Instance.MapView = value;
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
        /// Gets the region.
        /// </summary>
        public string Region
        {
            get
            {
                return NavigationController.Instance.Region;
            }
        }

        /// <summary>
        /// Gets the region's default latitude.
        /// </summary>
        public double RegionDefaultLat
        {
            get
            {
                return NavigationController.Instance.RegionDefaultLat;
            }
        }

        /// <summary>
        /// Gets the region's default longitude.
        /// </summary>
        public double RegionDefaultLon
        {
            get
            {
                return NavigationController.Instance.RegionDefaultLon;
            }
        }

        /// <summary>
        /// Return the go back command.
        /// </summary>
        public ObservableCommand GoBackCommand
        {
            get
            {
                return NavigationController.Instance.GoBackCommand;
            }
        }

        /// <summary>
        /// Returns the refresh command.
        /// </summary>
        public ObservableCommand RefreshCommand
        {
            get
            {
                return NavigationController.Instance.RefreshCommand;
            }
        }

        /// <summary>
        /// Returns the go to users location command.
        /// </summary>
        public ObservableCommand GoToUsersLocationCommand
        {
            get
            {
                return NavigationController.Instance.GoToUsersLocationCommand;
            }
        }

        /// <summary>
        /// Returns the go to favorites page command.
        /// </summary>
        public ObservableCommand GoToFavoritesPageCommand
        {
            get
            {
                return NavigationController.Instance.GoToFavoritesPageCommand;
            }
        }

        /// <summary>
        /// Returns the real time page command.
        /// </summary>
        public ObservableCommand GoToRealTimePageCommand
        {
            get
            {
                return NavigationController.Instance.GoToRealTimePageCommand;
            }
        }

        /// <summary>
        /// Returns the go to help page command.
        /// </summary>
        public ObservableCommand GoToHelpPageCommand
        {
            get
            {
                return NavigationController.Instance.GoToHelpPageCommand;
            }
        }

        /// <summary>
        /// Returns the go to time table page command.
        /// </summary>
        public ObservableCommand GoToTimeTablePageCommand
        {
            get
            {
                return NavigationController.Instance.GoToTimeTablePageCommand;
            }
        }

        /// <summary>
        /// Returns the go to search page command.
        /// </summary>
        public ObservableCommand GoToSearchPageCommand
        {
            get
            {
                return NavigationController.Instance.GoToSearchPageCommand;
            }
        }

        public ObservableCommand AddToFavoritesCommand
        {
            get
            {
                return NavigationController.Instance.AddToFavoritesCommand;
            }
        }

        public ObservableCommand FilterByRouteCommand
        {
            get
            {
                return NavigationController.Instance.FilterByRouteCommand;
            }
        }

        /// <summary>
        /// Returns the go to trip details page command.
        /// </summary>
        public ObservableCommand GoToTripDetailsPageCommand
        {
            get
            {
                return NavigationController.Instance.GoToTripDetailsPageCommand;
            }
        }

    }
}
