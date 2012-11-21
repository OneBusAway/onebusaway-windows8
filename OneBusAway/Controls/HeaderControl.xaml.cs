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
    public sealed partial class HeaderControl : UserControl
    {
        public HeaderControl()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// called when the help button is clicked
        /// </summary>
        private void OnHelpClicked(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Called when the search button is clicked.
        /// </summary>
        private void OnSearchClicked(object sender, RoutedEventArgs e)
        {

        }
    }
}
