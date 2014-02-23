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

using System;
using System.ComponentModel;
using OneBusAway.Converters;
using OneBusAway.ViewModels.Controls;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;

namespace OneBusAway.Controls
{
    public sealed partial class BusStop : UserControl
    {
        private static MapViewToVisibilityConverter mapViewToVisibilityConverter = new MapViewToVisibilityConverter();
        private static StringEqualsToVisibilityConverter stringEqualsToVisibilityConverter = new StringEqualsToVisibilityConverter();
        private static BoolToVisibilityConverter boolToVisibilityConverter = new BoolToVisibilityConverter();

        private static BitmapImage busBitmapImage = new BitmapImage(new Uri("ms-appx:///Assets/bus.png"));
        private static SolidColorBrush crimsonBrush = new SolidColorBrush(Colors.Crimson);
        private static SolidColorBrush redBrush = new SolidColorBrush(Colors.Red);

        private BusStopHeaderControl headerControl;
        private BusStopControlViewModel viewModel;
        private NavigationControllerProxy proxy;
        private Ellipse closestEllipse;

        public BusStop()
        {
            this.InitializeComponent();

            this.proxy = new NavigationControllerProxy();

            // For perf reasons, we do as much of this in code as we can as it is faster than relying on the xaml
            // decompiler. This constructor is a hot path so we need to be smart about how we create it:

            // This binding hides / shows the control depending on how far the user has zoomed:
            Binding canvasVisibilityBinding = new Binding()
            {
                Path = new PropertyPath("MapView"),
                Converter = mapViewToVisibilityConverter,
                Source = this.proxy
            };

            this.canvas.SetBinding(UIElement.VisibilityProperty, canvasVisibilityBinding);

            // Add the closest ellipse. When the view model is set we will
            // bind the visibility to it.
            this.closestEllipse = new Ellipse();
            this.closestEllipse.Width = 40;
            this.closestEllipse.Height = 40;
            this.closestEllipse.Opacity = .5;
            this.closestEllipse.Margin = new Thickness(-20, -20, 0, 0);
            this.closestEllipse.Fill = redBrush;
            this.closestEllipse.Visibility = Visibility.Collapsed;
            this.canvas.Children.Add(this.closestEllipse);

            // Add the image:
            Image busImage = new Image();
            busImage.Width = 28;
            busImage.Height = 28;
            busImage.Source = busBitmapImage;
            busImage.Margin = new Thickness(-14, -14, 0, 0);
            busImage.PointerPressed += OnPointerPressed;
            this.canvas.Children.Add(busImage);

            this.Unloaded += OnUnloaded;
        }

