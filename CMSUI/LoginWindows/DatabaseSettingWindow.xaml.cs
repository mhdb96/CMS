using System;
using System.Collections.Generic;
using System.IO;
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
            File.WriteAllText($"{System.AppDomain.CurrentDomain.BaseDirectory}info.txt", $"{GlobalConfig.Ip};{GlobalConfig.Port};{GlobalConfig.Username};{GlobalConfig.Password};");
            this.Close();
            CallinWindow.DatabaseSettingSaved();
        }
    }
}
