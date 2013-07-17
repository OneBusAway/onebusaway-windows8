<<<<<<< HEAD
﻿using OneBusAway.Model;
using OneBusAway.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace OneBusAway.Controls
{
    public sealed partial class TripTimelineControl : UserControl
    {
        public TripTimelineControl()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Called when the user selects a stop.
        /// </summary>
        private void OnStopClicked(object sender, PointerRoutedEventArgs e)
        {
            var viewModel = this.DataContext as TripTimelineControlViewModel;
            if (viewModel != null)
            {
                var stop = ((Control)sender).DataContext as TripStop;
                if (stop != null)
                {
                    viewModel.SelectNewStop(stop);
                }
            }
        }

        /// <summary>
        /// Called when the datatemplate in the itemscontrol loads. Here, we see whether the bound
        /// TripStop is selected or not, and if it is, we then scroll to it.
        /// </summary>
        private void OnItemsControlTemplateLoaded(object sender, RoutedEventArgs e)
        {
            Grid grid = (Grid)sender;
            TripStop tripStop = grid.DataContext as TripStop;

            if (tripStop != null && tripStop.IsSelectedStop)
            {
                this.ScrollToSelectedTripStop();
            }
        }

        /// <summary>
        /// Scrolls the items control to the selected trip stop.
        /// </summary>
        public void ScrollToSelectedTripStop()
        {
            var viewModel = this.DataContext as TripTimelineControlViewModel;
            if (viewModel != null)
            {
                var tripStop = viewModel.SelectedStop;
                if (tripStop != null)
                {
                    var contentPresenter = this.itemsControl.ItemContainerGenerator.ContainerFromItem(tripStop) as ContentPresenter;
                    if (contentPresenter != null)
                    {
                        ScrollViewer scrollViewer = ControlUtilities.GetParent<ScrollViewer>(this.Parent);
                        if (scrollViewer != null)
                        {
                            // So now we need to find where this stop is in the control:
                            var transform = contentPresenter.TransformToVisual(scrollViewer);
                            var point = transform.TransformPoint(new Windows.Foundation.Point(0, 0));

                            double newVerticalOffset = point.Y + scrollViewer.VerticalOffset;
                            scrollViewer.ScrollToVerticalOffset(newVerticalOffset);
                        }
                    }
                }
            }
        }        
    }
}
=======
﻿using OneBusAway.Model;
using OneBusAway.ViewModels;
using OneBusAway.ViewModels.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace OneBusAway.Controls
{
    public sealed partial class TripTimelineControl : UserControl
    {
        public TripTimelineControl()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Called when the user selects a stop.
        /// </summary>
        private void OnStopClicked(object sender, PointerRoutedEventArgs e)
        {
            var viewModel = this.DataContext as TripTimelineControlViewModel;
            if (viewModel != null)
            {
                var stop = ((Control)sender).DataContext as TripStop;
                if (stop != null)
                {
                    viewModel.SelectNewStop(stop);
                }
            }
        }

        /// <summary>
        /// Called when the datatemplate in the itemscontrol loads. Here, we see whether the bound
        /// TripStop is selected or not, and if it is, we then scroll to it.
        /// </summary>
        private void OnItemsControlTemplateLoaded(object sender, RoutedEventArgs e)
        {
            Grid grid = (Grid)sender;
            TripTimelineControlViewModel viewModel = (TripTimelineControlViewModel)this.DataContext;

            // Scroll to where the bus is, if we can. Otherwise, scroll to the selected stop:
            var closestStop = (from tripStop in viewModel.TripDetails.TripStops
                               where tripStop.IsClosestStop
                               select tripStop).FirstOrDefault();

            if (closestStop != null)
            {
                this.ScrollToTripStop(closestStop);
            }
            else
            {
                TripStop tripStop = grid.DataContext as TripStop;
                if (tripStop != null && tripStop.IsSelectedStop)
                {
                    this.ScrollToTripStop(tripStop);
                }
            }
        }

        /// <summary>
        /// Scrolls the items control to the selected trip stop.
        /// </summary>
        public void ScrollToSelectedTripStop()
        {
            var viewModel = this.DataContext as TripTimelineControlViewModel;
            if (viewModel != null)
            {
                var tripStop = viewModel.SelectedStop;
                ScrollToTripStop(tripStop);
            }
        }

        /// <summary>
        /// Scrolls to a specific trip stop.
        /// </summary>
        private void ScrollToTripStop(TripStop tripStop)
        {
            if (tripStop != null)
            {
                var contentPresenter = this.itemsControl.ItemContainerGenerator.ContainerFromItem(tripStop) as ContentPresenter;
                if (contentPresenter != null)
                {
                    ScrollViewer scrollViewer = ControlUtilities.GetParent<ScrollViewer>(this.Parent);
                    if (scrollViewer != null)
                    {
                        // So now we need to find where this stop is in the control:
                        var transform = contentPresenter.TransformToVisual(scrollViewer);
                        var point = transform.TransformPoint(new Windows.Foundation.Point(0, 0));

                        double newVerticalOffset = point.Y + scrollViewer.VerticalOffset;
                        scrollViewer.ScrollToVerticalOffset(newVerticalOffset);
                    }
                }
            }
        }        
    }
}
>>>>>>> 5fa5eaa... Re-arranging the view models by moving page controls & user control view models into sudifferent namepsaces / folders.
