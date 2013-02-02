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
    /// <summary>
    /// User control that defines the header for a bus stop control.  This control is typically 
    /// added to a bus stop when it is selected and removed when it is unselected.
    /// </summary>
    public sealed partial class BusStopHeaderControl : UserControl
    {
        /// <summary>
        /// Creates the control.
        /// </summary>
        public BusStopHeaderControl()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Sets the stop name.
        /// </summary>
        public void SetStopName(string stopName)
        {
            stopNameTextBlock.Text = stopName;
        }
    }
}
