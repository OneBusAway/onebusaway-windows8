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
