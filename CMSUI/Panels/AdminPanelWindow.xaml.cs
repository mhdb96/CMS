using CMSLibrary.Models;
using CMSUI.Requesters;
using CMSUI.UserControls.HelpMenus;
using CMSUI.UserControls.HelpMenus.Exam;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CMSUI.Panels
{
    /// <summary>
    /// Interaction logic for AdminPanelWindow.xaml
    /// </summary>
    /// 
    public partial class AdminPanelWindow : IParentWindow
    {
        IAdminPanelRequester CallingWindow;
        public AdminPanelWindow(IAdminPanelRequester caller)
        {
            InitializeComponent();
            CallingWindow = caller;
            Admin = CallingWindow.GetAdminInfo();
            myCoursesControl.MyAdmin = Admin;
        }

        public static readonly DependencyProperty AdminProperty =
        DependencyProperty.Register("Admin", typeof(AdminModel), typeof(AdminPanelWindow), new FrameworkPropertyMetadata(null));

        private AdminModel Admin
        {
            get { return (AdminModel)GetValue(AdminProperty); }
            set { SetValue(AdminProperty, value); }
        }

        private void MetroWindow_Closed(object sender, EventArgs e)
        {
            CallingWindow.AdminPanelClosed();
        }

        public async Task<MessageDialogResult> ShowMessage(string title, string message, MessageDialogStyle style)
        {
            return await this.ShowMessageAsync(title, message, style, null);
        }

        private void TutorialTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeViewItem treeItem = (TreeViewItem)tutorialTree.SelectedItem;
            UserControl userControl = new UserControl();
            if (!treeItem.HasItems)
            {
                switch (treeItem.Header.ToString())
                {
                    case "Departments Dashboard":
                        userControl = new DepartmentHelpUserControl();
                        break;
                    case "Teachers Dashboard":
                        userControl = new TeacherHelpUserControl();
                        break;
                    case "Terms Dashboard":
                        userControl = new TermHelpUserControl();
                        break;
                    case "Courses Dashboard":
                        userControl = new CourseHelpUserControl();
                        break;
                    case "Assignments Dashboard":
                        userControl = new AssignmentHelpUserControl();
                        break;
                    case "Exams Dashboard":
                        userControl = new ExamDashboardHelpUserControl();
                        break;
                    case "My Exams Dashboard":
                        userControl = new MyExamDashboardHelpUserControl();
                        break;
                    case "My Profile":
                        userControl = new MyProfileHelpUserControl();
                        break;
                    case "Insert Students":
                        userControl = new InsertStudentsHelpUserControl();
                        break;
                    case "Create Exam":
                        userControl = new CreateExamHelpUserControl();
                        break;
                    default: break;
                }
                if (userControl != null)
                {
                    helpSP.Children.Clear();
                    helpSP.Children.Add(userControl);
                }

            }
        }

        private void HelpBtn_Click(object sender, RoutedEventArgs e)
        {
            TabItem item = (TabItem)tabs.SelectedItem;
            UserControl userControl = new UserControl();
            switch (item.Header.ToString())
            {
                case "Departments":
                    userControl = new DepartmentHelpUserControl();
                    break;
                case "Teachers":
                    userControl = new TeacherHelpUserControl();
                    break;
                case "Terms":
                    userControl = new TermHelpUserControl();
                    break;
                case "Courses":
                    userControl = new CourseHelpUserControl();
                    break;
                case "Assignments":
                    userControl = new AssignmentHelpUserControl();
                    break;
                case "Exams":
                    userControl = new ExamDashboardHelpUserControl();
                    break;
                default: break;
            }
            flyout.IsOpen = true;
            if (userControl != null)
            {
                helpSP.Children.Clear();
                helpSP.Children.Add(userControl);
            }
        }
    }
}
