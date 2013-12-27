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
        /// <summary>
        /// A list of triggers.
        /// </summary>
        private List<Trigger> triggers;

        /// <summary>
        /// The owning control for this collection.
        /// </summary>
        private Control control;

        /// <summary>
        /// This constructor is called from the phone project.
        /// </summary>
        public TriggerCollection()
        {
        }

        /// <summary>
        /// The owning control.
        /// </summary>
        public TriggerCollection(Control control)
        {
            this.control = control;
            this.triggers = new List<Trigger>();
        }

        public List<Trigger> Triggers
        {
            get
            {
                return this.triggers;
            }
            set
            {
                this.triggers = value;
            }
        }

        /// <summary>
        /// Returns the index of a trigger.
        /// </summary>
        public int IndexOf(Trigger item)
        {
            return this.triggers.IndexOf(item);
        }

        /// <summary>
        /// Inserts a trigger.
        /// </summary>
        public void Insert(int index, Trigger item)
        {
            this.triggers.Insert(index, item);
        }

        /// <summary>
        /// Removes a trigger.
        /// </summary>
        public void RemoveAt(int index)
        {
            this.triggers.RemoveAt(index);
        }

        /// <summary>
        /// Indexer.
        /// </summary>
        public Trigger this[int index]
        {
            get
            {
                return this.triggers[index];
            }
            set
            {
                this.triggers[index] = value;
            }
        }

        /// <summary>
        /// Adds an item.
        /// </summary>
        public void Add(Trigger item)
        {
            this.triggers.Add(item);
            item.SetValue(Trigger.ControlProperty, this.control);
        }

        /// <summary>
        /// Clears the list.
        /// </summary>
        public void Clear()
        {
            this.triggers.Clear();
        }

        /// <summary>
        /// Returns true if the collection contains an item.
        /// </summary>
        public bool Contains(Trigger item)
        {
            return this.triggers.Contains(item);
        }

        /// <summary>
        /// Copies to an array.
        /// </summary>
        public void CopyTo(Trigger[] array, int arrayIndex)
        {
            this.triggers.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Returns the number of items in the collection.
        /// </summary>
        public int Count
        {
            get 
            {
                return this.triggers.Count;
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
            return this.triggers.Remove(item);
        }

        /// <summary>
        /// Returns the enumerator.
        /// </summary>
        public IEnumerator<Trigger> GetEnumerator()
        {
            return this.triggers.GetEnumerator();
        }

        /// <summary>
        /// Old style enumerator.
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.triggers.GetEnumerator();
        }
    }
}
