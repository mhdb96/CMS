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
    /// Interaction logic for TeacherPanelWindow.xaml
    /// </summary>
    /// 
    public partial class TeacherPanelWindow : IParentWindow

    {
        ITeacherPanelRequester CallingWindow;
        public TeacherPanelWindow(ITeacherPanelRequester caller)
        {
            InitializeComponent();
            CallingWindow = caller;
            Teacher = CallingWindow.GetTeacherInfo();
        }
        public static readonly DependencyProperty TeacherProperty =
        DependencyProperty.Register("Teacher", typeof(TeacherModel), typeof(TeacherPanelWindow), new FrameworkPropertyMetadata(null));

        private TeacherModel Teacher
        {
            get { return (TeacherModel)GetValue(TeacherProperty); }
            set { SetValue(TeacherProperty, value); }
        }

        private void TeacherPanel_Closed(object sender, EventArgs e)
        {
            CallingWindow.TeacherPanelClosed();
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
                case "My Profile":
                    userControl = new MyProfileHelpUserControl();
                    break;
                case "My Courses":
                    userControl = new MyExamDashboardHelpUserControl();
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
