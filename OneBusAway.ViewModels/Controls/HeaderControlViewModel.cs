using OneBusAway.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneBusAway.ViewModels.Controls
{
    public class HeaderControlViewModel : ViewModelBase
    {
        private string subText;

        public HeaderControlViewModel()
        {
        }

        public string SubText
        {
            get
            {
                return (string.IsNullOrEmpty(this.subText))
                    ? "ONE BUS AWAY"
                    : this.subText;
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