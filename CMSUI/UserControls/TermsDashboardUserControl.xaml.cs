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

        private void DeleteTermBtn_Click(object sender, RoutedEventArgs e)
        {
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
    }
}