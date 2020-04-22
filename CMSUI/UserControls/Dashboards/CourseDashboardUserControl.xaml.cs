using CMSLibrary;
using CMSLibrary.Models;
using CMSUI.CreateForms;
using CMSUI.Panels;
using CMSUI.Requesters;
using MahApps.Metro.Controls.Dialogs;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace CMSUI.UserControls
{
    /// <summary>
    /// Interaction logic for CourseDashboardControl.xaml
    /// </summary>
    public partial class CourseDashboardUserControl : UserControl, ICouresRequester
    {
        List<CourseModel> Courses;
        public CourseDashboardUserControl()
        {
            InitializeComponent();
            LoadCourses();
        }

        private void LoadCourses()
        {
            Courses = GlobalConfig.Connection.GetCourse_All();
            coursesList.ItemsSource = Courses;
            // TODO - look at the design of the course list
        }

        private void WireUpLists()
        {
            coursesList.ItemsSource = null;
            coursesList.Items.Clear();
            coursesList.ItemsSource = Courses;
        }

        private void AddCourseBtn_Click(object sender, RoutedEventArgs e)
        {
            CreateCourseWindow win = new CreateCourseWindow(this);
            win.ShowDialog();
        }

        public void CourseComplete(CourseModel model)
        {
            Courses.Add(model);
            WireUpLists();
            coursesList.SelectedIndex = coursesList.Items.Count - 1;
        }

        public void CourseUpdateComplete(CourseModel model)
        {
            Courses.Remove(model);
            Courses.Add(model);
            WireUpLists();
            coursesList.SelectedIndex = coursesList.Items.Count - 1;
        }

        private void CoursesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (coursesList.SelectedItem != null)
            {
                CourseModel model = (CourseModel)coursesList.SelectedItem;
                FindCourseOutcomes(model);
            }
            else
            {
                courseOutcomesList.ItemsSource = null;
            }


        }
        public void FindCourseOutcomes(CourseModel model)
        {
            if (coursesList.ItemsSource != null)
            {
                GlobalConfig.Connection.GetCourseOutcomes_ById(model);
                coursesList.SelectedItem = model;
                courseOutcomesList.ItemsSource = model.CourseOutcomes;
            }
        }
        private void UpdateCourseBtn_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            CourseModel model = new CourseModel();
            model = (CourseModel)btn.Tag;
            // TODO - Update the selected department

            if (model.CourseOutcomes.Count == 0)
            {
                FindCourseOutcomes(model);
            }

            CreateCourseWindow win = new CreateCourseWindow(this, model);

            win.ShowDialog();

            WireUpLists();

        }

        private async void DeleteCourseBtn_Click(object sender, RoutedEventArgs e)
        {
            IParentWindow parent = ParentFinder.FindParent<AdminPanelWindow>(this);
            MessageDialogResult r = await parent.ShowMessage("Warning",
                    "Are you sure you want to delete this course",
                    MessageDialogStyle.AffirmativeAndNegative);
            if (r == MessageDialogResult.Negative)
            {
                return;
            }
            Button btn = (Button)sender;
            CourseModel model = new CourseModel();
            model = (CourseModel)btn.Tag;
            // TODO - Delete the selected department

            if (GlobalConfig.Connection.DeleteCourse_ById(model.Id))
            {
                Courses.Remove(model);
                WireUpLists();
                // TODO - Delete the selected term
            }
            else
            {
                await parent.ShowMessage("Deletion Error",
                    "The selected course can't be deleted beacause it has an exam",
                    MessageDialogStyle.Affirmative);
                // TODO - ADD a MessageBox
            }
        }

        private void UpdateDataSourceBtn_Click(object sender, RoutedEventArgs e)
        {
            LoadCourses();
            WireUpLists();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string searchValue = searchText.Text;
            Courses = GlobalConfig.Connection.GetCourse_BySearchValue(searchValue);
            coursesList.ItemsSource = Courses;
            WireUpLists();
        }
    }
}
