using System.Diagnostics;

namespace CMSUI.LoginWindows
{
    /// <summary>
    /// Interaction logic for DevelopersInfoWindow.xaml
    /// </summary>
    public partial class DevelopersInfoWindow
    {
        public DevelopersInfoWindow()
        {
            InitializeComponent();
        }

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }
}