        /// <summary>
        /// For perf reasons, we don't databind the view model to the control because creating this control
        /// is a hot-path, and the bindings actually take a lot of time when 100s of these controls
        /// are being created. So instead of binding, we have this method. All we need to do here anyways is 
        /// set the directional and listen for the IsSelected property. 
        /// </summary>
        public BusStopControlViewModel ViewModel
        {
            get
            {
                return this.viewModel;
            }
            set
            {
                if (this.viewModel != null)
                {
                    this.viewModel.PropertyChanged -= OnViewModelPropertyChanged;
                }
                else
                {
                    // Add the directional polygon. It's much faster to only add one polygon
                    // to the control based on the direction of the view model. The direction will never change
                    // once the bus stop is created, so we don't need to update it if the view model changes.
                    if (string.Equals("SE", value.Direction, StringComparison.OrdinalIgnoreCase))
                    {
                        this.MakeDirectional(new Thickness(19, 12, 0, 0), 45);
                    }
                    else if (string.Equals("S", value.Direction, StringComparison.OrdinalIgnoreCase))
                    {
                        this.MakeDirectional(new Thickness(5, 17, 0, 0), 90);
                    }
                    else if (string.Equals("SW", value.Direction, StringComparison.OrdinalIgnoreCase))
                    {
                        this.MakeDirectional(new Thickness(-12, 19, 0, 0), 135);
                    }
                    else if (string.Equals("W", value.Direction, StringComparison.OrdinalIgnoreCase))
                    {
                        this.MakeDirectional(new Thickness(-16, 5, 0, 0), 180);
                    }
                    else if (string.Equals("NW", value.Direction, StringComparison.OrdinalIgnoreCase))
                    {
                        this.MakeDirectional(new Thickness(-19, -12, 0, 0), 225);
                    }
                    else if (string.Equals("N", value.Direction, StringComparison.OrdinalIgnoreCase))
                    {
                        this.MakeDirectional(new Thickness(-5, -17, 0, 0), -90);
                    }
                    else if (string.Equals("NE", value.Direction, StringComparison.OrdinalIgnoreCase))
                    {
                        this.MakeDirectional(new Thickness(12, -19, 0, 0), -45);
                    }
                    else if (string.Equals("E", value.Direction, StringComparison.OrdinalIgnoreCase))
                    {
                        this.MakeDirectional(new Thickness(16, -5, 0, 0), 0);
                    }
                }

                // Store the view model and wire up the property changed handler:
                this.viewModel = value;                
                this.viewModel.PropertyChanged += OnViewModelPropertyChanged;
                this.SetIsSelected(this.viewModel.IsSelected);

                Binding closestEllipseVisibilityBinding = new Binding()
                {
                    Path = new PropertyPath("IsClosestStop"),
                    Converter = boolToVisibilityConverter,
                    Source = this.viewModel,
                };

                this.closestEllipse.SetBinding(UIElement.VisibilityProperty, closestEllipseVisibilityBinding);
            }
        }

        /// <summary>
        /// Clears the view model when we're unloaded.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            if (this.viewModel != null)
            {
                this.viewModel.PropertyChanged -= OnViewModelPropertyChanged;
                this.viewModel = null;
            }
        }

        /// <summary>
        /// Utility method that makes directionals for the bus stop.
        /// </summary>
        private void MakeDirectional(Thickness margin, double angle)
        {
            Polygon polygon = new Polygon();
            polygon.Points = new PointCollection() { new Point(0, 0), new Point(8, 5), new Point(0, 10) };
            polygon.Fill = crimsonBrush;
            polygon.Margin = margin;
            polygon.RenderTransformOrigin = new Point(.5, .5);
            polygon.RenderTransform = new RotateTransform() { Angle = angle };
            this.canvas.Children.Add(polygon);
        }

        /// <summary>
        /// Invoked when a property changes on the view model.
        /// </summary>
        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (string.Equals("IsSelected", args.PropertyName, StringComparison.OrdinalIgnoreCase))
            {
                SetIsSelected(((BusStopControlViewModel)sender).IsSelected);
            }
        }

        /// <summary>
        /// Selects the bus stop if neccessary.
        /// </summary>
        private void SetIsSelected(bool isSelected)
        {
            this.SetValue(Canvas.ZIndexProperty, (isSelected) ? 100 : 0);
            if (isSelected && this.headerControl == null)
            {
                this.headerControl = new BusStopHeaderControl();
                this.headerControl.Margin = new Thickness(0, -55, 0, 0);
                this.headerControl.SetValue(Canvas.ZIndexProperty, 100);
                this.headerControl.SetStopName(this.viewModel.StopName);
                this.grid.Children.Add(this.headerControl);
            }
            else if (this.headerControl != null)
            {
                this.grid.Children.Remove(this.headerControl);
                this.headerControl = null;
            }
        }

        /// <summary>
        /// Fire the view models' event.
        /// </summary>
        private void OnPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (this.viewModel != null)
            {
                // Find the Map control that we're parented to:
                MapControl mapControl = ControlUtilities.GetParent<MapControl>(this.Parent);
                if (mapControl != null)
                {
                    MapControlViewModel mapControlViewModel = mapControl.DataContext as MapControlViewModel;
                    if (mapControlViewModel != null)
                    {
                        mapControlViewModel.SelectStop(this.viewModel);
                    }
                }
            }
        }
    }
}