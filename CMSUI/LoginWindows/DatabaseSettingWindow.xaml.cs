using CMSLibrary;
using CMSUI.Requesters;
using System.IO;
using System.Windows;

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
