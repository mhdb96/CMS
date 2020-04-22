using CMSLibrary.Evaluation;
using System.Windows;
using System.Windows.Controls;

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
        public void Refresh()
        {
            studentsAnswersGrid.ItemsSource = null;
            studentsAnswersGrid.Items.Clear();
            studentsAnswersGrid.ItemsSource = MyEvaluator.StudentsAnswers;
        }
    }
}
