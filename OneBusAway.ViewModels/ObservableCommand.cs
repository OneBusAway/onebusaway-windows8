using OneBusAway.Model;
using System;
using System.Collections.Generic;
using System.Linq;
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
                    var canExecuteChangedEvent = CanExecuteChanged;
                    if (canExecuteChangedEvent != null)
                    {
                        canExecuteChangedEvent(this, new EventArgs());
                    }
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