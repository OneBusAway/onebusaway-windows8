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
    public sealed partial class BusStop : UserControl
    {
        public BusStop()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Fire the view models' event.
        /// </summary>
        private void OnPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            var viewModel = this.DataContext as BusStopControlViewModel;
            if (viewModel != null)
            {
                // Find the Map control that we're parented to:
                MapControl mapControl = GetParent<MapControl>();
                if (mapControl != null)
                {
                    MapControlViewModel mapControlViewModel = mapControl.DataContext as MapControlViewModel;
                    if (mapControlViewModel != null)
                    {
                        mapControlViewModel.SelectStop(viewModel);
                    }
                }
            }
        }

        /// <summary>
        /// Walks the logical tree until it finds type T.
        /// </summary>
        private T GetParent<T>()
            where T : DependencyObject
        {
            DependencyObject parent = VisualTreeHelper.GetParent(this.Parent);
            while (parent != null)
            {
                if (parent is T)
                {
                    break;
                }

                parent = VisualTreeHelper.GetParent(parent);
            }

            return parent as T;
        }
    }
}
