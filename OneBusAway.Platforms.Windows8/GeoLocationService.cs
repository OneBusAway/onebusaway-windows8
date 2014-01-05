/* Copyright 2013 Michael Braude and individual contributors.
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
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OneBusAway.Model;
using OneBusAway.Services;
using Windows.Devices.Geolocation;
using Windows.UI.Core;

namespace OneBusAway.Platforms.Windows8
{
    public class GeoLocationService : IGeoLocationService
    {
        private Geolocator locator;

        private List<WeakReference<IGeoLocationServiceObserver>> observers;

        private CoreDispatcher dispatcher;

        public GeoLocationService()
            : this(null)
        {
        }

        public GeoLocationService(CoreDispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
            this.observers = new List<WeakReference<IGeoLocationServiceObserver>>();

            this.locator = new Geolocator();
            this.locator.DesiredAccuracy = PositionAccuracy.High;
            this.locator.ReportInterval = 5000;

            // In some cases, ther emay be no dispatcher...for instance,
            // if we are in the background process. In that case, there's no point in listening
            // to the position changed event because there is no UI to report the change to anyways.
            if (this.dispatcher != null)
            {
                this.locator.PositionChanged += OnLocatorPositionChanged;
            }
        }

        /// <summary>
        /// Fire the OnUserLocationChanged event if somebody has subscribed. 
        /// NOTE: this method is ALWAYS called on a background thread!
        /// </summary>
        private async void OnLocatorPositionChanged(object sender, PositionChangedEventArgs args)
        {
            Geopoint geoPoint = args.Position.Coordinate.Point;
            Point newLocation = new Point(geoPoint.Position.Latitude, geoPoint.Position.Longitude);

            // Count backwards so that we can remove references from the list:
            for (int i = this.observers.Count - 1; i >= 0; i--)
            {
                IGeoLocationServiceObserver observer;
                if (observers[i].TryGetTarget(out observer))
                {
                    await dispatcher.RunAsync(CoreDispatcherPriority.High, () => observer.OnUserLocationChanged(newLocation));
                }
                else
                {
                    observers.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Registers the observer with the list of observers to update.
        /// </summary>
        public void RegisterForLocationChanged(IGeoLocationServiceObserver observer)
        {
            this.observers.Add(new WeakReference<IGeoLocationServiceObserver>(observer));
        }

        public async Task<Point> FindUserLocationAsync()
        {   
            Geoposition geoPosition = await this.locator.GetGeopositionAsync();
            BasicGeoposition position = geoPosition.Coordinate.Point.Position;
            return new Point(position.Latitude, position.Longitude);
        }
    }
}
