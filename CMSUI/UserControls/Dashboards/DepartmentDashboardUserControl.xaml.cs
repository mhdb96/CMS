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
    /// Interaction logic for DepartmentDashboardControl.xaml
    /// </summary>
    public partial class DepartmentDashboardUserControl : UserControl, IDepartmentRequester
    {
        List<DepartmentModel> Departments;
        //List<DepartmentOutcomeModel> DepartmentOutcomes;
        public DepartmentDashboardUserControl()
        {
            InitializeComponent();
            LoadDepartments();
        }

        private void LoadDepartments()
        {
            Departments = GlobalConfig.Connection.GetDepartment_All();
            depatmentsList.ItemsSource = Departments;
        }

        private void WireUpLists()
        {
            depatmentsList.ItemsSource = null;
            depatmentsList.Items.Clear();
            depatmentsList.ItemsSource = Departments;
        }

        private async void DeleteDepartmentBtn_Click(object sender, RoutedEventArgs e)
        {
            IParentWindow parent = ParentFinder.FindParent<AdminPanelWindow>(this);
            MessageDialogResult r = await parent.ShowMessage("Warning",
                    "Are you sure you want to delete this department",
                    MessageDialogStyle.AffirmativeAndNegative);
            if (r == MessageDialogResult.Negative)
            {
                return;
            }
            Button btn = (Button)sender;
            DepartmentModel model = new DepartmentModel();
            model = (DepartmentModel)btn.Tag;

            if (GlobalConfig.Connection.DeleteDepartment_ById(model.Id))
            {
                Departments.Remove(model);
                WireUpLists();
                // TODO - Delete the selected term
            }
            else
            {
                await parent.ShowMessage("Deletion Error",
                    "The selected department can't be deleted beacause it has an exam",
                    MessageDialogStyle.Affirmative);
                // TODO - ADD a MessageBox
            }

        }

        private void UpdateDepartmentBtn_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            DepartmentModel model = new DepartmentModel();
            model = (DepartmentModel)btn.Tag;
            if (model.Outcomes.Count == 0)
            {
                FindCourseOutcomes(model);
            }
            // TODO - Update the selected department

            CreateDepartmentWindow win = new CreateDepartmentWindow(this, model);
            win.ShowDialog();
            WireUpLists();
        }

        private void DepatmentsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (depatmentsList.SelectedItem != null)
            {
                DepartmentModel model = (DepartmentModel)depatmentsList.SelectedItem;
                FindCourseOutcomes(model);
            }
            else
            {
                depatmentOutcomesList.ItemsSource = null;
            }

        }

        private void FindCourseOutcomes(DepartmentModel model)
        {
            if (depatmentsList.ItemsSource != null)
            {
                GlobalConfig.Connection.GetDepartmentOutcomes_ById(model);
                depatmentsList.SelectedItem = model;
                depatmentOutcomesList.ItemsSource = model.Outcomes;
            }
        }

        private void AddDepartmentBtn_Click(object sender, RoutedEventArgs e)
        {
            CreateDepartmentWindow win = new CreateDepartmentWindow(this);
            win.ShowDialog();
        }

        public void DepartmentComplete(DepartmentModel model)
        {
            Departments.Add(model);
            WireUpLists();
            depatmentsList.SelectedIndex = depatmentsList.Items.Count - 1;
        }
        public void DepartmentUpdateComplete(DepartmentModel model)
        {
            Departments.Remove(model);
            Departments.Add(model);
            WireUpLists();
            depatmentsList.SelectedIndex = depatmentsList.Items.Count - 1;
        }

        private void UpdateDataSourceBtn_Click(object sender, RoutedEventArgs e)
        {
            LoadDepartments();
            WireUpLists();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string searchValue = searchText.Text;
            Departments = GlobalConfig.Connection.GetDepartment_BySearchValue(searchValue);
            depatmentsList.ItemsSource = Departments;
            WireUpLists();
        }
    }
}
