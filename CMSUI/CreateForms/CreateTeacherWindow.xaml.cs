using CMSLibrary;
using CMSLibrary.Models;
using CMSUI.Requesters;
using System.Windows;
using System.Windows.Controls;

namespace CMSUI.CreateForms
{
    /// <summary>
    /// Interaction logic for CreateTeacherWindow.xaml
    /// </summary>
    public partial class CreateTeacherWindow
    {
        ITeacherRequester CallingWindow;

        bool update;
        TeacherModel model = new TeacherModel();

        public CreateTeacherWindow(ITeacherRequester caller)
        {
            InitializeComponent();
            CallingWindow = caller;

            update = false;
            createTeacherBtn.Content = "Create";//gerek var mı?
        }
        public CreateTeacherWindow(ITeacherRequester caller, TeacherModel teacher)
        {
            InitializeComponent();
            CallingWindow = caller;

            update = true;
            createTeacherBtn.Content = "Update";
            titleText.Text = "Update The Teacher";
            title.Title = "UPDATE TEACHER";

            model = teacher;

            regNoText.Text = model.RegNo.ToString();
            firstNameText.Text = model.FirstName;
            lastNameText.Text = model.LastName;
            usernameText.Text = model.User.UserName;
            passwordText.Password = model.User.Password;
        }

        private void CreateTeacherBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ValidForm())
            {
                model.RegNo = int.Parse(regNoText.Text);
                model.FirstName = firstNameText.Text;
                model.LastName = lastNameText.Text;
                model.User.UserName = usernameText.Text;
                model.User.Password = passwordText.Password;

                if (!GlobalConfig.Connection.User_ValidByUsername(model.User.UserName))
                {
                    errorUserName.Visibility = Visibility.Visible;
                    errorUserNameText.Text = "Username is already in use";
                    return;
                }


                if (!update)
                {
                    GlobalConfig.Connection.CreateTeacher(model);
                    CallingWindow.TeacherComplete(model);
                    this.Close();
                }
                else
                {
                    GlobalConfig.Connection.UpdateTeachers(model);
                    CallingWindow.TeacherComplete(model);
                    this.Close();

                }
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
                    errorUserNameText.Text = "Username is required";
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
                errorUserNameText.Text = "Username is required";
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
