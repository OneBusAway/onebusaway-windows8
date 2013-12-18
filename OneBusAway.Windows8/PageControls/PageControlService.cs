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
using OneBusAway.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneBusAway.PageControls
{
    public class PageControlService : IPageControlService
    {
        /// <summary>
        /// Nothing to do
        /// </summary>
        public PageControlService()
        {
        }

        /// <summary>
        /// Creates a page control instance.
        /// </summary>
        public IPageControl CreatePageControl(PageControlTypes pageControlType)
        {
            switch(pageControlType)
            {
                case PageControlTypes.Favorites:
                    return new FavoritesPageControl();

                case PageControlTypes.RealTime:
                    return new RealTimePageControl();

                case PageControlTypes.SearchResults:
                    return new SearchResultsPageControl();

                case PageControlTypes.TimeTable:
                    return new TimeTablePageControl();

                case PageControlTypes.TripDetails:
                    return new TripDetailsPageControl();

                default:
                    throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "Unsupported page control type {0}", pageControlType));
            }
        }
    }
}
