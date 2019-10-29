using CMSLibrary;
using CMSLibrary.Models;
using CMSUI.Requesters;
using CMSUI.UserControls;
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
    /// Interaction logic for CreateCourseWindow.xaml
    /// </summary>
    public partial class CreateCourseWindow
    {
        ICouresRequester CallingWindow;
        List<EducationalYearModel> EduYears;
        public CreateCourseWindow(ICouresRequester caller)
        {
            InitializeComponent();
            CallingWindow = caller;
            LoadListsData();
        }

        private void LoadListsData()
        {
            EduYears = GlobalConfig.Connection.GetEducationalYear_ALL();
            eduYearCombobox.ItemsSource = EduYears;            
        }

        private void AddOutcome_Click(object sender, RoutedEventArgs e)
        {
            // TODO - fix the placement system for the letters
            OutcomeUserControl outcome = new OutcomeUserControl();
            outcome.nameText.Text = Convert.ToChar(outcomesList.Children.Count + 65).ToString();
            outcomesList.Children.Add(outcome);
        }
        
        private void CreateCourseBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ValidForm())
            {
                CourseModel model = new CourseModel();
                model.Name = nameText.Text;
                model.Code = codeText.Text;
                model.EduYear = (EducationalYearModel)eduYearCombobox.SelectedItem;              
                foreach (OutcomeUserControl outcome in outcomesList.Children)
                {
                    CourseOutcomeModel cO = new CourseOutcomeModel();
                    cO.Name = outcome.nameText.Text;
                    cO.Description = outcome.descriptionText.Text;
                    model.CourseOutcomes.Add(cO);
                }
                GlobalConfig.Connection.CreateCourse(model);
                CallingWindow.CourseComplete(model);
                this.Close();
            }
        }

        // TODO - empliment validation
        private bool ValidForm()
        {
            if (nameText.Text != "" && codeText.Text != "" && eduYearCombobox.SelectedItem != null )
            {
                foreach (OutcomeUserControl outcome in outcomesList.Children)
                {
                    if (outcome.nameText.Text == "" || outcome.descriptionText.Text == "")
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

        private void CancelCourseBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
