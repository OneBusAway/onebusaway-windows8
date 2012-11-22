using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneBusAway.ViewModels
{
    public class PageViewModelBase : ViewModelBase
    {
        /// <summary>
        /// Header control for all pages
        /// </summary>
        private HeaderControlViewModel headerControlVM;

        public PageViewModelBase()
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
