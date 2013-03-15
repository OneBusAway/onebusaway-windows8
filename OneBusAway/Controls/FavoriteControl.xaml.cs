using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace OneBusAway.Controls
{
    public sealed partial class FavoriteControl : UserControl
    {
        public FavoriteControl()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Fires the AddToFavorites command and passes it our data context.
        /// </summary>
        private void OnPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            NavigationController.Instance.AddToFavoritesCommand.Execute(this.DataContext);
        }
    }
}
