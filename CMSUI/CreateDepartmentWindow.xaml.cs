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
using CMSLibrary;
using CMSLibrary.Models;
using CMSUI.UserControls;

namespace CMSUI
{
    /// <summary>
    /// Interaction logic for CreateDepartmentWindow.xaml
    /// </summary>
    public partial class CreateDepartmentWindow
    {
        
        public CreateDepartmentWindow()
        {
            InitializeComponent();            
        }

        private void CreateDepartmentBtn_Click(object sender, RoutedEventArgs e)
        {
            if(ValidForm())
            {
                DepartmentModel model = new DepartmentModel();
                model.Name = nameText.Text;
                foreach (OutcomeUserControl outcome in outcomesList.Children)
                {
                    DepartmentOutcomeModel dO = new DepartmentOutcomeModel();
                    dO.Name = outcome.nameText.Text;
                    dO.Description = outcome.descriptionText.Text;
                    model.Outcomes.Add(dO);
                }
                GlobalConfig.Connection.CreateDepartment(model);
            }
            
        }
        // TODO - empliment validation
        private bool ValidForm()
        {
            bool valid = true;
            return valid;
        }

        private void CancelDepartmentBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AddOutcome_Click(object sender, RoutedEventArgs e)
        {
            // TODO - fix the placement system for the letters
            OutcomeUserControl outcome = new OutcomeUserControl();
            outcome.nameText.Text = Convert.ToChar(outcomesList.Children.Count + 65).ToString();
            outcomesList.Children.Add(outcome);
        }
    }
}
