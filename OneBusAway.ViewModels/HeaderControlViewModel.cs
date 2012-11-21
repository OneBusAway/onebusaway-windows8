using OneBusAway.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneBusAway.ViewModels
{
    public class HeaderControlViewModel : BindableBase
    {
        private bool favoriteIsEnabled;

        public HeaderControlViewModel()
        {
        }

        public bool FavoritesIsEnabled
        {
            get
            {
                return this.favoriteIsEnabled;
            }
            set
            {
                SetProperty(ref this.favoriteIsEnabled, value);
            }
        }
    }
}
