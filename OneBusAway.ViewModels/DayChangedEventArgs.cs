using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneBusAway.ViewModels
{
    public class DayChangedEventArgs
    {
        public DayChangedEventArgs(int dayOfWeek)
        {
            this.DayOfWeek = dayOfWeek;
        }

        public int DayOfWeek
        {
            get;
            private set;
        }
    }
}
