using OneBusAway.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OneBusAway.Platforms.Windows8
{
    public class NetworkService : INetworkService
    {
        public const int TIMEOUT_LENGTH = 15000;

        public async Task<byte[]> ReadAsByteArrayAsync(string url)
        {
            using (CancellationTokenSource source = new CancellationTokenSource(TIMEOUT_LENGTH))
            {
                using (HttpClient client = new HttpClient())
                {
                    var message = await client.GetAsync(url, source.Token);
                    return await message.Content.ReadAsByteArrayAsync();
                }
            }
        }

        public async Task<string> ReadAsStringAsync(string url)
        {
            using (CancellationTokenSource source = new CancellationTokenSource(TIMEOUT_LENGTH))
            {
                using (HttpClient client = new HttpClient())
                {
                    var message = await client.GetAsync(url, source.Token);
                    return await message.Content.ReadAsStringAsync();
                }
            }
        }

        public async Task<Stream> ReadAsStreamAsync(string url)
        {
            using (CancellationTokenSource source = new CancellationTokenSource(TIMEOUT_LENGTH))
            {
                using (HttpClient client = new HttpClient())
                {
                    var message = await client.GetAsync(url, source.Token);
                    return await message.Content.ReadAsStreamAsync();
                }
            }
        }
    }
}
