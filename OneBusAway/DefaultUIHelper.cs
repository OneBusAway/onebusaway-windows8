using OneBusAway.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace OneBusAway
{
    /// <summary>
    /// Passed into some view models to help them interact with the UI.
    /// </summary>
    public class DefaultUIHelper : IUIHelper
    {
        private CoreDispatcher dispatcher;

        /// <summary>
        /// Crteates the default UI helper.
        /// </summary>
        public DefaultUIHelper(CoreDispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
        }

        /// <summary>
        /// Allows view models to wait for the UI to idle.
        /// </summary>
        public async Task WaitForIdleAsync()
        {
            await this.dispatcher.RunIdleAsync(args => { });
        }
    }
}
