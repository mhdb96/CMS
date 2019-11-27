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
    public partial class AdminPanelWindow
    {
        //public string test = "10";
        IAdminPanelRequester CallingWindow;
        AdminModel Admin;
        public AdminPanelWindow(IAdminPanelRequester caller)
        {
            InitializeComponent();
            CallingWindow = caller;
            Admin = CallingWindow.GetAdminInfo();            
        }
        public async Task<MessageDialogResult> ShowMessageOnAdmin(string title, string message, MessageDialogStyle style)
        {
            return await this.ShowMessageAsync(title, message, style, null);    
        }
    }
}
