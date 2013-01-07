using OneBusAway.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace OneBusAway.PageControls
{
    /// <summary>
    /// Page control for real time info.
    /// </summary>
    public sealed partial class RealTimePageControl : UserControl, IPageControl
    {
        /// <summary>
        /// This is the view model for the real time page.
        /// </summary>
        private RealTimePageViewModel viewModel;

        /// <summary>
        /// Creates the real time page control.
        /// </summary>
        public RealTimePageControl()
        {
            this.InitializeComponent();
            this.viewModel = new RealTimePageViewModel();
        }

        /// <summary>
        /// Returns the view model for this page.
        /// </summary>
        public PageViewModelBase ViewModel
        {
            get 
            {
                return this.viewModel;
            }
        }

        /// <summary>
        /// Initializes the control.
        /// </summary>
        public async Task InitializeAsync(object parameter)
        {
            StopSelectedEventArgs stopSelectedEventArgs = parameter as StopSelectedEventArgs;

            if (stopSelectedEventArgs != null)
            {
                await this.viewModel.NavigateDirectlyToStop(
                    stopSelectedEventArgs.Latitude,
                    stopSelectedEventArgs.Longitude,
                    stopSelectedEventArgs.SelectedStopId,
                    stopSelectedEventArgs.StopName,
                    stopSelectedEventArgs.Direction);
            }
        }

        /// <summary>
        /// Restores asynchronously.
        /// </summary>
        public Task RestoreAsync()
        {
            return Task.FromResult<object>(null);
        }

    }
}
