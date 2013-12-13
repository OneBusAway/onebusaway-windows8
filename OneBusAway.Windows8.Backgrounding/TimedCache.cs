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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneBusAway.Backgrounding
{
    /// <summary>
    /// A class that keeps objects until they expire.
    /// </summary>
    internal class TimedCache
    {
        /// <summary>
        /// A dictionary that keeps track of items, along with their expiration time. Values
        /// expire after 15 minutes.
        /// </summary>
        private Dictionary<string, Tuple<DateTime, object>> objectLookup;

        /// <summary>
        /// Creates the cache.
        /// </summary>
        public TimedCache()
        {
            this.objectLookup = new Dictionary<string, Tuple<DateTime, object>>();
        }

        /// <summary>
        /// Clears the cache.
        /// </summary>
        public void Clear()
        {
            this.objectLookup.Clear();
        }

        /// <summary>
        /// Either gets the value from the cache or calls addFun to put it into the cache.
        /// </summary>
        public async Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> addFunc)
        {
            Tuple<DateTime, object> valuePair = null;
            if (this.objectLookup.TryGetValue(key, out valuePair))
            {
                if ((DateTime.Now - valuePair.Item1).TotalMinutes < 15)
                {
                    return (T)valuePair.Item2;
                }
            }

            T value = await addFunc();
            this.objectLookup[key] = new Tuple<DateTime, object>(DateTime.Now, value);
            return value;
        }
    }
}
