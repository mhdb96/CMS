﻿using CMSLibrary;
using CMSLibrary.DataAccess;
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
    /// Interaction logic for CreateTeacherWindow.xaml
    /// </summary>
    public partial class CreateTeacherWindow
    {
        ITeacherRequester CallingWindow;
        public CreateTeacherWindow(ITeacherRequester caller)
        {
            InitializeComponent();
            CallingWindow = caller;
        }

        private void CreateTeacherBtn_Click(object sender, RoutedEventArgs e)
        {
            if(ValidForm())
            {
                TeacherModel model = new TeacherModel();
                model.RegNo = int.Parse(regNoText.Text);
                model.FirstName = firstNameText.Text;
                model.LastName = lastNameText.Text;
                model.User.UserName = usernameText.Text;
                model.User.Password = passwordText.Password;
                GlobalConfig.Connection.CreateTeacher(model);
                CallingWindow.TeacherComplete(model);
                this.Close();
            }
                      
        }

        private void CancelTeacherBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private bool ValidForm()
        {
            // TODO - validate this form!
            if (regNoText.Text == "" || firstNameText.Text == "" || lastNameText.Text == "" || usernameText.Text == "" || usernameText.Text == "" || passwordText.Password.Length == 0)
            {
                if (regNoText.Text == "")
                {
                    errorRegisterNo.Visibility = Visibility.Visible;
                }
                if (firstNameText.Text == "")
                {
                    errorFirstName.Visibility = Visibility.Visible;
                }
                if (lastNameText.Text == "")
                {
                    errorLastName.Visibility = Visibility.Visible;
                }
                if (usernameText.Text == "")
                {
                    errorUserName.Visibility = Visibility.Visible;
                }
                if (passwordText.Password.Length == 0)
                {
                    errorPassword.Visibility = Visibility.Visible;
                }
                return false;
            }
            else
            {
                return true;
            }
        }

        private void RegNoText_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (regNoText.Text == "")
            {
                errorRegisterNo.Visibility = Visibility.Visible;
            }
            else
            {
                errorRegisterNo.Visibility = Visibility.Hidden;
            }
        }

        private void FirstNameText_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (firstNameText.Text == "")
            {
                errorFirstName.Visibility = Visibility.Visible;
            }
            else
            {
                errorFirstName.Visibility = Visibility.Hidden;
            }
        }

        private void LastNameText_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (lastNameText.Text == "")
            {
                errorLastName.Visibility = Visibility.Visible;
            }
            else
            {
                errorLastName.Visibility = Visibility.Hidden;
            }
        }

        private void UsernameText_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (usernameText.Text == "")
            {
                errorUserName.Visibility = Visibility.Visible;
            }
            else
            {
                errorUserName.Visibility = Visibility.Hidden;
            }
        }


        private void PasswordText_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (passwordText.Password.Length == 0)
            {
                errorPassword.Visibility = Visibility.Visible;
            }
            else
            {
                errorPassword.Visibility = Visibility.Hidden;
            }
        }
    }
}
