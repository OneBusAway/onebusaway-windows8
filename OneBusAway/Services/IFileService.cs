using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneBusAway.Services
{
    public interface IFileService
    {
        Task<DateTimeOffset> GetFileCreatedTimeAsync(string relativePath);

        Task<DateTimeOffset> GetFileModifiedTimeAsync(string relativePath);

        Task<string> ReadFileAsStringAsync(string relativePath);

        Task<Stream> ReadFileAsStreamAsync(string relativePath);

        Task WriteFileAsync(string relativePath, Stream stream);
    }
}
