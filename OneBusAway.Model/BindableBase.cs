using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OneBusAway.Model
{
    /// <summary>
    /// A generic, bindable base class
    /// </summary>
    public abstract class BindableBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Satisfies the notify property changed interface.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Creates the bindable base.
        /// </summary>
        public BindableBase()
        {
        }

        /// <summary>
        /// Sets a property.
        /// </summary>
        protected bool SetProperty<T>(ref T prop, T newValue, [CallerMemberName] string propName = null)
        {
            if (! object.Equals(prop, newValue))
            {
                prop = newValue;
                this.FirePropertyChanged(propName);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Fires a property changed event.
        /// </summary>
        private void FirePropertyChanged(string propName)
        {
            var propChanged = this.PropertyChanged;
            if (propChanged != null)
            {
                propChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
    }
}
