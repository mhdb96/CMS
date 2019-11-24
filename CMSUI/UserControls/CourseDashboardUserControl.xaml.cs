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
using System.Windows.Navigation;
using System.Windows.Shapes;

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
            CourseModel model = (CourseModel)coursesList.SelectedItem;
            FindCourseOutcomes(model);
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

        private void DeleteCourseBtn_Click(object sender, RoutedEventArgs e)
        {
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
                // TODO - ADD a MessageBox
            }

            

        }
    }
}
