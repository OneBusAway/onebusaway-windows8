using OneBusAway.Shared.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace OneBusAway.Platforms.Windows8
{
    public class FileService : IFileService
    {
        public async Task<DateTimeOffset> GetFileCreatedTimeAsync(string relativePath)
        {
            var file = await ApplicationData.Current.LocalFolder.GetFileAsync(relativePath);
            return file.DateCreated;
        }

        public async Task<DateTimeOffset> GetFileModifiedTimeAsync(string relativePath)
        {
            var file = await ApplicationData.Current.LocalFolder.GetFileAsync(relativePath);
            var properties = await file.GetBasicPropertiesAsync();
            return properties.DateModified;
        }

        public async Task<string> ReadFileAsStringAsync(string relativePath)
        {
            var file = await ApplicationData.Current.LocalFolder.GetFileAsync(relativePath);
            return await FileIO.ReadTextAsync(file);
        }

        public async Task<Stream> ReadFileAsStreamAsync(string relativePath)
        {
            var file = await ApplicationData.Current.LocalFolder.GetFileAsync(relativePath);
            var fileAccess = await file.OpenReadAsync();
            return fileAccess.AsStream();
        }

        public async Task WriteFileAsync(string relativePath, Stream stream)
        {
            StorageFile file = null;
            
            var storageItem = await ApplicationData.Current.LocalFolder.TryGetItemAsync(relativePath);
            if (storageItem == null)
            {
                file = await ApplicationData.Current.LocalFolder.CreateFileAsync(relativePath, CreationCollisionOption.ReplaceExisting);
            }
            else
            {
                file = await ApplicationData.Current.LocalFolder.GetFileAsync(relativePath);
            }

            using (var fileStream = await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                await stream.CopyToAsync(fileStream.AsStream(), (int)stream.Length);
            }
        }
    }
}
