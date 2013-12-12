using OneBusAway.Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneBusAway.Platforms.Windows8
{
    public class NetworkService : INetworkService
    {
        public Task<byte[]> ReadAsByteArrayAsync(string url)
        {
            throw new NotImplementedException();
        }

        public Task<string> ReadAsStringAsync(string url)
        {
            throw new NotImplementedException();
        }

        public Task<System.IO.Stream> ReadAsStreamAsync(string url)
        {
            throw new NotImplementedException();
        }
    }
}
