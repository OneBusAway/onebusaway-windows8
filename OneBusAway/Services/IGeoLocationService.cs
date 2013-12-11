using OneBusAway.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneBusAway.Shared.Services
{
    public interface IGeoLocationService
    {
        Task<Point> FindUserLocationAsync();
    }
}
