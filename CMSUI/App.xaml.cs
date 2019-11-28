using CMSLibrary;
using MahApps.Metro;
using Squirrel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace CMSUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            ThemeManager.ChangeAppStyle(Application.Current,
                                    ThemeManager.GetAccent("Cobalt"),
                                    ThemeManager.GetAppTheme("BaseDark"));
            base.OnStartup(e);
            GlobalConfig.InitializeConnections();
            CheckForUpdates();
        }
        private async Task CheckForUpdates()
        {
            using (var manager = new UpdateManager(@"C:\r"))
            {
                await manager.UpdateApp();
            }
        }
    }
}
