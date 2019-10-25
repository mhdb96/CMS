using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CMSUI.UserControls
{
    /// <summary>
    /// Interaction logic for Outcomes.xaml
    /// </summary>
    public partial class OutcomeUserControl : UserControl
    {
        public OutcomeUserControl()
        {
            InitializeComponent();
        }     

        private void DeleteOutcome_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            var conditionUserControl = FindParent<OutcomeUserControl>(btn);
            if (conditionUserControl != null)
            {
                var sp = FindParent<StackPanel>(conditionUserControl);
                if (sp != null)
                    sp.Children.Remove(conditionUserControl);
            }
        }
        // TODO - Try to understand this Func.
        private static T FindParent<T>(DependencyObject dependencyObject) where T : DependencyObject
        {
            var parent = VisualTreeHelper.GetParent(dependencyObject);

            if (parent == null) return null;

            var parentT = parent as T;
            return parentT ?? FindParent<T>(parent);
        }
    }
}
