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

using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using OneBusAway.Model;
using OneBusAway.ViewModels.Controls;

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
        private void OnStopTapped(object sender, System.Windows.Input.GestureEventArgs e)
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
                        var point = transform.Transform(new System.Windows.Point(0, 0));

                        double newVerticalOffset = point.Y + scrollViewer.VerticalOffset;
                        scrollViewer.ScrollToVerticalOffset(newVerticalOffset);
                    }
                }
            }
        }

    }
}