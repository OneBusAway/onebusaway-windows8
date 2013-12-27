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
using System.Collections.Generic;

#if WINDOWS_PHONE
using System.Windows;
using System.Windows.Controls;
#else
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
#endif

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
                control.Unloaded += OnControlUnloaded;
            }

            return collection;
        }

        public static void SetTriggers(Control control, TriggerCollection collection)
        {
            if (!instance.triggerLookup.ContainsKey(control))
            {
                instance.triggerLookup[control] = collection;
                control.Unloaded += OnControlUnloaded;
            }
        }

        /// <summary>
        /// Unregisters triggers when the control is unloaded.
        /// </summary>
        private static void OnControlUnloaded(object sender, RoutedEventArgs e)
        {
            Control control = sender as Control;
            if (control != null)
            {
                instance.triggerLookup.Remove(control);
            }
        }
    }
}
