using OneBusAway.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneBusAway.PageControls
{
    /// <summary>
    /// Defines a page control object.  These objects are created when a user navigates to a new 'page',
    /// which is really just a refreshed control inside the scroll viewer on the main page with a new data binding.
    /// </summary>
    public interface IPageControl
    {
        /// <summary>
        /// Page controls should be able to return their view model.
        /// </summary>
        PageViewModelBase ViewModel
        {
            get;
        }

        /// <summary>
        /// Pages should be able to initialize.
        /// </summary>
        Task InitializeAsync(object parameter);

        /// <summary>
        /// Pages should be represent themselves as a string of parameters.
        /// </summary>
        PageInitializationParameters GetParameters();
        
        /// <summary>
        /// Pages should be able to restore themselves.
        /// </summary>
        Task RestoreAsync();

        /// <summary>
        /// Pages should be able to refresh their data on demand.
        /// </summary>
        Task RefreshAsync();
    }
}
