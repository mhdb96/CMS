using CMSLibrary.Enums;
using CMSLibrary.Models;
using CMSUI.CreateForms;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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
            TagData td = (TagData)this.Tag;
            if (td.IsDeletable == true)
            {
                var sp = ParentFinder.FindParent<StackPanel>(this);
                if (!td.IsNew)
                {
                    if (td.Type == OutcomeType.CourseOutcome)
                    {
                        var win = ParentFinder.FindParent<CreateCourseWindow>(this);
                        win.outcomesToDelete.Add(td.Id);
                    }
                    else if (td.Type == OutcomeType.DepartmentOutcome)
                    {
                        var win = ParentFinder.FindParent<CreateDepartmentWindow>(this);
                        win.outcomesToDelete.Add(td.Id);
                    }
                }
                if (sp != null)
                    sp.Children.Remove(this);
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

        private void NameText_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (nameText.Text == "")
            {
                nameText.BorderBrush = Brushes.Red;
            }
            else
            {
                nameText.BorderBrush = Brushes.LightGray;
            }
        }

        private void DescriptionText_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (descriptionText.Text == "")
            {
                descriptionText.BorderBrush = Brushes.Red;
            }
            else
            {
                descriptionText.BorderBrush = Brushes.LightGray;
            }
        }
    }
}
