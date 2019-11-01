using CMSLibrary;
using CMSLibrary.Models;
using CMSUI.Requesters;
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

namespace CMSUI
{
    /// <summary>
    /// Interaction logic for CreateAssignmentWindow.xaml
    /// </summary>
    public partial class CreateAssignmentWindow
    {
        IAssignmentRequester CallingWindow;
        List<DepartmentModel> Departments;
        List<ActiveTermModel> ActiveTerms;
        List<CourseModel> Courses;
        List<TeacherModel> Teachers;

        public CreateAssignmentWindow(IAssignmentRequester caller)
        {
            InitializeComponent();
            CallingWindow = caller;
            LoadListsData();
        }

        private void LoadListsData()
        {
            Departments = GlobalConfig.Connection.GetDepartment_All();
            departmentsCombobox.ItemsSource = Departments;
            ActiveTerms = GlobalConfig.Connection.GetActiveTerm_All();
            activeTermsCombobox.ItemsSource = ActiveTerms;
            Courses = GlobalConfig.Connection.GetCourse_All();
            coursesCombobox.ItemsSource = Courses;
            Teachers = GlobalConfig.Connection.GetTeacher_All();
            teachersCombobox.ItemsSource = Teachers;
        }

        private void CancelAssignmentBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void CreateAssignmentBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ValidForm())
            {
                AssignmentModel model = new AssignmentModel();
                model.Department = (DepartmentModel)departmentsCombobox.SelectedItem;
                model.ActiveTerm = (ActiveTermModel)activeTermsCombobox.SelectedItem;
                model.Course = (CourseModel)coursesCombobox.SelectedItem;
                model.Teacher = (TeacherModel)teachersCombobox.SelectedItem;
                GlobalConfig.Connection.CreateAssignment(model);
                CallingWindow.AssignmentComplete(model);
                this.Close();
            }
        }
        private bool ValidForm()
        {
            // TODO - Validate this form
            if (departmentsCombobox.SelectedItem == null || activeTermsCombobox.SelectedItem == null || coursesCombobox.SelectedItem == null || teachersCombobox.SelectedItem == null)
            {
                if (departmentsCombobox.SelectedItem == null)
                {
                    errorDepartment.Visibility = Visibility.Visible;
                }
                if (activeTermsCombobox.SelectedItem == null)
                {
                    errorActiveTerm.Visibility = Visibility.Visible;
                }
                if (coursesCombobox.SelectedItem == null)
                {
                    errorCourse.Visibility = Visibility.Visible;
                }
                if (teachersCombobox.SelectedItem == null)
                {
                    errorTeacher.Visibility = Visibility.Visible;
                }
                return false;
            }
            else
            {
                return true;
            }
        }

        private void DepartmentsCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (departmentsCombobox.SelectedItem == null)
            {
                errorDepartment.Visibility = Visibility.Visible;
            }
            else
            {
                errorDepartment.Visibility = Visibility.Hidden;
            }
        }

        private void ActiveTermsCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (activeTermsCombobox.SelectedItem == null)
            {
                errorActiveTerm.Visibility = Visibility.Visible;
            }
            else
            {
                errorActiveTerm.Visibility = Visibility.Hidden;
            }
        }

        private void CoursesCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (coursesCombobox.SelectedItem == null)
            {
                errorCourse.Visibility = Visibility.Visible;
            }
            else
            {
                errorCourse.Visibility = Visibility.Hidden;
            }
        }

        private void TeachersCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (teachersCombobox.SelectedItem == null)
            {
                errorTeacher.Visibility = Visibility.Visible;
            }
            else
            {
                errorTeacher.Visibility = Visibility.Hidden;
            }
        }
    }
}
