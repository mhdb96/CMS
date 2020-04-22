using CMSLibrary;
using CMSLibrary.Enums;
using CMSLibrary.Models;
using CMSUI.LoginWindows;
using CMSUI.Panels;
using CMSUI.Requesters;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace CMSUI
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : IAdminPanelRequester, ITeacherPanelRequester, IDatabaseSettingRequester
    {

        UserModel User;
        public LoginWindow()
        {
            InitializeComponent();
        }

        public AdminModel GetAdminInfo()
        {
            AdminModel model = new AdminModel();
            model = GlobalConfig.Connection.GetAdmin_ByUserId(User.Id);
            model.User = User;
            return model;
        }

        public TeacherModel GetTeacherInfo()
        {
            TeacherModel model = new TeacherModel();
            model = GlobalConfig.Connection.GetTeacher_ByUserId(User.Id);
            model.User = User;
            return model;
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            errorUserName.Visibility = Visibility.Collapsed;
            errorPassword.Visibility = Visibility.Collapsed;
            User = new UserModel
            {
                UserName = usernameText.Text,
                Password = passwordText.Password
            };
            AuthenticationState authState = Login.Check(User);
            if (authState == AuthenticationState.UserNotFound)
            {
                errorUsernameText.Text = "User not found";
                errorUserName.Visibility = Visibility.Visible;
            }
            else if (authState == AuthenticationState.WrongPassword)
            {
                errorPasswordText.Text = "You've entered a wrong password";
                errorPassword.Visibility = Visibility.Visible;
            }
            else if (authState == AuthenticationState.Authenticated)
            {
                if (User.Role.Name == "Admin")
                {
                    AdminPanelWindow win = new AdminPanelWindow(this);
                    win.Show();
                }
                else
                {
                    TeacherPanelWindow win = new TeacherPanelWindow(this);
                    win.Show();
                }
                this.Hide();
            }
        }

        private async Task ShowProgressDialogAsync()
        {
            var mySettings = new MetroDialogSettings()
            {
                AnimateShow = false,
                AnimateHide = false,
                ColorScheme = this.MetroDialogOptions.ColorScheme,
                MaximumBodyHeight = 100
            };
            var controller = await this.ShowProgressAsync("Please wait...", "Connectting to the database", settings: mySettings);
            controller.SetIndeterminate();
            string errMsg = await Task.Run(() => GlobalConfig.Connection.CheckConniction());
            await Task.Delay(1000);
            await controller.CloseAsync();
            if (errMsg == "")
            {
                await this.ShowMessageAsync("Success", "You have succesfully connected to the database.");
                //DatabaseSettingWindow win = new DatabaseSettingWindow(this);
                //win.ShowDialog();
            }
            else
            {
                await this.ShowMessageAsync("Failure", $"A connection couldn't be established to the database. {Environment.NewLine}Error Message: {Environment.NewLine}{errMsg}", MessageDialogStyle.Affirmative, mySettings);
            }
        }

        private async void MetroWindow_ContentRendered(object sender, EventArgs e)
        {
            await ShowProgressDialogAsync();
        }

        private void DatabaseBtn_Click(object sender, RoutedEventArgs e)
        {
            DatabaseSettingWindow win = new DatabaseSettingWindow(this);
            win.ShowDialog();
        }

        private void InfoBtn_Click(object sender, RoutedEventArgs e)
        {
            DevelopersInfoWindow win = new DevelopersInfoWindow();
            win.ShowDialog();
        }

        private void ClearFields()
        {
            usernameText.Text = "";
            passwordText.Password = "";
        }

        public void AdminPanelClosed()
        {
            this.Show();
            ClearFields();
        }

        public void TeacherPanelClosed()
        {
            this.Show();
            ClearFields();
        }

        public async Task DatabaseSettingSaved()
        {
            await ShowProgressDialogAsync();
        }
    }
}
