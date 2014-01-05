using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OneBusAway.Services;

namespace OneBusAway.Platforms.WindowsPhone
{
    /// <summary>
    /// File service for windows phone.
    /// </summary>
    public class FileService : IFileService
    {
        public FileService()
        {
        }

        public Task<DateTimeOffset> GetFileCreatedTimeAsync(string relativePath)
        {
            var store = IsolatedStorageFile.GetUserStoreForApplication();
            return Task.FromResult<DateTimeOffset>(store.GetCreationTime(relativePath));
        }

        public Task<DateTimeOffset> GetFileModifiedTimeAsync(string relativePath)
        {
            var store = IsolatedStorageFile.GetUserStoreForApplication();
            return Task.FromResult<DateTimeOffset>(store.GetLastWriteTime(relativePath));
        }

        public async Task<string> ReadFileAsStringAsync(string relativePath)
        {
            var store = IsolatedStorageFile.GetUserStoreForApplication();
            using (var stream = new StreamReader(store.OpenFile(relativePath, FileMode.Open)))
            {
                return await stream.ReadToEndAsync();
            }
        }

        public Task<Stream> ReadFileAsStreamAsync(string relativePath)
        {
            var store = IsolatedStorageFile.GetUserStoreForApplication();
            return Task.FromResult<Stream>(store.OpenFile(relativePath, FileMode.Open));
        }

        public Task<Stream> OpenFileWriteStreamAsync(string relativePath)
        {
            var store = IsolatedStorageFile.GetUserStoreForApplication();

            // Create any directorties in the path:
            string temporaryPath = string.Empty;
            string[] directories = relativePath.Split(Path.DirectorySeparatorChar);
            foreach (string directory in directories)
            {
                if (! Path.HasExtension(directory))
                {
                    temporaryPath = Path.Combine(temporaryPath, directory);
                    if (! store.DirectoryExists(temporaryPath))
                    {
                        store.CreateDirectory(temporaryPath);
                    }
                }
            }

            return Task.FromResult<Stream>(store.OpenFile(relativePath, FileMode.Create));
        }
    }
}
