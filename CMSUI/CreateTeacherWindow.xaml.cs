using CMSLibrary;
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
            if (regNoText.Text == "" || firstNameText.Text == "" || lastNameText.Text == "" || usernameText.Text == "" || usernameText.Text == "" )
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
