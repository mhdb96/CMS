using CMSLibrary;
using CMSLibrary.Models;
using CMSUI.EvaluationWindows;
using CMSUI.Requesters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for AnswersOutcomesUserControl.xaml
    /// </summary>
    public partial class AnswersOutcomesUserControl : UserControl
    {
        public static readonly DependencyProperty MyExamProperty =
        DependencyProperty.Register("MyExam", typeof(ExamModel), typeof(AnswersOutcomesUserControl), new FrameworkPropertyMetadata(null));

        public ExamModel MyExam
        {
            get { return (ExamModel)GetValue(MyExamProperty); }
            set { SetValue(MyExamProperty, value); }
        }

        int GroupIndex;

        public AnswersOutcomesUserControl()
        {
            InitializeComponent();
        }
        
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            groupNameText.Text = $"{this.Tag.ToString()} Group";
            GroupIndex = MyExam.ExamGroups.FindIndex(q => q.Group.Name == this.Tag.ToString());
            CreateGridDefinition();
            CreateCheckBoxesMatrix();            
        }

        private void CreateCheckBoxesMatrix()
        {
            int i = 1;
            foreach (CourseOutcomeModel o in MyExam.Assignment.Course.CourseOutcomes)
            {
                TextBlock tb = new TextBlock();
                tb.Text = o.Name;
                tb.ToolTip = o.Description;
                tb.Margin = new Thickness(10, 0, 10, 0);
                Grid.SetRow(tb, 0);
                Grid.SetColumn(tb, i);
                answersOutcomesMatrix.Children.Add(tb);
                i++;
            }
            i = 1;
            foreach (QuestionModel q in MyExam.ExamGroups[GroupIndex].Questions)
            {
                TextBlock tb = new TextBlock();
                tb.Text = q.Question;
                tb.ToolTip = $"Question {q.Question}";
                tb.Margin = new Thickness(0, 10, 0, 10);
                Grid.SetRow(tb, i);
                Grid.SetColumn(tb, 0);
                answersOutcomesMatrix.Children.Add(tb);
                i++;
            }

            i = 1;
            foreach (QuestionModel q in MyExam.ExamGroups[GroupIndex].Questions)
            {
                TextBox mark = new TextBox();
                mark.Tag = q;
                mark.Text = (100.0 / MyExam.ExamGroups[GroupIndex].Questions.Count).ToString("0.##");
                mark.Width = 40;
                mark.VerticalAlignment = VerticalAlignment.Center;
                mark.Margin = new Thickness(0, 10, 0, 10);
                Grid.SetRow(mark, i);
                Grid.SetColumn(mark, answersOutcomesMatrix.ColumnDefinitions.Count - 1);
                answersOutcomesMatrix.Children.Add(mark);

                TextBlock tb = new TextBlock();
                tb.Text = "Mark = ";
                tb.Margin = new Thickness(0, 10, 0, 10);
                Grid.SetRow(tb, i);
                Grid.SetColumn(tb, answersOutcomesMatrix.ColumnDefinitions.Count - 2);
                answersOutcomesMatrix.Children.Add(tb);
                i++;
            }
            i = 1;
            foreach (QuestionModel q in MyExam.ExamGroups[GroupIndex].Questions)
            {
                int j = 1;
                foreach (CourseOutcomeModel o in MyExam.Assignment.Course.CourseOutcomes)
                {
                    CheckBox cb = new CheckBox();
                    cb.Margin = new Thickness(10);
                    Grid.SetRow(cb, i);
                    Grid.SetColumn(cb, j);
                    answersOutcomesMatrix.Children.Add(cb);
                    j++;
                }
                i++;
            }
        }

        private void CreateGridDefinition()

        {
            RowDefinition r = new RowDefinition();
            r.Height = new GridLength();
            answersOutcomesMatrix.RowDefinitions.Add(r);

            foreach (QuestionModel q in MyExam.ExamGroups[GroupIndex].Questions)
            {
                RowDefinition rd = new RowDefinition();
                rd.Tag = q;
                rd.Height = new GridLength();
                answersOutcomesMatrix.RowDefinitions.Add(rd);
            }

            ColumnDefinition c = new ColumnDefinition();
            c.Width = new GridLength();
            answersOutcomesMatrix.ColumnDefinitions.Add(c);

            foreach (CourseOutcomeModel o in MyExam.Assignment.Course.CourseOutcomes)
            {
                ColumnDefinition cd = new ColumnDefinition();
                cd.Tag = o;
                cd.Width = new GridLength();
                answersOutcomesMatrix.ColumnDefinitions.Add(cd);
            }

            for (int k = 0; k < 2; k++)
            {
                c = new ColumnDefinition();
                c.Width = new GridLength();
                answersOutcomesMatrix.ColumnDefinitions.Add(c);
            }
        }

        private void SaveMarksAmdOutcomesBtn_Click(object sender, RoutedEventArgs e)
        {
            foreach (UIElement Child in answersOutcomesMatrix.Children)
            {                
                if (Child is TextBox)
                {
                    QuestionModel question = (QuestionModel)answersOutcomesMatrix.RowDefinitions[Grid.GetRow(Child)].Tag;
                    int questionIndex = MyExam.ExamGroups[GroupIndex].Questions.FindIndex(q => q.Question == question.Question);
                    decimal mark = decimal.Parse(((TextBox)Child).Text);
                    MyExam.ExamGroups[GroupIndex].Questions[questionIndex].Mark = mark;                                       
                }
                else if (Child is CheckBox)
                {
                    if (((CheckBox)Child).IsChecked == true)
                    {
                        CourseOutcomeModel outcome = (CourseOutcomeModel)answersOutcomesMatrix.ColumnDefinitions[Grid.GetColumn(Child)].Tag;
                        QuestionModel question = (QuestionModel)answersOutcomesMatrix.RowDefinitions[Grid.GetRow(Child)].Tag;
                        int questionIndex = MyExam.ExamGroups[GroupIndex].Questions.FindIndex(q => q.Question == question.Question);
                        MyExam.ExamGroups[GroupIndex].Questions[questionIndex].QuestionOutcomes.Add(outcome);
                    }                                        
                }
                
            }
        }
    }
}
