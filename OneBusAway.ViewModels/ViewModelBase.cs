using OneBusAway.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneBusAway.ViewModels
{
    /// <summary>
    /// Base class for all view models.
    /// </summary>
    public class ViewModelBase : BindableBase
    {
        /// <summary>
        /// Header control for all pages
        /// </summary>
        private HeaderControlViewModel headerControlVM;

        public ViewModelBase()
        {
            this.HeaderViewModel = new HeaderControlViewModel();
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

    }
}
