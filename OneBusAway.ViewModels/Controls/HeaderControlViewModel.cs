<<<<<<< HEAD:OneBusAway.ViewModels/HeaderControlViewModel.cs
﻿using OneBusAway.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneBusAway.ViewModels
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
=======
﻿using OneBusAway.Model;
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
>>>>>>> 5fa5eaa... Re-arranging the view models by moving page controls & user control view models into sudifferent namepsaces / folders.:OneBusAway.ViewModels/Controls/HeaderControlViewModel.cs
