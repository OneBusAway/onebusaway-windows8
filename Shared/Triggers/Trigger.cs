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
using System;
using System.Reflection;

#if WINDOWS_PHONE
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
#else
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
#endif

namespace OneBusAway.Triggers
{
    /// <summary>
    /// A trigger represents a binding between a control, a property and a visual state to 
    /// transition to when the bound property changes to a specific value.
    /// </summary>
    public class Trigger : DependencyObject
    {
        /// <summary>
        /// The current value property.
        /// </summary>
        public static readonly DependencyProperty CurrentValueProperty = DependencyProperty.Register("CurrentValue",
            typeof(object),
            typeof(Trigger),
            new PropertyMetadata(null, new PropertyChangedCallback(OnCurrentValueChanged)));

        /// <summary>
        /// This is the control that the trigger is bound to.
        /// </summary>
        public static readonly DependencyProperty ControlProperty = DependencyProperty.Register("Control",
            typeof(Control),
            typeof(Trigger),
            new PropertyMetadata(null, new PropertyChangedCallback(OnControlChanged)));

        /// <summary>
        /// Creates the trigger.
        /// </summary>
        public Trigger()
        {
        }

        /// <summary>
        /// Gets / sets the binding.
        /// </summary>
        public Binding Binding
        {
            get;
            set;
        }

        /// <summary>
        /// This is the target value for the binding.
        /// </summary>
        public string Value
        {
            get;
            set;
        }

        /// <summary>
        /// This is the target visual state.
        /// </summary>
        public string VisualState
        {
            get;
            set;
        }

        /// <summary>
        /// Evaluates the trigger and changes state if neccessary.
        /// </summary>
        public void Evaluate()
        {
            
        }

        /// <summary>
        /// Called when the binding updates us.  Here is where we will change visual states.
        /// </summary>
        private static void OnCurrentValueChanged(DependencyObject depObject, DependencyPropertyChangedEventArgs args)
        {
            if(args.NewValue == null)
            {
                return;
            }

            Trigger trigger = (Trigger)depObject;
            TryTransitionToVisualState(trigger, args.NewValue);            
        }

        /// <summary>
        /// Called when the control property changes.  This is where we will set up the binding.
        /// </summary>
        private static void OnControlChanged(DependencyObject depObject, DependencyPropertyChangedEventArgs args)
        {
            Trigger trigger = (Trigger)depObject;
            Control newControl = args.NewValue as Control;

            if (newControl != null)
            {
                newControl.Loaded += (sender, loadedArgs) =>
                    {
                        Binding binding = trigger.Binding;

                        // First we need to do an initial analysis to see if the binding value equals the state.
                        object source = binding.Source;
                        string path = binding.Path.Path;

                        var propertyInfo = source.GetType().GetRuntimeProperty(path);
                        if (propertyInfo != null)
                        {
                            object value = propertyInfo.GetValue(source);
                            TryTransitionToVisualState(trigger, value);
                        }
                    };
            }

            BindingOperations.SetBinding(trigger,
                Trigger.CurrentValueProperty,
                trigger.Binding);            
        }

        /// <summary>
        /// Compares a value to the trigger and moves to a new visual state if they are equal.
        /// </summary>
        private static void TryTransitionToVisualState(Trigger trigger, object value)
        {
            if (value != null)
            {
                string newValue = value.ToString();

                if (string.Equals(trigger.Value, newValue, StringComparison.OrdinalIgnoreCase))
                {
                    Control control = (Control)trigger.GetValue(Trigger.ControlProperty);
                    if (control != null)
                    {
                        VisualStateManager.GoToState(control, trigger.VisualState, false);
                    }
                }
            }
        }
    }
}
