using OneBusAway.Model;
using OneBusAway.Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneBusAway.Platforms.Windows8
{
    public class GeoLocationService : IGeoLocationService
    {
        public Task<Point> FindUserLocationAsync()
        {
            throw new NotImplementedException();
        }
    }
}
