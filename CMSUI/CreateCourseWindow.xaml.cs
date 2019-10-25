using CMSLibrary;
using CMSLibrary.Models;
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
        List<EducationalYearModel> EduYears;
        public CreateCourseWindow()
        {
            InitializeComponent();
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
            }
        }

        // TODO - empliment validation
        private bool ValidForm()
        {
            bool valid = true;
            return valid;
        }

        private void CancelCourseBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
