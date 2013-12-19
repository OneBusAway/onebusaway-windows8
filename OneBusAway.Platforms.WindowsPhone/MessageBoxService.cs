using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using OneBusAway.Services;

namespace OneBusAway.Platforms.WindowsPhone
{
    public class MessageBoxService : IMessageBoxService
    {
        public MessageBoxService()
        {
        }

        public Task ShowAsync(string title, string message)
        {
            MessageBox.Show(title, message, MessageBoxButton.OK);
            return Task.FromResult<object>(null);
        }
    }
}
