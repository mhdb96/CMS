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
using CMSLibrary;
using CMSUI.Requesters;

namespace CMSUI.LoginWindows
{
    /// <summary>
    /// Interaction logic for DatabaseSettingWindow.xaml
    /// </summary>
    public partial class DatabaseSettingWindow
    {
        IDatabaseSettingRequester CallinWindow;
        public DatabaseSettingWindow(IDatabaseSettingRequester caller)
        {
            InitializeComponent();
            CallinWindow = caller;
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            GlobalConfig.Ip = ipText.Text;
            GlobalConfig.Port = portText.Text;
            GlobalConfig.Username = usernameText.Text;
            GlobalConfig.Password = passwordText.Password;
            this.Close();
            CallinWindow.DatabaseSettingSaved();
        }
    }
}
