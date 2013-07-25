using OneBusAway.ViewModels.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneBusAway.ViewModels.PageControls
{
    public class PageViewModelBase : ViewModelBase
    {
        /// <summary>
        /// Header control for all pages
        /// </summary>
        private HeaderControlViewModel headerControlVM;

        /// <summary>
        /// Map control view model for all pages.
        /// </summary>
        private MapControlViewModel mapControlViewModel;

        /// <summary>
        /// Creates the base class.
        /// </summary>
        public PageViewModelBase()
        {
            this.HeaderViewModel = new HeaderControlViewModel();
            this.MapControlViewModel = new MapControlViewModel();
        }

        public HeaderControlViewModel HeaderViewModel
        {
            get
            {
                return this.headerControlVM;
            }
            set
            {
                SetProperty(ref this.headerControlVM, value);
            }
        }

        /// <summary>
        /// Gets / sets the map control view model.
        /// </summary>
        public virtual MapControlViewModel MapControlViewModel
        {
            get
            {
                return this.mapControlViewModel;
            }
            set
            {
                SetProperty(ref this.mapControlViewModel, value);
            }
        }
    }
}