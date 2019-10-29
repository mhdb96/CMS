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
using CMSUI.Requesters;
using CMSUI.UserControls;

namespace CMSUI
{
    /// <summary>
    /// Interaction logic for CreateDepartmentWindow.xaml
    /// </summary>
    public partial class CreateDepartmentWindow
    {
        IDepartmentRequester callingWindow;
        public CreateDepartmentWindow(IDepartmentRequester caller)
        {
            InitializeComponent();
            callingWindow = caller;
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
                callingWindow.DepartmentComplete(model);
                this.Close();
            }
            

        }

        //private async void ShowMessageDialog()
        //{
        //    var mySettings = new MetroDialogSettings()
        //    {
        //        AffirmativeButtonText = "OK",
        //        ColorScheme = MetroDialogOptions.ColorScheme, DialogMessageFontSize = 10

        //    };

        //    MessageDialogResult result = await this.ShowMessageAsync("Success!", "Department has been successfuly added to the database",
        //        MessageDialogStyle.Affirmative, mySettings);

        //    if (result == MessageDialogResult.Affirmative)
        //    {
                
        //    }
        //}


        // TODO - empliment validation
        private bool ValidForm()
        {
            
            if (nameText.Text != "")
            {
                foreach (OutcomeUserControl outcome in outcomesList.Children)
                {
                    if(outcome.nameText.Text == "" || outcome.descriptionText.Text =="")
                    {
                        return false;

                    }
                }
                return true;
            }
            else
            {
                return false;
            }
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
