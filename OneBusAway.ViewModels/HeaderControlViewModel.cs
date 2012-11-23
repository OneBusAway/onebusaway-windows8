using OneBusAway.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneBusAway.ViewModels
{
    public class HeaderControlViewModel : ViewModelBase
    {
        private bool favoriteIsEnabled;
        private string subText;

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

        public string SubText
        {
            get
            {
                return this.subText;
            }
            set
            {
                if (SetProperty(ref this.subText, value))
                {
                    // If the subtext is set, make sure we fire the HasSubText event to update the UI.
                    base.FirePropertyChanged("HasSubText");
                }
            }
        }

        public bool HasSubText
        {
            get
            {
                return !string.IsNullOrEmpty(this.subText);
            }
        }
    }
}
