/* Copyright 2013 Microsoft
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OneBusAway.ViewModels
{
    /// <summary>
    /// This is a wrapper around a long-living ObservableCommand. This helps with memory leaks - 
    /// the proxy can go away and the observable command can periodically remove any proxies 
    /// that it weakly holds.
    /// </summary>
    public class ObservableCommandProxy : ICommand
    {
        /// <summary>
        /// The actual command.
        /// </summary>
        private ObservableCommand command;

        /// <summary>
        /// Creates the command proxy.
        /// </summary>
        /// <param name="command">The command that actually executes</param>
        public ObservableCommandProxy(ObservableCommand command)
        {
            this.command = command;
            this.command.RegisterProxy(this, this.OnCommandsCanExecuteChanged);
        }

        /// <summary>
        /// Funnels the call to the command.
        /// </summary>
        public bool CanExecute(object parameter)
        {
            return this.command.CanExecute(parameter);
        }

        /// <summary>
        /// Fire this when the main command's CanExecuteChanged is fired.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="parameter">The parameter of the command.</param>
        public void Execute(object parameter)
        {
            this.command.Execute(parameter);
        }

        /// <summary>
        /// Called when the command's CanExecuteChanged is fired.
        /// </summary>
        private void OnCommandsCanExecuteChanged(object sender, EventArgs args)
        {
            var canExecuteChanged = this.CanExecuteChanged;
            if (canExecuteChanged != null)
            {
                canExecuteChanged(sender, args);
            }
        }
    }
}
