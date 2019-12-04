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
using System.Windows.Shapes;
using CMSUI.UserControls.HelpMenus;
using CMSUI.UserControls.HelpMenus.Exam;

namespace CMSUI
{
    /// <summary>
    /// Interaction logic for Help.xaml
    /// </summary>
    public partial class Help
    {
        public Help()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            flyout.IsOpen = true;
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeViewItem treeItem = (TreeViewItem)tree.SelectedItem;
            UserControl userControl = new UserControl();
            if(!treeItem.HasItems)
            {
                switch(treeItem.Header.ToString())
                {
                    case "Departments Dashboard":
                        userControl = new DepartmentHelpUserControl();
                        break;
                    case "Teachers Dashboard":
                        userControl = new TeacherHelpUserControl();
                        break;
                    case "Terms Dashboard":
                        userControl = new TermHelpUserControl();
                        break;
                    case "Courses Dashboard":
                        userControl = new CourseHelpUserControl();
                        break;
                    case "Assignments Dashboard":
                        userControl = new AssignmentHelpUserControl();
                        break;
                    case "Exams Dashboard":
                        userControl = new ExamDashboardHelpUserControl();
                        break;
                    case "My Exams Dashboard":
                        userControl = new MyExamDashboardHelpUserControl();
                        break;
                    case "My Profile":
                        userControl = new MyProfileHelpUserControl();
                        break;
                    case "Insert Students":
                        userControl = new InsertStudentsHelpUserControl();
                        break;
                    case "Create Exam":
                        userControl = new CreateExamHelpUserControl();
                        break;
                    default: break;
                }
                if(userControl != null)
                {
                    helpSP.Children.Clear();
                    helpSP.Children.Add(userControl);
                }
                
            }
        }
    }
}