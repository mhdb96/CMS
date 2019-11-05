using CMSLibrary;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CMSUI.UserControls
{
    /// <summary>
    /// Interaction logic for StudentsAnswersListUserControl.xaml
    /// </summary>
    public partial class StudentsAnswersListUserControl : UserControl
    {
        public static readonly DependencyProperty MyEvaluateProperty =
        DependencyProperty.Register("MyEvaluator", typeof(Evaluate), typeof(StudentsAnswersListUserControl), new FrameworkPropertyMetadata(null));

        public Evaluate MyEvaluator
        {
            get { return (Evaluate)GetValue(MyEvaluateProperty); }
            set { SetValue(MyEvaluateProperty, value); }
        }

        public StudentsAnswersListUserControl()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            studentsAnswersGrid.ItemsSource = MyEvaluator.StudentsAnswers;
        }
        public void refresh()
        {            
            studentsAnswersGrid.ItemsSource = null;
            studentsAnswersGrid.Items.Clear();
            studentsAnswersGrid.ItemsSource = MyEvaluator.StudentsAnswers;            
        }

        private void Test_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
