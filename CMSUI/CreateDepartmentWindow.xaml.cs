﻿using System;
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

            department = model;

            nameText.Text = department.Name;
            foreach (var outcome in department.Outcomes)
            {
                OutcomeUserControl outcomeUserControl = new OutcomeUserControl();
                outcomeUserControl.nameText.Text = outcome.Name;
                outcomeUserControl.descriptionText.Text = outcome.Description;
                outcomesList.Children.Add(outcomeUserControl);
            }

        }

        private void CreateDepartmentBtn_Click(object sender, RoutedEventArgs e)
        {
            if(ValidForm())
            {
                if (!update)
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

                }
                else
                {
                    department.Name = nameText.Text;

                    foreach (DepartmentOutcomeModel addDepartmentOutcome in addDepartmentOutcomes)
                    {
                        department.Outcomes.Add(addDepartmentOutcome);
                    }

                    int i = 0;
                    foreach (OutcomeUserControl outcome in outcomesList.Children)
                    {
                        department.Outcomes[i].Name = outcome.nameText.Text;
                        department.Outcomes[i].Description = outcome.descriptionText.Text;
                        i++;
                    }
                    foreach (DepartmentOutcomeModel addDepartmentOutcome in addDepartmentOutcomes)
                    {
                        addDepartmentOutcome.DepartmentId = department.Id;
                        GlobalConfig.Connection.CreateDepartmentOutcome(addDepartmentOutcome);
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
            outcomesList.Children.Add(outcome);

            if (update)
            {
                DepartmentOutcomeModel dO = new DepartmentOutcomeModel();
                dO.Name = outcome.nameText.Text;
                dO.Description = outcome.descriptionText.Text;
                addDepartmentOutcomes.Add(dO);
            }

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
