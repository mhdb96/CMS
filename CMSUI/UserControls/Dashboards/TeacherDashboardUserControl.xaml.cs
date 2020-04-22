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
    /// Interaction logic for TeacherDashboardUserControl.xaml
    /// </summary>
    public partial class TeacherDashboardUserControl : UserControl, ITeacherRequester
    {
        List<TeacherModel> Teachers;
        public TeacherDashboardUserControl()
        {
            InitializeComponent();
            LoadTeachers();
        }

        private void LoadTeachers()
        {
            Teachers = GlobalConfig.Connection.GetFullTeacher_All();
            //TeacherModel t = new TeacherModel();
            //t.FirstName = "1111";
            //t.LastName = "11111";
            //t.User.UserName = "uuuuuu";
            //t.User.Password = "bbbbb";
            //Teachers.Add(t);
            teachersGrid.ItemsSource = Teachers;
        }

        private void WireUpLists()
        {
            teachersGrid.ItemsSource = null;
            teachersGrid.Items.Clear();
            teachersGrid.ItemsSource = Teachers;
        }

        public void TeacherComplete(TeacherModel model)
        {
            Teachers.Remove(model);
            Teachers.Add(model);
            WireUpLists();
            teachersGrid.SelectedIndex = teachersGrid.Items.Count - 1;
        }

        private void AddTeacherBtn_Click(object sender, RoutedEventArgs e)
        {
            CreateTeacherWindow win = new CreateTeacherWindow(this);
            win.ShowDialog();
        }

        private async void DeleteTeacherBtn_Click(object sender, RoutedEventArgs e)
        {
            IParentWindow parent = ParentFinder.FindParent<AdminPanelWindow>(this);
            MessageDialogResult r = await parent.ShowMessage("Warning",
                    "Are you sure you want to delete this teacher",
                    MessageDialogStyle.AffirmativeAndNegative);
            if (r == MessageDialogResult.Negative)
            {
                return;
            }
            // TODO - Delete the selected Teacher
            TeacherModel model = (TeacherModel)teachersGrid.SelectedItem;

            if (GlobalConfig.Connection.DeleteTeacher_ById(model.Id))
            {
                Teachers.Remove(model);
                WireUpLists();
                // TODO - Delete the selected term
            }
            else
            {
                await parent.ShowMessage("Deletion Error",
                    "The selected teacher can't be deleted beacause it has an exam",
                    MessageDialogStyle.Affirmative);
                // TODO - ADD a MessageBox
            }

        }

        private void UpdateTeacherBtn_Click(object sender, RoutedEventArgs e)
        {
            // TODO - Update the selected Teacher
            TeacherModel model = (TeacherModel)teachersGrid.SelectedItem;

            CreateTeacherWindow win = new CreateTeacherWindow(this, model);
            win.ShowDialog();

            WireUpLists();

        }

        private void UpdateDataSourceBtn_Click(object sender, RoutedEventArgs e)
        {
            LoadTeachers();
            WireUpLists();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string searchValue = searchText.Text;
            Teachers = GlobalConfig.Connection.GetTeacher_BySearchValue(searchValue);
            teachersGrid.ItemsSource = Teachers;
            WireUpLists();
        }
    }
}