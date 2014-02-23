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

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace OneBusAway.Controls
{
    /// <summary>
    /// Defines an object that resizes text based on the width of the app.
    /// </summary>
    public sealed partial class ScalableTextBlock : UserControl
    {
        public static readonly DependencyProperty LargeFontSizeProperty = DependencyProperty.Register("LargeFontSize",
            typeof(double),
            typeof(ScalableTextBlock),
            new PropertyMetadata(14.0));

        public static readonly DependencyProperty NormalFontSizeProperty = DependencyProperty.Register("NormalFontSize",
            typeof(double),
            typeof(ScalableTextBlock),
            new PropertyMetadata(12.0));

        public static readonly DependencyProperty SnappedFontSizeProperty = DependencyProperty.Register("SnappedFontSize",
            typeof(double),
            typeof(ScalableTextBlock),
            new PropertyMetadata(10.0));

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text",
            typeof(string),
            typeof(ScalableTextBlock),
            new PropertyMetadata(null));

        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command",
            typeof(ICommand),
            typeof(ScalableTextBlock),
            new PropertyMetadata(null));

        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register("CommandParameter",
            typeof(object),
            typeof(ScalableTextBlock),
            new PropertyMetadata(null));

        public static readonly DependencyProperty VerticalTextAlignmentProperty = DependencyProperty.Register("VerticalTextAlignment",
            typeof(VerticalAlignment),
            typeof(ScalableTextBlock),
            new PropertyMetadata(VerticalAlignment.Top));

        public static readonly DependencyProperty HorizontalTextAlignmentProperty = DependencyProperty.Register("HorizontalTextAlignment",
            typeof(HorizontalAlignment),
            typeof(ScalableTextBlock),
            new PropertyMetadata(HorizontalAlignment.Left));

        /// <summary>
        /// Navigation controller proxy.
        /// </summary>
        private NavigationControllerProxy proxy;

        public ScalableTextBlock()
        {
            this.InitializeComponent();

            this.proxy = new NavigationControllerProxy();
            this.proxy.PropertyChanged += OnProxyPropertyChanged;
            this.Unloaded += (sender, args) => this.proxy.PropertyChanged -= OnProxyPropertyChanged;
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public double SnappedFontSize
        {
            get { return (double)GetValue(SnappedFontSizeProperty); }
            set { SetValue(SnappedFontSizeProperty, value); }
        }

        public double NormalFontSize
        {
            get { return (double)GetValue(NormalFontSizeProperty); }
            set { SetValue(NormalFontSizeProperty, value); }
        }

        public double LargeFontSize
        {
            get { return (double)GetValue(LargeFontSizeProperty); }
            set { SetValue(LargeFontSizeProperty, value); }
        }

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        public VerticalAlignment VerticalTextAlignment
        {
            get { return (VerticalAlignment)GetValue(VerticalTextAlignmentProperty); }
            set { SetValue(VerticalTextAlignmentProperty, value); }
        }

        public HorizontalAlignment HorizontalTextAlignment
        {
            get { return (HorizontalAlignment)GetValue(HorizontalTextAlignmentProperty); }
            set { SetValue(HorizontalTextAlignmentProperty, value); }
        }

        /// <summary>
        /// Called when a property changes on the navigation proxy. Since this is a hot path, it's cheaper 
        /// to implement these events manually instead of binding via the triggers system in xaml.
        /// </summary>
        private void OnProxyPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (string.Equals("IsSnapped", e.PropertyName, StringComparison.OrdinalIgnoreCase))
            {
                CalculateState();
            }
        }

        /// <summary>
        /// Calculates the state of the text block based on whether we are snapped or not.
        /// </summary>
        private void CalculateState()
        {
            if (this.proxy.IsSnapped)
            {
                this.textBlock.FontSize = this.SnappedFontSize;
                this.textBlock.TextTrimming = TextTrimming.WordEllipsis;
                this.textBlock.TextWrapping = TextWrapping.NoWrap;
            }
            else
            {
                this.textBlock.FontSize = this.NormalFontSize;
                this.textBlock.TextTrimming = TextTrimming.None;
                this.textBlock.TextWrapping = TextWrapping.Wrap;
            }
        }

        /// <summary>
        /// Called when the user control is loaded.
        /// </summary>
        private void OnUserControlLoaded(object sender, RoutedEventArgs e)
        {
            this.CalculateState();
        }

        /// <summary>
        /// If there is a command bound to this text block, invoke it.
        /// </summary>
        private void OnTapped(object sender, GestureEventArgs e)
        {
            if (this.Command != null && this.Command.CanExecute(this.CommandParameter))
            {
                this.Command.Execute(this.CommandParameter);
            }
        }
    }
}
