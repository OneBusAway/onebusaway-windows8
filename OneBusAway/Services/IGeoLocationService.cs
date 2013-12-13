using OneBusAway.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneBusAway.Services
{
    public interface IGeoLocationService
    {
        Task<Point> FindUserLocationAsync();
    }
}
