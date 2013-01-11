using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace OneBusAway.Triggers
{
    /// <summary>
    /// Trigger manager is used to register trigger collections on the page.
    /// </summary>
    public class TriggerManager : DependencyObject
    {
        /// <summary>
        /// The one and only instance.
        /// </summary>
        public static TriggerManager instance = new TriggerManager();

        /// <summary>
        /// Triggers attached property definition.
        /// </summary>
        public static readonly DependencyProperty TriggersProperty = DependencyProperty.RegisterAttached("Triggers",
            typeof(List<Trigger>),
            typeof(TriggerManager),
            new PropertyMetadata(null));

        /// <summary>
        /// A lookup of controls to trigger collections.
        /// </summary>
        private Dictionary<Control, TriggerCollection> triggerLookup;

        /// <summary>
        /// Singleton ctor.
        /// </summary>
        private TriggerManager()
        {
            this.triggerLookup = new Dictionary<Control, TriggerCollection>();
        }

        /// <summary>
        /// Returns the trigger collection for a particular control.
        /// </summary>
        public static TriggerCollection GetTriggers(Control control)
        {
            TriggerCollection collection;
            if (!instance.triggerLookup.TryGetValue(control, out collection))
            {
                collection = new TriggerCollection(control);
                instance.triggerLookup[control] = collection;
            }

            return collection;
        }
    }
}
