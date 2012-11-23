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
        public BusStop(string stopId, string direction)
        {
            this.InitializeComponent();

            N.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            S.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            E.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            W.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            NW.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            NE.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            SW.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            SE.Visibility = Windows.UI.Xaml.Visibility.Collapsed;

            _stopId = stopId;

            switch (direction)
            {
                case "N":
                    N.Visibility = Windows.UI.Xaml.Visibility.Visible;
                    break;
                case "S":
                    S.Visibility = Windows.UI.Xaml.Visibility.Visible;
                    break;
                case "E":
                    E.Visibility = Windows.UI.Xaml.Visibility.Visible;
                    break;
                case "W":
                    W.Visibility = Windows.UI.Xaml.Visibility.Visible;
                    break;
                case "NW":
                    NW.Visibility = Windows.UI.Xaml.Visibility.Visible;
                    break;
                case "NE":
                    NE.Visibility = Windows.UI.Xaml.Visibility.Visible;
                    break;
                case "SW":
                    SW.Visibility = Windows.UI.Xaml.Visibility.Visible;
                    break;
                case "SE":
                    SE.Visibility = Windows.UI.Xaml.Visibility.Visible;
                    break;
                default:
                    break;
            }

        }

        private string _stopId;
        public string StopId 
        {
            get
            {
                return _stopId;
            }
        }
    }
}
