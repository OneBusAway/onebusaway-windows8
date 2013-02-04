using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace OneBusAway.Controls
{
    public static class ControlUtilities
    {
        /// <summary>
        /// Walks the logical tree until it finds type T.
        /// </summary>
        public static T GetParent<T>(DependencyObject parent)
            where T : DependencyObject
        {
            parent = VisualTreeHelper.GetParent(parent);
            while (parent != null)
            {
                if (parent is T)
                {
                    break;
                }

                parent = VisualTreeHelper.GetParent(parent);
            }

            return parent as T;
        }
    }
}
