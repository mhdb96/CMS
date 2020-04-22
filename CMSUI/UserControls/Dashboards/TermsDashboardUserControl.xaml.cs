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
    /// Interaction logic for TermsDashboardUserControl.xaml
    /// </summary>
    public partial class TermsDashboardUserControl : UserControl, IActiveTermRequester
    {
        List<ActiveTermModel> Terms;
        public TermsDashboardUserControl()
        {
            InitializeComponent();
            LoadTerms();
        }

        private void LoadTerms()
        {
            // TODO - Think of a way to sort the terms
            Terms = GlobalConfig.Connection.GetActiveTerm_All();
            termsList.ItemsSource = Terms;
        }

        private void WireUpLists()
        {
            termsList.ItemsSource = null;
            termsList.Items.Clear();
            termsList.ItemsSource = Terms;
        }

        private void AddTermBtn_Click(object sender, RoutedEventArgs e)
        {
            CreateActiveTermWindow win = new CreateActiveTermWindow(this);
            win.ShowDialog();
        }

        private void UpdateTermBtn_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            ActiveTermModel model = new ActiveTermModel();
            model = (ActiveTermModel)btn.Tag;
            // TODO - Update the selected term

            CreateActiveTermWindow win = new CreateActiveTermWindow(this, model);
            win.ShowDialog();

            WireUpLists();

        }

        private async void DeleteTermBtn_Click(object sender, RoutedEventArgs e)
        {
            IParentWindow parent = ParentFinder.FindParent<AdminPanelWindow>(this);
            MessageDialogResult r = await parent.ShowMessage("Warning",
                    "Are you sure you want to delete this term",
                    MessageDialogStyle.AffirmativeAndNegative);
            if (r == MessageDialogResult.Negative)
            {
                return;
            }
            Button btn = (Button)sender;
            ActiveTermModel model = new ActiveTermModel();
            model = (ActiveTermModel)btn.Tag;


            if (GlobalConfig.Connection.DeleteActiveTerm_ById(model.Id))
            {
                Terms.Remove(model);
                WireUpLists();
                // TODO - Delete the selected term
            }
            else
            {
                await parent.ShowMessage("Deletion Error",
                    "The selected term can't be deleted beacause it has an exam",
                    MessageDialogStyle.Affirmative);
                // TODO - ADD a MessageBox
            }
        }

        public void ActiveTermComplete(ActiveTermModel model)
        {
            Terms.Remove(model);
            Terms.Add(model);
            WireUpLists();
            termsList.SelectedIndex = termsList.Items.Count - 1;
        }

        private void UpdateDataSourceBtn_Click(object sender, RoutedEventArgs e)
        {
            LoadTerms();
            WireUpLists();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string searchValue = searchText.Text;
            Terms = GlobalConfig.Connection.GetActiveTerm_BySearchValue(searchValue);
            termsList.ItemsSource = Terms;
            WireUpLists();
        }
    }
}