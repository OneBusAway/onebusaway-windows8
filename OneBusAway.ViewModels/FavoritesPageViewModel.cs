using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneBusAway.ViewModels
{
    /// <summary>
    /// View model for the favorites page.
    /// </summary>
    public class FavoritesPageViewModel : PageViewModelBase
    {
        /// <summary>
        /// View model for the routes and stops control.
        /// </summary>
        private RoutesAndStopsControlViewModel routesAndStopsViewModel;

        /// <summary>
        /// This event is fired when a bus stop is selected.
        /// </summary>
        public event EventHandler<StopSelectedEventArgs> StopSelected;

        /// <summary>
        /// Creates the favorites view model.
        /// </summary>
        public FavoritesPageViewModel()
        {
            this.RoutesAndStopsViewModel = new RoutesAndStopsControlViewModel();
            this.MapControlViewModel.StopSelected += OnMapControlViewModelStopSelected;
        }

        #region Public Properties

        /// <summary>
        /// The view model for the routes and stops page.
        /// </summary>
        public RoutesAndStopsControlViewModel RoutesAndStopsViewModel
        {
            get
            {
                return this.routesAndStopsViewModel;                
            }
            set
            {
                SetProperty(ref this.routesAndStopsViewModel, value);
            }
        }

        /// <summary>
        /// Tell any views listening that a stop was selected.
        /// </summary>
        protected void OnMapControlViewModelStopSelected(object sender, StopSelectedEventArgs e)
        {
            var stopSelected = this.StopSelected;
            if (stopSelected != null)
            {
                stopSelected(this, e);
            }
        }

        #endregion
    }
}
