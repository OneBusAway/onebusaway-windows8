/* Copyright 2014 Michael Braude and individual contributors.
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

using System.Collections;
using System.Collections.Generic;

#if WINDOWS_PHONE
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
#else
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
#endif

namespace OneBusAway.Triggers
{
    /// <summary>
    /// Trigger collection is a list of triggers.
    /// </summary>
#if WINDOWS_PHONE
    [ContentProperty("Triggers")]
#endif
    public class TriggerCollection : DependencyObject, IList<Trigger>
    {
        public static DependencyProperty TriggersProperty = DependencyProperty.Register("Triggers",
            typeof(List<Trigger>),
            typeof(TriggerCollection),
            new PropertyMetadata(null));

        public static DependencyProperty ControlProperty = DependencyProperty.Register("Control",
            typeof(Control),
            typeof(TriggerCollection),
            new PropertyMetadata(null, new PropertyChangedCallback(OnControlChanged)));

        /// <summary>
        /// This constructor is called from the phone project.
        /// </summary>
        public TriggerCollection()
        {
            this.Triggers = new List<Trigger>();
        }

        /// <summary>
        /// The owning control.
        /// </summary>
        public TriggerCollection(Control control)
        {
            this.Control = control;
            this.Triggers = new List<Trigger>();
        }

        public List<Trigger> Triggers
        {
            get
            {
                return (List<Trigger>)GetValue(TriggersProperty);
            }
            set
            {
                this.SetValue(TriggersProperty, value);
            }
        }

        public Control Control
        {
            get
            {
                return (Control)GetValue(ControlProperty);
            }
            set
            {
                SetValue(ControlProperty, value);
            }
        }

        /// <summary>
        /// Returns the index of a trigger.
        /// </summary>
        public int IndexOf(Trigger item)
        {
            return this.Triggers.IndexOf(item);
        }

        /// <summary>
        /// Inserts a trigger.
        /// </summary>
        public void Insert(int index, Trigger item)
        {
            this.Triggers.Insert(index, item);
        }

        /// <summary>
        /// Removes a trigger.
        /// </summary>
        public void RemoveAt(int index)
        {
            this.Triggers.RemoveAt(index);
        }

        /// <summary>
        /// Indexer.
        /// </summary>
        public Trigger this[int index]
        {
            get
            {
                return this.Triggers[index];
            }
            set
            {
                this.Triggers[index] = value;
            }
        }

        /// <summary>
        /// Adds an item. This is called from the Windows 8 runtime.
        /// </summary>
        public void Add(Trigger item)
        {
            this.Triggers.Add(item);
            item.SetValue(Trigger.ControlProperty, this.Control);
        }

        /// <summary>
        /// Clears the list.
        /// </summary>
        public void Clear()
        {
            this.Triggers.Clear();
        }

        /// <summary>
        /// Returns true if the collection contains an item.
        /// </summary>
        public bool Contains(Trigger item)
        {
            return this.Triggers.Contains(item);
        }

        /// <summary>
        /// Copies to an array.
        /// </summary>
        public void CopyTo(Trigger[] array, int arrayIndex)
        {
            this.Triggers.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Returns the number of items in the collection.
        /// </summary>
        public int Count
        {
            get 
            {
                return this.Triggers.Count;
            }
        }

        /// <summary>
        /// Not a read only list.
        /// </summary>
        public bool IsReadOnly
        {
            get 
            {
                return false;
            }
        }

        /// <summary>
        /// Removes an item.
        /// </summary>
        public bool Remove(Trigger item)
        {
            return this.Triggers.Remove(item);
        }

        /// <summary>
        /// Returns the enumerator.
        /// </summary>
        public IEnumerator<Trigger> GetEnumerator()
        {
            return this.Triggers.GetEnumerator();
        }

        /// <summary>
        /// Old style enumerator.
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.Triggers.GetEnumerator();
        }

        /// <summary>
        /// Called when the control property changes. Make sure we set the control property
        /// for all the triggers.
        /// </summary>
        private static void OnControlChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TriggerCollection collection = d as TriggerCollection;
            Control newControl = e.NewValue as Control;

            if (collection != null && newControl != null)
            {
                List<Trigger> triggers = collection.Triggers;
                if (triggers != null)
                {
                    foreach (var trigger in triggers)
                    {
                        trigger.SetValue(Trigger.ControlProperty, newControl);
                    }
                }
            }            
        }
    }
}
