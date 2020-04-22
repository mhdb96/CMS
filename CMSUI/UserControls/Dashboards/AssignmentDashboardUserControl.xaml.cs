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
    /// Interaction logic for AssignmentDashboardUserControl.xaml
    /// </summary>
    public partial class AssignmentDashboardUserControl : UserControl, IAssignmentRequester
    {
        List<AssignmentModel> Assignments;
        List<DepartmentModel> Departments;
        List<ActiveTermModel> ActiveTerms;
        List<AssignmentModel> FilteredAssignments;
        public AssignmentDashboardUserControl()
        {
            InitializeComponent();
            LoadAssignments();
        }

        private void LoadAssignments()
        {
            Assignments = GlobalConfig.Connection.GetAssignment_All();
            assignmentsGrid.ItemsSource = Assignments;
            Departments = GlobalConfig.Connection.GetDepartment_All();
            departmentsCombobox.ItemsSource = Departments;
            ActiveTerms = GlobalConfig.Connection.GetActiveTerm_All();
            activeTermsCombobox.ItemsSource = ActiveTerms;
        }

        private void WireUpLists(List<AssignmentModel> model)
        {
            assignmentsGrid.ItemsSource = null;
            assignmentsGrid.Items.Clear();
            assignmentsGrid.ItemsSource = model;
        }

        private void UpdateAssignmentBtn_Click(object sender, RoutedEventArgs e)
        {
            // TODO - Update the selected Assignment
            AssignmentModel model = (AssignmentModel)assignmentsGrid.SelectedItem;

            CreateAssignmentWindow win = new CreateAssignmentWindow(this, model);
            win.ShowDialog();

            WireUpLists(Assignments);
        }

        private void AddAssignmentBtn_Click(object sender, RoutedEventArgs e)
        {
            CreateAssignmentWindow win = new CreateAssignmentWindow(this);
            win.ShowDialog();
        }

        private async void DeleteAssignmentBtn_Click(object sender, RoutedEventArgs e)
        {
            IParentWindow parent = ParentFinder.FindParent<AdminPanelWindow>(this);
            MessageDialogResult r = await parent.ShowMessage("Warning",
                    "Are you sure you want to delete this assginment",
                    MessageDialogStyle.AffirmativeAndNegative);
            if (r == MessageDialogResult.Negative)
            {
                return;
            }

            //    // TODO - Delete the selected Assignment
            AssignmentModel model = (AssignmentModel)assignmentsGrid.SelectedItem;

            if (GlobalConfig.Connection.DeleteAssignment_ById(model.Id))
            {
                Assignments.Remove(model);
                WireUpLists(Assignments);
                // TODO - Delete the selected term
            }
            else
            {
                await parent.ShowMessage("Deletion Error",
                    "The selected assignment can't be deleted beacause it has an exam",
                    MessageDialogStyle.Affirmative);
                // TODO - ADD a MessageBox
            }

        }

        private void ActiveTermsCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterAssignments();
        }

        private void DepartmentsCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterAssignments();
        }

        private void FilterAssignments()
        {
            if (departmentsCombobox.SelectedItem == null && activeTermsCombobox.SelectedItem == null)
            {
                WireUpLists(Assignments);
                return;
            }
            if (departmentsCombobox.SelectedItem == null)
            {
                //filter by activeTerm
                FilteredAssignments = new List<AssignmentModel>();
                foreach (AssignmentModel model in Assignments)
                {
                    ActiveTermModel a = (ActiveTermModel)activeTermsCombobox.SelectedItem;
                    if (model.ActiveTerm.Id == a.Id)
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
                foreach (AssignmentModel model in Assignments)
                {
                    DepartmentModel d = (DepartmentModel)departmentsCombobox.SelectedItem;
                    if (model.Department.Id == d.Id)
                    {
                        FilteredAssignments.Add(model);
                    }
                }
                WireUpLists(FilteredAssignments);
                return;
            }
            FilteredAssignments = new List<AssignmentModel>();
            foreach (AssignmentModel model in Assignments)
            {
                DepartmentModel d = (DepartmentModel)departmentsCombobox.SelectedItem;
                ActiveTermModel a = (ActiveTermModel)activeTermsCombobox.SelectedItem;
                if (model.Department.Id == d.Id && model.ActiveTerm.Id == a.Id)
                {
                    FilteredAssignments.Add(model);
                }
            }
            WireUpLists(FilteredAssignments);
            return;
        }

        public void AssignmentComplete(AssignmentModel model)
        {
            Assignments.Remove(model);
            Assignments.Add(model);
            WireUpLists(Assignments);
            assignmentsGrid.SelectedIndex = assignmentsGrid.Items.Count - 1;
        }

        private void UpdateDataSourceBtn_Click(object sender, RoutedEventArgs e)
        {
            LoadAssignments();
            WireUpLists(Assignments);
        }
    }
}
