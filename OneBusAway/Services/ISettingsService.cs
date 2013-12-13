using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneBusAway.Services
{
    public interface ISettingsService
    {
        bool Contains(string setting);

        string this[string setting] { get; set; }
    }
}
