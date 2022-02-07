using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace MouseMover
{
    public static class Helpers
    {
        public static IEnumerable<T> Descendants<T>(this DependencyObject dependencyItem) where T : DependencyObject
        {
            if (dependencyItem != null)
            {
                for (var index = 0; index < VisualTreeHelper.GetChildrenCount(dependencyItem); index++)
                {
                    var child = VisualTreeHelper.GetChild(dependencyItem, index);
                    if (child is T dependencyObject)
                    {
                        yield return dependencyObject;
                    }

                    foreach (var childOfChild in Descendants<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }
    }
}
