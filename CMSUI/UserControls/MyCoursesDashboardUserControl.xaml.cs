using CMSLibrary;
using CMSLibrary.Models;
using CMSUI.EvaluationWindows;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CMSUI.UserControls
{
    /// <summary>
    /// Interaction logic for MyCoursesDashboardUserControl.xaml
    /// </summary>
    public partial class MyCoursesDashboardUserControl : UserControl, IExamRequester
    {
        List<AssignmentModel> MyAssignments;
        List<AssignmentModel> FilteredAssignments;
        AssignmentModel SelectedAssignment;
        List<ActiveTermModel> MyTerms = new List<ActiveTermModel>();
        List<DepartmentModel> MyDepartments = new List<DepartmentModel>();
        //List<CourseModel> MyCourses;

        public static readonly DependencyProperty MyTeacherProperty =
        DependencyProperty.Register("MyTeacher", typeof(TeacherModel), typeof(MyCoursesDashboardUserControl), new FrameworkPropertyMetadata(null));

        public TeacherModel MyTeacher
        {
            get { return (TeacherModel)GetValue(MyTeacherProperty); }
            set { SetValue(MyTeacherProperty, value); }
        }

        public MyCoursesDashboardUserControl()
        {
            InitializeComponent();
            //Loaded += UserControl_Loaded;            
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadMyAssignments();
        }

        private void LoadMyAssignments()
        {
            MyAssignments = GlobalConfig.Connection.GetAssignment_ByTeacherId(MyTeacher.Id);
            foreach (AssignmentModel model in MyAssignments)
            {
                MyTerms.Add(model.ActiveTerm);
                MyDepartments.Add(model.Department);
            }
            departmentsCombobox.ItemsSource = MyDepartments.Select(x => x.Name).Distinct();
            activeTermsCombobox.ItemsSource = MyTerms.Select(x => x.Name).Distinct();
            MyAssignments = MyAssignments.OrderBy(a => a.Course.Name).ToList();
            myCoursesList.ItemsSource = MyAssignments;
        }
        private void WireUpLists(List<AssignmentModel> model)
        {
            myCoursesList.ItemsSource = null;
            myCoursesList.Items.Clear();
            myCoursesList.ItemsSource = model;
        }

        private void FilterCourses()
        {
            if (departmentsCombobox.SelectedItem == null && activeTermsCombobox.SelectedItem == null)
            {
                WireUpLists(MyAssignments);
                return;
            }
            if (departmentsCombobox.SelectedItem == null)
            {
                //filter by activeTerm
                FilteredAssignments = new List<AssignmentModel>();
                foreach (AssignmentModel model in MyAssignments)
                {                    
                    if (model.ActiveTerm.Name == (string)activeTermsCombobox.SelectedItem)
                    {
                        FilteredAssignments.Add(model);
                    }
                }
                WireUpLists(FilteredAssignments);
                return;
            }
            if (activeTermsCombobox.SelectedItem == null)
            {
                //filter by department
                FilteredAssignments = new List<AssignmentModel>();
                foreach (AssignmentModel model in MyAssignments)
                {                    
                    if (model.Department.Name == (string)departmentsCombobox.SelectedItem)
                    {
                        FilteredAssignments.Add(model);
                    }
                }
                WireUpLists(FilteredAssignments);
                return;
            }
            FilteredAssignments = new List<AssignmentModel>();
            foreach (AssignmentModel model in MyAssignments)
            {
                string d = (string)departmentsCombobox.SelectedItem;
                string a = (string)activeTermsCombobox.SelectedItem;
                if (model.Department.Name == d && model.ActiveTerm.Name == a)
                {
                    FilteredAssignments.Add(model);
                }
            }
            WireUpLists(FilteredAssignments);
            return;
        }

        private void DepartmentsCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterCourses();
        }

        private void ActiveTermsCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterCourses();
        }

        private void MyCoursesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void AddExamBtn_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            SelectedAssignment = (AssignmentModel)btn.Tag;
            CreateExamWindow win = new CreateExamWindow(this);
            win.ShowDialog();
            
        }

        public void ExamComplete(ExamModel model)
        {
            throw new NotImplementedException();
        }

        public UserModel GetUserInfo()
        {
            return MyTeacher.User;
        }

        public AssignmentModel GetAssignment()
        {
            return SelectedAssignment;
        }
    }
}
