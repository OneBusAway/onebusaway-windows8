using System;
using System.Collections.Generic;
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
        /// Allows view models to wait for idle asynchronously.
        /// </summary>
        Task WaitForIdleAsync();
    }
}
