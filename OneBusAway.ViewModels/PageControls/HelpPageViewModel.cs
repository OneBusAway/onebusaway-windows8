<<<<<<< HEAD:OneBusAway.ViewModels/HelpPageViewModel.cs
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneBusAway.ViewModels
{
    public class HelpPageViewModel : PageViewModelBase
    {
        public HelpPageViewModel()
        {
            this.HeaderViewModel.SubText = "HELP";
        }

    }
}
=======
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneBusAway.ViewModels.PageControls
{
    public class HelpPageViewModel : PageViewModelBase
    {
        public HelpPageViewModel()
        {
            this.HeaderViewModel.SubText = "HELP";
        }
    }
}
>>>>>>> 5fa5eaa... Re-arranging the view models by moving page controls & user control view models into sudifferent namepsaces / folders.:OneBusAway.ViewModels/PageControls/HelpPageViewModel.cs
