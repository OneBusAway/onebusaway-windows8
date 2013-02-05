using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneBusAway.ViewModels
{
    /// <summary>
    /// Interface allows view models to interact with the UI in an abstacted way.
    /// </summary>
    public interface IUIHelper
    {
        /// <summary>
        /// Defines a method that will add items to an observable collection one at a time, waiting for 
        /// idle before each add.
        /// </summary>
        Task BatchAddItemsAsync<T>(ObservableCollection<T> collection, IEnumerable<T> newItems, bool clear = true);

        /// <summary>
        /// Allows view models to wait for idle asynchronously.
        /// </summary>
        Task WaitForIdleAsync();
    }
}
