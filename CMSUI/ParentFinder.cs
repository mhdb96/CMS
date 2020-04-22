using System.Windows;
using System.Windows.Media;

namespace CMSUI
{
    public static class ParentFinder
    {
        public static T FindParent<T>(DependencyObject dependencyObject) where T : DependencyObject
        {
            var parent = VisualTreeHelper.GetParent(dependencyObject);

            if (parent == null) return null;

            var parentT = parent as T;
            return parentT ?? FindParent<T>(parent);
        }
    }
}
