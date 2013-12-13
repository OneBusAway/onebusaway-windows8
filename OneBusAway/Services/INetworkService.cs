using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneBusAway.Services
{
    public interface INetworkService
    {
        Task<byte[]> ReadAsByteArrayAsync(string url);

        Task<string> ReadAsStringAsync(string url);

        Task<Stream> ReadAsStreamAsync(string url);
    }
}
