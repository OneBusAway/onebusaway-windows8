/* Copyright 2013 Michael Braude and individual contributors.
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
using OneBusAway.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OneBusAway.ViewModels
{
    /// <summary>
    /// A generic command that takes a delegate.
    /// </summary>
    public class ObservableCommand : BindableBase, ICommand
    {
        /// <summary>
        /// A list of command proxies.
        /// </summary>
        private List<WeakReference<ObservableCommandProxy>> proxies;

        /// <summary>
        /// A list of proxy handlers.
        /// </summary>
        private ConditionalWeakTable<ObservableCommandProxy, EventHandler> proxyHandlers;

        /// <summary>
        /// Fires when CanExecute changes.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// This event is invoked when this command is executed.
        /// </summary>
        public event Func<object, object, Task> Executed;
                
        /// <summary>
        /// A method that we call to determine whether we can execute.
        /// </summary>
        private Func<object, bool> canExecuteFunc;

        /// <summary>
        /// True when the command can execute.
        /// </summary>
        private bool enabled;

        /// <summary>
        /// Executes an action.
        /// </summary>
        public ObservableCommand(Func<object, bool> canExecuteFunc = null, bool enabled = true)
        {
            this.enabled = enabled;
            this.canExecuteFunc = canExecuteFunc;
            this.proxies = new List<WeakReference<ObservableCommandProxy>>();
            this.proxyHandlers = new ConditionalWeakTable<ObservableCommandProxy, EventHandler>();
        }

        /// <summary>
        /// Allows callers to set whether the command can be executed.
        /// </summary>
        public bool Enabled
        {
            get
            {
                return this.enabled;
            }
            set
            {
                if (base.SetProperty(ref this.enabled, value))
                {
                    var canExecuteChanged = this.CanExecuteChanged;
                    if (canExecuteChanged != null)
                    {
                        canExecuteChanged(this, EventArgs.Empty);
                    }

                    // Invoke the weakly held proxies:
                    foreach (var weakReference in this.proxies)
                    {
                        EventHandler handler = null;
                        ObservableCommandProxy proxy = null;

                        if (weakReference.TryGetTarget(out proxy) && this.proxyHandlers.TryGetValue(proxy, out handler))
                        {
                            handler(this, EventArgs.Empty);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Registers a proxy with the command.
        /// </summary>
        public void RegisterProxy(ObservableCommandProxy proxy, EventHandler handler)
        {
            this.proxies.Add(new WeakReference<ObservableCommandProxy>(proxy));
            this.proxyHandlers.Add(proxy, handler);
        }

        /// <summary>
        /// Cleans up any weakly held references.
        /// </summary>
        public void CleanupUnheldProxies()
        {
            // Invoke the weakly held proxies:
            for (int i = this.proxies.Count - 1; i >= 0; i--)
            {
                ObservableCommandProxy proxy = null;
                WeakReference<ObservableCommandProxy> weakReference = this.proxies[i];

                if (!weakReference.TryGetTarget(out proxy))
                {
                    this.proxies.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Called to determine whether we can execute or not.
        /// </summary>
        public bool CanExecute(object parameter)
        {
            return (canExecuteFunc != null) ? canExecuteFunc(parameter) : true;
        }

        /// <summary>
        /// Call the action command.
        /// </summary>
        public async void Execute(object parameter)
        {
            var executed = this.Executed;
            if (executed != null)
            {
                await executed(this, parameter);
            }
        }        
    }
}