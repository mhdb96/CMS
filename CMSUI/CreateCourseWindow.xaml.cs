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
    /// 
    public partial class CreateCourseWindow
    {
        ICouresRequester CallingWindow;
        List<EducationalYearModel> EduYears;

        bool update;
        CourseModel course = new CourseModel();
        List<CourseOutcomeModel> newCourseOutcomes = new List<CourseOutcomeModel>();

        public CreateCourseWindow(ICouresRequester caller)
        {
            InitializeComponent();
            CallingWindow = caller;
            LoadListsData();
        }
        public CreateCourseWindow(ICouresRequester caller, CourseModel model)
        {
            InitializeComponent();
            CallingWindow = caller;
            LoadListsData();

            update = true;
            createCourseBtn.Content = "Update";

            course = model;

            nameText.Text = course.Name;
            codeText.Text = course.Code;

            foreach (var eduYear in EduYears)
            {
                if (eduYear.Id == course.EduYear.Id)
                {
                    eduYearCombobox.SelectedItem = eduYear;
                }
            }
            
            foreach (var outcome in course.CourseOutcomes)
            {
                OutcomeUserControl outcomeUserControl = new OutcomeUserControl();

                outcomeUserControl.nameText.Text = outcome.Name;
                outcomeUserControl.descriptionText.Text = outcome.Description;
                TagData td = new TagData();
                td.IsNew = false;
                td.IsDeletable = GlobalConfig.Connection.CourseOutcome_IsDeletable(outcome.Id);
                td.Id = outcome.Id;
                td.Type = OutcomeType.CourseOutcome;
                outcomeUserControl.Tag = td;
                outcomesList.Children.Add(outcomeUserControl);
            }
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
            TagData td = new TagData();
            td.Id = -1;
            td.IsDeletable = true;
            td.IsNew = true;
            td.Type = OutcomeType.CourseOutcome;
            outcome.Tag = td;
            outcomesList.Children.Add(outcome);
        }
        
        private void CreateCourseBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ValidForm())
            {
                if (!update)
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

                }
                else
                {
                    course.Name = nameText.Text;
                    course.Code = codeText.Text;
                    course.EduYear = (EducationalYearModel)eduYearCombobox.SelectedItem;                    
                    foreach (OutcomeUserControl outcome in outcomesList.Children)
                    {
                        CourseOutcomeModel cO = new CourseOutcomeModel();
                        cO.Name = outcome.nameText.Text;
                        cO.Description = outcome.descriptionText.Text;
                        cO.CourseId = course.Id;
                        TagData td = (TagData)outcome.Tag;
                        if(td.IsNew == true)
                        {
                            
                            GlobalConfig.Connection.CreateCourseOutcome(cO);
                        }
                        else
                        {
                            GlobalConfig.Connection.UpdateCourseOutcome(cO);
                        }
                    }                    
                    GlobalConfig.Connection.UpdateCourse(course);
                    CallingWindow.CourseUpdateComplete(course);

                }
                this.Close();

            }
        }

        // TODO - empliment validation
        private bool ValidForm()
        {
            foreach (OutcomeUserControl outcome in outcomesList.Children)
            {
                if (outcome.nameText.Text == "" || outcome.descriptionText.Text == "")
                {
                    if (outcome.descriptionText.Text == "")
                    {
                        outcome.descriptionText.BorderBrush = Brushes.Red;
                    }
                    errorOutcomes.Visibility = Visibility.Visible;
                }
            }

            if (nameText.Text == "")
            {
                errorName.Visibility = Visibility.Visible;
            }
            if (codeText.Text == "")
            {
                errorCode.Visibility = Visibility.Visible;
            }
            if (eduYearCombobox.SelectedItem == null)
            {
                errorYear.Visibility = Visibility.Visible;
            }
            if (errorName.Visibility == Visibility.Visible || errorCode.Visibility == Visibility.Visible || errorYear.Visibility == Visibility.Visible || errorOutcomes.Visibility == Visibility.Visible)
            {
                return false;
            }
            else
            {
                return true;
            }


        }

        private void CancelCourseBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void NameText_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (nameText.Text == "")
            {
                errorName.Visibility = Visibility.Visible;
            }
            else
            {
                errorName.Visibility = Visibility.Hidden;
            }
        }

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            errorOutcomes.Visibility = Visibility.Hidden;

            foreach (OutcomeUserControl outcome in outcomesList.Children)
            {
                if (outcome.nameText.BorderBrush == Brushes.Red || outcome.descriptionText.BorderBrush == Brushes.Red)
                {
                    errorOutcomes.Visibility = Visibility.Visible;
                }
            }
        }

        private void CodeText_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (codeText.Text == "")
            {
                errorCode.Visibility = Visibility.Visible;
            }
            else
            {
                errorCode.Visibility = Visibility.Hidden;
            }
        }

        private void EduYearCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (eduYearCombobox.SelectedItem == null)
            {
                errorYear.Visibility = Visibility.Visible;
            }
            else
            {
                errorYear.Visibility = Visibility.Hidden;
            }
        }
    }

}
