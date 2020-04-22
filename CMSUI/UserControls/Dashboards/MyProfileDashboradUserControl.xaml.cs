using CMSLibrary;
using CMSLibrary.Models;
using System.Windows;
using System.Windows.Controls;

namespace CMSUI.UserControls
{
    /// <summary>
    /// Interaction logic for MyProfileDashboradUserControl.xaml
    /// </summary>
    public partial class MyProfileDashboradUserControl : UserControl
    {
        public static readonly DependencyProperty MTeacherProperty =
        DependencyProperty.Register("MTeacher", typeof(TeacherModel), typeof(MyProfileDashboradUserControl), new FrameworkPropertyMetadata(null));

        public TeacherModel MTeacher
        {
            get { return (TeacherModel)GetValue(MTeacherProperty); }
            set { SetValue(MTeacherProperty, value); }
        }

        public MyProfileDashboradUserControl()
        {
            InitializeComponent();



        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            regNoText.Text = MTeacher.RegNo.ToString();
            firstNameText.Text = MTeacher.FirstName;
            lastNameText.Text = MTeacher.LastName;
            usernameText.Text = MTeacher.User.UserName;
            passwordText.Password = MTeacher.User.Password;
        }


        private void PasswoedUpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            MTeacher.User.Password = passwordText.Password;
            GlobalConfig.Connection.UpdateTeachers(MTeacher);
        }
    }
}
