using CMSLibrary.Models;
using CMSUI.Requesters;
using CMSUI.EvaluationWindows;
using MahApps.Metro.Controls.Dialogs;
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
    }
}
