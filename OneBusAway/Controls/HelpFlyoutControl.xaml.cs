using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
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
    public sealed partial class HelpFlyoutControl : UserControl
    {
        public HelpFlyoutControl()
        {
            this.InitializeComponent();
            Window.Current.CoreWindow.KeyDown += OnWindowKeyDown;
        }

        /// <summary>
        /// Shows the flyout.
        /// </summary>
        public void Show()
        {
            double screenWidth = Window.Current.Bounds.Width;
            double screenHeight = Window.Current.Bounds.Height;
            mainBorder.Height = screenHeight;
                        
            // Put the flyout on the left-hand side of the screen:
            Canvas.SetLeft(popup, screenWidth - popup.Width);
            this.popup.IsOpen = true;
        }

        /// <summary>
        /// Close the flyout if neccessary.
        /// </summary>
        private void OnWindowKeyDown(CoreWindow sender, KeyEventArgs args)
        {
            this.popup.IsOpen = false;
        }

        /// <summary>
        /// Close the popup.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBackButtonClicked(object sender, RoutedEventArgs e)
        {
            this.popup.IsOpen = false;
        }

        /// <summary>
        /// Opens the mail app for the user to send us feedback.
        /// </summary>
        private async void OnFeedbackLinkClicked(object sender, RoutedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri("mailto:onebusaway@outlook.com"));
        }
    }
}
