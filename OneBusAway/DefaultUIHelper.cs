using OneBusAway.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace OneBusAway
{
    /// <summary>
    /// Passed into some view models to help them interact with the UI.
    /// </summary>
    public class DefaultUIHelper : IUIHelper
    {
        private CoreDispatcher dispatcher;

        /// <summary>
        /// Crteates the default UI helper.
        /// </summary>
        public DefaultUIHelper(CoreDispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
        }

        /// <summary>
        /// Batch-adds new items to the search result list in a way that prevents LayoutCycleExceptions. If 
        /// we add too many at once, WinRT thinks we've entered an infinite loop.
        /// </summary>
        public async Task BatchAddItemsAsync<T>(ObservableCollection<T> collection, IEnumerable<T> newItems, bool clear = true)
        {
            if (clear)
            {
                collection.Clear();
            }

            foreach (var item in newItems)
            {
                collection.Add(item);
                await Task.Delay(20);
            }
        }

        /// <summary>
        /// Allows view models to wait for the UI to idle.
        /// </summary>
        public async Task WaitForIdleAsync()
        {
            await this.dispatcher.RunIdleAsync(args => { });
        }
    }
}
