using System.Windows;
using System.Windows.Controls;

namespace CMSUI.UserControls
{
    /// <summary>
    /// Interaction logic for StudentDataUserControl.xaml
    /// </summary>
    public partial class StudentDataUserControl : UserControl
    {
        public StudentDataUserControl()
        {
            InitializeComponent();
        }

        private void DeleteStudentData_Click(object sender, RoutedEventArgs e)
        {
            var sp = ParentFinder.FindParent<StackPanel>(this);
            if (sp != null)
                sp.Children.Remove(this);
        }
    }
}
