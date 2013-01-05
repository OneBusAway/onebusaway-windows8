using OneBusAway.Model;
using OneBusAway.Model.BingService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneBusAway.ViewModels
{
    /// <summary>
    /// View model that represents a search location result.
    /// </summary>
    public class SearchLocationResultViewModel : ViewModelBase
    {
        /// <summary>
        /// The route that we're based on.
        /// </summary>
        private Location location;

        /// <summary>
        /// True when this result is selected.
        /// </summary>
        private bool isSelected;

        /// <summary>
        /// Called when this view model is selected.
        /// </summary>
        private ObservableCommand selectedCommand;

        /// <summary>
        /// This event is fired whenever a location is selected.
        /// </summary>
        public event EventHandler<LocationSelectedEventArgs> LocationSelected;

        /// <summary>
        /// The location to base the view model on.
        /// </summary>
        public SearchLocationResultViewModel(Location location, EventHandler<LocationSelectedEventArgs> locationSelectedEventHandler)
        {
            this.Location = location;
            this.SelectedCommand = new ObservableCommand();
            this.SelectedCommand.Executed += OnSelectedCommandExecuted;
            this.LocationSelected += locationSelectedEventHandler;
        }

        /// <summary>
        /// Gets / sets the route.
        /// </summary>
        public Location Location
        {
            get
            {
                return this.location;
            }
            set
            {
                SetProperty(ref this.location, value);
            }
        }

        /// <summary>
        /// Truen when this view model is selected.
        /// </summary>
        public bool IsSelected
        {
            get
            {
                return this.isSelected;
            }
            set
            {
                SetProperty(ref this.isSelected, value);
            }
        }

        /// <summary>
        /// Returns the selected command.
        /// </summary>
        public ObservableCommand SelectedCommand
        {
            get
            {
                return this.selectedCommand;
            }
            set
            {
                SetProperty(ref this.selectedCommand, value);
            }
        }

        /// <summary>
        /// Fires when this view is selected.
        /// </summary>
        private Task OnSelectedCommandExecuted(object arg1, object arg2)
        {
            this.IsSelected = true;

            var locationSelected = this.LocationSelected;
            if (locationSelected != null)
            {
                locationSelected(this, new LocationSelectedEventArgs(Location));
            }

            return Task.FromResult<object>(null);
        }
    }
}
