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
using System.Windows.Shapes;

namespace CMSUI
{
    /// <summary>
    /// Interaction logic for CreateActiveTermWindow.xaml
    /// </summary>
    public partial class CreateActiveTermWindow
    {
        IActiveTermRequester CallingWindow;
        List<YearModel> Years;
        List<TermModel> Terms;
        public CreateActiveTermWindow(IActiveTermRequester caller)
        {
            InitializeComponent();
            CallingWindow = caller;
            LoadListsData();
        }
        private void LoadListsData()
        {
            Years = GlobalConfig.Connection.GetYear_ALL();
            yearsCombobox.ItemsSource = Years;
            Terms = GlobalConfig.Connection.GetTerm_ALL();
            termsCombobox.ItemsSource = Terms;
        }

        private void CreateActiveTermBtn_Click(object sender, RoutedEventArgs e)
        {
            if(ValidForm())
            {
                ActiveTermModel model = new ActiveTermModel();
                model.Year = (YearModel)yearsCombobox.SelectedItem;
                model.Term = (TermModel)termsCombobox.SelectedItem;
                GlobalConfig.Connection.CreateActiveTerm(model);
                CallingWindow.ActiveTermComplete(model);
                this.Close();
            }
            
        }

        private bool ValidForm()
        {
            //TODO - Validate this form
            bool valid = true;
            return valid;
        }

        private void CancelActiveTermBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
