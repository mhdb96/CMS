using CMSLibrary;
using CMSLibrary.Evaluation;
using CMSLibrary.Models;
using CMSUI.EvaluationWindows;
using CMSUI.Panels;
using CMSUI.Requesters;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace CMSUI.UserControls
{
    /// <summary>
    /// Interaction logic for MyCoursesDashboardUserControl.xaml
    /// </summary>
    public partial class MyCoursesDashboardUserControl : UserControl, IExamRequester
    {
        List<AssignmentModel> MyAssignments;
        List<ExamModel> MyExams;
        List<AssignmentModel> FilteredAssignments;
        AssignmentModel SelectedAssignment;
        List<ActiveTermModel> MyTerms = new List<ActiveTermModel>();
        List<DepartmentModel> MyDepartments = new List<DepartmentModel>();
        //Visibility isAdmin = Visibility.Collapsed;
        public static readonly DependencyProperty MyTeacherProperty =
        DependencyProperty.Register("MyTeacher", typeof(TeacherModel), typeof(MyCoursesDashboardUserControl), new FrameworkPropertyMetadata(null));

        public TeacherModel MyTeacher
        {
            get { return (TeacherModel)GetValue(MyTeacherProperty); }
            set { SetValue(MyTeacherProperty, value); }
        }

        public static readonly DependencyProperty MyAdminProperty =
        DependencyProperty.Register("MyAdmin", typeof(AdminModel), typeof(MyCoursesDashboardUserControl), new FrameworkPropertyMetadata(null));

        public AdminModel MyAdmin
        {
            get { return (AdminModel)GetValue(MyAdminProperty); }
            set { SetValue(MyAdminProperty, value); }
        }

        public MyCoursesDashboardUserControl()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadMyAssignments();
            insertStudentBtn.Visibility = Visibility.Visible;
        }

        private void LoadMyAssignments()
        {
            if (MyTeacher == null)
            {
                MyAssignments = GlobalConfig.Connection.GetAssignment_All();
            }
            else
            {
                MyAssignments = GlobalConfig.Connection.GetAssignment_ByTeacherId(MyTeacher.Id);
            }

            if (MyTerms.Count != 0)
            {
                MyTerms.Clear();
                MyDepartments.Clear();
                departmentsCombobox.ItemsSource = null;
                activeTermsCombobox.ItemsSource = null;
            }
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
        private void WireUpLists(List<ExamModel> model)
        {
            examsGrid.ItemsSource = null;
            examsGrid.Items.Clear();
            examsGrid.ItemsSource = model;
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
            if (myCoursesList.ItemsSource != null && myCoursesList.SelectedItem != null)
            {
                AssignmentModel model = (AssignmentModel)myCoursesList.SelectedItem;
                MyExams = GlobalConfig.Connection.GetExam_ByAssignmentId(model.Id);
                foreach (ExamModel exam in MyExams)
                {
                    exam.Assignment = model;
                }
                examsGrid.ItemsSource = MyExams;
            }
            else
            {
                examsGrid.ItemsSource = null;
            }
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
            MyExams.Add(model);
            WireUpLists(MyExams);
        }

        public UserModel GetUserInfo()
        {
            if (MyTeacher == null)
            {
                return MyAdmin.User;
            }
            else
            {
                return MyTeacher.User;
            }
        }

        public AssignmentModel GetAssignment()
        {
            return SelectedAssignment;
        }

        private void UpdateExamBtn_Click(object sender, RoutedEventArgs e)
        {
            // TODO - Update the selected Teacher
            ExamModel model = (ExamModel)examsGrid.SelectedItem;
        }

        private async void DeleteExamBtn_Click(object sender, RoutedEventArgs e)
        {
            IParentWindow parent;

            if (MyAdmin != null)
            {
                parent = ParentFinder.FindParent<AdminPanelWindow>(this);
            }
            else
            {
                parent = ParentFinder.FindParent<TeacherPanelWindow>(this);
            }

            MessageDialogResult r = await parent.ShowMessage("Warning",
                    "Are you sure you want to delete this exam",
                    MessageDialogStyle.AffirmativeAndNegative);
            if (r == MessageDialogResult.Negative)
            {
                return;
            }
            ExamModel model = (ExamModel)examsGrid.SelectedItem;
            GlobalConfig.Connection.DeleteExam_ById(model.Id);
            MyExams.Remove(model);
            WireUpLists(MyExams);
        }

        private void CreateExcelFileBtn_Click(object sender, RoutedEventArgs e)
        {

            CreateExcelFile();
        }

        private void CreateExcelFile()
        {
            ExamModel model = (ExamModel)examsGrid.SelectedItem;
            WriteToExcel w = new WriteToExcel(model);
            GlobalConfig.Connection.UpdateExam_ById(model);
            int t = myCoursesList.SelectedIndex;
            myCoursesList.SelectedItem = null;
            myCoursesList.SelectedIndex = t;
        }

        private async void FileLink_Click(object sender, RoutedEventArgs e)
        {
            ExamModel model = (ExamModel)examsGrid.SelectedItem;
            try
            {
                System.Diagnostics.Process.Start(model.FilePath);
            }
            catch (Exception)
            {
                IParentWindow parent;
                if (MyAdmin != null)
                {
                    parent = ParentFinder.FindParent<AdminPanelWindow>(this);

                }
                else
                {
                    parent = ParentFinder.FindParent<TeacherPanelWindow>(this);
                }
                MessageDialogResult r = await parent.ShowMessage("File Not Found",
                    "The requested excel file wasn't found. Do you want to create a new file?",
                    MessageDialogStyle.AffirmativeAndNegative);
                if (r == MessageDialogResult.Affirmative)
                {
                    CreateExcelFile();
                }

            }

        }
        private void UpdateDataSourceBtn_Click(object sender, RoutedEventArgs e)
        {
            LoadMyAssignments();
            WireUpLists(MyAssignments);
        }

        private void InsertStudentBtn_Click(object sender, RoutedEventArgs e)
        {
            InsertStudentsFromTxtFiles win = new InsertStudentsFromTxtFiles();
            win.ShowDialog();
        }
    }
}
