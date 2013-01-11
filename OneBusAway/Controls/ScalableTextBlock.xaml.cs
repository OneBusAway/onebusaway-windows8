using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace OneBusAway.Controls
{
    /// <summary>
    /// Defines an object that resizes text based on the width of the app.
    /// </summary>
    public sealed partial class ScalableTextBlock : UserControl
    {
        public static readonly DependencyProperty LargeFontSizeProperty = DependencyProperty.Register("LargeFontSize", 
            typeof(int), 
            typeof(ScalableTextBlock), 
            new PropertyMetadata(14));
        
        public static readonly DependencyProperty NormalFontSizeProperty = DependencyProperty.Register("NormalFontSize", 
            typeof(int), 
            typeof(ScalableTextBlock), 
            new PropertyMetadata(12));
        
        public static readonly DependencyProperty SnappedFontSizeProperty = DependencyProperty.Register("SnappedFontSize", 
            typeof(int), 
            typeof(ScalableTextBlock), 
            new PropertyMetadata(10));
        
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", 
            typeof(string), 
            typeof(ScalableTextBlock), 
            new PropertyMetadata(null));

        public ScalableTextBlock()
        {
            this.InitializeComponent();
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public int SnappedFontSize
        {
            get { return (int)GetValue(SnappedFontSizeProperty); }
            set { SetValue(SnappedFontSizeProperty, value); }
        }

        public int NormalFontSize
        {
            get { return (int)GetValue(NormalFontSizeProperty); }
            set { SetValue(NormalFontSizeProperty, value); }
        }

        public int LargeFontSize
        {
            get { return (int)GetValue(LargeFontSizeProperty); }
            set { SetValue(LargeFontSizeProperty, value); }
        }
    }
}
