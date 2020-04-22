using CMSLibrary;
using CMSLibrary.Models;
using CMSUI.Requesters;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace CMSUI.CreateForms
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

        bool update;
        AssignmentModel assignmentModel = new AssignmentModel();

        public CreateAssignmentWindow(IAssignmentRequester caller)
        {
            InitializeComponent();
            CallingWindow = caller;
            LoadListsData();

            update = false;
            createAssignmentBtn.Content = "Create";//gerek var mı?
        }
        public CreateAssignmentWindow(IAssignmentRequester caller, AssignmentModel model)
        {
            InitializeComponent();
            CallingWindow = caller;
            LoadListsData();

            update = true;
            createAssignmentBtn.Content = "Update";
            titleText.Text = "Update The Assignment";
            title.Title = "UPDATE ASSIGNMENT";
            assignmentModel = model;


            // TODO Foreachsız nasıl olurdu
            foreach (var department in Departments)
            {
                if (assignmentModel.Department.Id == department.Id)
                {
                    departmentsCombobox.SelectedItem = department;
                }
            }
            foreach (var activeTerm in ActiveTerms)
            {
                if (assignmentModel.ActiveTerm.Id == activeTerm.Id)
                {
                    activeTermsCombobox.SelectedItem = activeTerm;
                }
            }
            foreach (var course in Courses)
            {
                if (assignmentModel.Course.Id == course.Id)
                {
                    coursesCombobox.SelectedItem = course;
                }
            }
            foreach (var teacher in Teachers)
            {
                if (assignmentModel.Teacher.Id == teacher.Id)
                {
                    teachersCombobox.SelectedItem = teacher;
                }
            }
            //

        }

        private void LoadListsData()
        {
            Departments = GlobalConfig.Connection.GetDepartment_All();
            departmentsCombobox.ItemsSource = Departments;
            ActiveTerms = GlobalConfig.Connection.GetActiveTerm_All();
            activeTermsCombobox.ItemsSource = ActiveTerms;
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
                assignmentModel.Department = (DepartmentModel)departmentsCombobox.SelectedItem;
                assignmentModel.ActiveTerm = (ActiveTermModel)activeTermsCombobox.SelectedItem;
                assignmentModel.Course = (CourseModel)coursesCombobox.SelectedItem;
                assignmentModel.Teacher = (TeacherModel)teachersCombobox.SelectedItem;

                if (!update)
                {
                    GlobalConfig.Connection.CreateAssignment(assignmentModel);
                }
                else
                {
                    GlobalConfig.Connection.UpdateAssignments(assignmentModel);
                }

                CallingWindow.AssignmentComplete(assignmentModel);
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

        void GetCourse()
        {
            if (departmentsCombobox.SelectedItem == null || activeTermsCombobox.SelectedItem == null)
            {
                coursesCombobox.IsHitTestVisible = false;
            }
            else
            {
                DepartmentModel departmentModel = (DepartmentModel)departmentsCombobox.SelectedItem;
                ActiveTermModel activeTermModel = (ActiveTermModel)activeTermsCombobox.SelectedItem;

                Courses = GlobalConfig.Connection.GetCourse_ValidByDepartmentIdAndActiveTermId(departmentModel.Id, activeTermModel.Id);

                if (update)
                {
                    if (departmentModel.Id == assignmentModel.Department.Id && activeTermModel.Id == assignmentModel.ActiveTerm.Id)
                    {
                        Courses.Add(assignmentModel.Course);
                    }
                }


                coursesCombobox.ItemsSource = Courses;

                if (!Courses.Any())
                {
                    coursesCombobox.IsHitTestVisible = false;
                }
                else
                {
                    coursesCombobox.IsHitTestVisible = true;
                }
            }
        }


        private void DepartmentsCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GetCourse();

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
            GetCourse();

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



//private void LoadListsData()
//{
//    Years = GlobalConfig.Connection.GetYear_ALL();
//    yearsCombobox.ItemsSource = Years;

//    //Terms = GlobalConfig.Connection.GetTerm_ALL();
//    //termsCombobox.ItemsSource = Terms;
//}

//private void CreateActiveTermBtn_Click(object sender, RoutedEventArgs e)
//{

//    if (ValidForm())
//    {
//        ActiveTermModel model = new ActiveTermModel();
//        model.Year = (YearModel)yearsCombobox.SelectedItem;
//        model.Term = (TermModel)termsCombobox.SelectedItem;
//        GlobalConfig.Connection.CreateActiveTerm(model);
//        CallingWindow.ActiveTermComplete(model);
//        this.Close();
//    }

//}

//private bool ValidForm()
//{

//    if (yearsCombobox.SelectedItem == null || termsCombobox.SelectedItem == null)
//    {
//        if (yearsCombobox.SelectedItem == null)
//        {
//            errorYear.Visibility = Visibility.Visible;
//        }
//        if (termsCombobox.SelectedItem == null)
//        {
//            errorTerm.Visibility = Visibility.Visible;
//        }
//        return false;
//    }
//    else
//    {
//        return true;
//    }
//}

//private bool ValidForm2()
//{
//    int i = 0;
//    List<int> rows = new List<int>();
//    List<String> text = new List<String>();

//    StackPanel sp;

//    TextBlock tb;
//    PackIconMaterial icon;
//    foreach (UIElement Child in myGrid.Children)
//    {
//        if (Child is ComboBox)
//        {
//            if (((ComboBox)Child).SelectedItem == null)
//            {
//                rows.Add(Grid.GetRow(Child));
//                text.Add(((ComboBox)Child).Name);
//                i++;

//            }
//        }
//    }
//    foreach (var item in spList)
//    {
//        item.Children.Clear();
//    }

//    if (i > 0)
//    {
//        i = 0;
//        foreach (var row in rows)
//        {
//            sp = new StackPanel();
//            sp.Name = "errorSp";
//            sp.Orientation = Orientation.Horizontal;

//            spList.Add(sp);

//            tb = new TextBlock();
//            tb.Text = "You need to choose a " + text[i];
//            tb.FontSize = 20;
//            tb.VerticalAlignment = VerticalAlignment.Center;

//            icon = new PackIconMaterial();
//            icon.Kind = PackIconMaterialKind.AlertCircle;
//            icon.VerticalAlignment = VerticalAlignment.Center;
//            icon.Width = 20;
//            icon.Height = 20;
//            icon.Margin = new Thickness(5);
//            icon.Foreground = new SolidColorBrush(Colors.Red);

//            sp.Children.Add(icon);
//            sp.Children.Add(tb);
//            Grid.SetRow(sp, row);
//            Grid.SetColumn(sp, 3);
//            Grid.SetColumnSpan(sp, 3);
//            myGrid.Children.Add(sp);
//            i++;
//        }
//        return false;
//    }
//    else
//    {
//        return true;
//    }

//}

//private void CancelActiveTermBtn_Click(object sender, RoutedEventArgs e)
//{
//    this.Close();
//}

//private void YearsCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
//{


//    if (yearsCombobox.SelectedItem == null)
//    {
//        termsCombobox.IsHitTestVisible = false;
//        errorYear.Visibility = Visibility.Visible;
//    }
//    else
//    {
//        model = (YearModel)yearsCombobox.SelectedItem;
//        myTerms = GlobalConfig.Connection.GetTerm_ValidByYearId(model.Id);
//        termsCombobox.ItemsSource = myTerms;

//        if (!myTerms.Any())
//        {
//            termsCombobox.IsHitTestVisible = false;
//        }
//        else
//        {
//            termsCombobox.IsHitTestVisible = true;
//        }

//        errorYear.Visibility = Visibility.Hidden;
//    }
//}

//private void TermsCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
//{
//    if (termsCombobox.SelectedItem == null)
//    {
//        errorTerm.Visibility = Visibility.Visible;
//    }
//    else
//    {
//        errorTerm.Visibility = Visibility.Hidden;
//    }
//}
//    }
//}