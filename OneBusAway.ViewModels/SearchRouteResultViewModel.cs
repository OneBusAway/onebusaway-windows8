/* Copyright 2013 Microsoft
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneBusAway.ViewModels
{
    /// <summary>
    /// View model that represents a search route result.
    /// </summary>
    public class SearchRouteResultViewModel : ViewModelBase
    {
        /// <summary>
        /// The route that we're based on.
        /// </summary>
        private Route route;

        /// <summary>
        /// True when this result is selected.
        /// </summary>
        private bool isSelected;

        /// <summary>
        /// True when we are getting route data.
        /// </summary>
        private bool isGettingRouteData;

        /// <summary>
        /// Called when this view model is selected.
        /// </summary>
        private ObservableCommand selectedCommand;

        /// <summary>
        /// This event is fired whenever a route is selected.
        /// </summary>
        public event EventHandler<RouteSelectedEventArgs> RouteSelected;

        /// <summary>
        /// The route to base the view model on.
        /// </summary>
        public SearchRouteResultViewModel(Route route)
        {
            this.Route = route;
            this.SelectedCommand = new ObservableCommand();
            this.SelectedCommand.Executed += OnSelectedCommandExecuted;
        }

        /// <summary>
        /// Gets / sets the route.
        /// </summary>
        public Route Route
        {
            get
            {
                return this.route;
            }
            set
            {
                SetProperty(ref this.route, value);
            }
        }

        /// <summary>
        /// True when we are getting data for this route.
        /// </summary>
        public bool IsGettingRouteData
        {
            get
            {
                return this.isGettingRouteData;
            }
            set
            {
                SetProperty(ref this.isGettingRouteData, value);
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
        /// Selects this view model.
        /// </summary>
        public async Task SelectAsync()
        {
            await this.OnSelectedCommandExecuted(null, null);
        }

        /// <summary>
        /// Fires when this view is selected.
        /// </summary>
        private Task OnSelectedCommandExecuted(object arg1, object arg2)
        {
            this.IsSelected = true;

            var routeSelected = this.RouteSelected;
            if (routeSelected != null)
            {
                routeSelected(this, new RouteSelectedEventArgs(this.route.Id));
            }

            return Task.FromResult<object>(null);
        }
    }
}
