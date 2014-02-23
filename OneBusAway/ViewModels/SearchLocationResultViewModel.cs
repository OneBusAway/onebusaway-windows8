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
        public SearchLocationResultViewModel(Location location)
        {
            this.Location = location;
            this.SelectedCommand = new ObservableCommand();
            this.SelectedCommand.Executed += OnSelectedCommandExecuted;
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
