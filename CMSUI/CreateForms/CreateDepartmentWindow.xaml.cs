using CMSLibrary;
using CMSLibrary.Enums;
using CMSLibrary.Models;
using CMSUI.Requesters;
using CMSUI.UserControls;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CMSUI.CreateForms
{
    /// <summary>
    /// Interaction logic for CreateDepartmentWindow.xaml
    /// </summary>
    public partial class CreateDepartmentWindow
    {
        IDepartmentRequester callingWindow;

        public List<int> outcomesToDelete = new List<int>();

        bool update;
        DepartmentModel department = new DepartmentModel();
        List<DepartmentOutcomeModel> addDepartmentOutcomes = new List<DepartmentOutcomeModel>();

        public CreateDepartmentWindow(IDepartmentRequester caller)
        {
            InitializeComponent();
            callingWindow = caller;
        }

        public CreateDepartmentWindow(IDepartmentRequester caller, DepartmentModel model)
        {
            InitializeComponent();
            callingWindow = caller;

            update = true;
            createDepartmentBtn.Content = "Update";
            titleText.Text = "Update the Department";
            title.Title = "UPDATE DEPARTMENT";
            department = model;

            nameText.Text = department.Name;
            foreach (var outcome in department.Outcomes)
            {
                OutcomeUserControl outcomeUserControl = new OutcomeUserControl();
                outcomeUserControl.nameText.Text = outcome.Name;
                outcomeUserControl.descriptionText.Text = outcome.Description;

                TagData td = new TagData
                {
                    IsNew = false,
                    IsDeletable = true,
                    Id = outcome.Id,
                    Type = OutcomeType.DepartmentOutcome
                };
                outcomeUserControl.Tag = td;

                outcomesList.Children.Add(outcomeUserControl);
            }

        }

        private void CreateDepartmentBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ValidForm())
            {
                if (!update)
                {
                    DepartmentModel model = new DepartmentModel
                    {
                        Name = nameText.Text
                    };
                    foreach (OutcomeUserControl outcome in outcomesList.Children)
                    {
                        DepartmentOutcomeModel dO = new DepartmentOutcomeModel
                        {
                            Name = outcome.nameText.Text,
                            Description = outcome.descriptionText.Text
                        };
                        model.Outcomes.Add(dO);
                    }
                    GlobalConfig.Connection.CreateDepartment(model);
                    callingWindow.DepartmentComplete(model);

                }
                else
                {

                    department.Name = nameText.Text;
                    foreach (OutcomeUserControl outcome in outcomesList.Children)
                    {
                        TagData td = (TagData)outcome.Tag;

                        DepartmentOutcomeModel dO = new DepartmentOutcomeModel
                        {
                            Id = td.Id,
                            Name = outcome.nameText.Text,
                            Description = outcome.descriptionText.Text,
                            DepartmentId = department.Id
                        };

                        if (td.IsNew == true)
                        {
                            GlobalConfig.Connection.CreateDepartmentOutcome(dO);
                        }
                        else
                        {
                            GlobalConfig.Connection.UpdateDepartmentOutcome(dO);
                        }
                    }
                    foreach (var delete in outcomesToDelete)
                    {
                        GlobalConfig.Connection.DepartmentOutcome_Delete(delete);
                    }
                    GlobalConfig.Connection.UpdateDepartment(department);
                    callingWindow.DepartmentUpdateComplete(department);


                }
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
            if (outcomesList.Children.Count == 0)
            {
                errorOutcomes.Visibility = Visibility.Visible;
            }
            if (nameText.Text == "")
            {
                errorName.Visibility = Visibility.Visible;
            }
            if (errorOutcomes.Visibility == Visibility.Visible || errorName.Visibility == Visibility.Visible)
            {
                return false;
            }
            else
            {
                return true;
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
            // TODO - fix the ascii code

            outcome.nameText.Text = Convert.ToChar(outcomesList.Children.Count + 65).ToString();

            TagData td = new TagData
            {
                Id = -1,
                IsDeletable = true,
                IsNew = true,
                Type = OutcomeType.CourseOutcome
            };
            outcome.Tag = td;

            outcomesList.Children.Add(outcome);

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
    }
}
