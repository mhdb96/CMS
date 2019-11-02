using CMSLibrary;
using CMSLibrary.Models;
using CMSUI.Requesters;
using CMSUI.UserControls;
using Microsoft.Win32;
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

namespace CMSUI.EvaluationWindows
{
    /// <summary>
    /// Interaction logic for CreateExamWindow.xaml
    /// </summary>
    public partial class CreateExamWindow
    {

        public static readonly DependencyProperty ExamProperty =
        DependencyProperty.Register("Exam", typeof(ExamModel), typeof(CreateExamWindow), new FrameworkPropertyMetadata(null));        

        private ExamModel Exam
        {
            get { return (ExamModel)GetValue(ExamProperty); }
            set { SetValue(ExamProperty, value); }
        }

        IExamRequester CallingWindow;
        public CreateExamWindow(IExamRequester caller)
        {
            InitializeComponent();
            CallingWindow = caller;
            Exam = new ExamModel();
            Exam.Assignment = CallingWindow.GetAssignment();
            Exam.User = CallingWindow.GetUserInfo();
            examTypesCombobox.ItemsSource = GlobalConfig.Connection.GetExamType_All();

        }

        public string GetFilePath()
        {
            OpenFileDialog file = new OpenFileDialog();
            file.Filter = ".txt Dosyası |*.txt";
            file.ShowDialog();
            string path = file.FileName;
            return path;

        }

        private void ChooseAnswerKeyBtn_Click(object sender, RoutedEventArgs e)
        {
            
            string answersKeyPath = GetFilePath();
            if(answersKeyPath == "")
            {
                return;
            }
            Evaluate ev = new Evaluate();
            List<AnswerKeyModel> answerKeys = ev.AnswersList(answersKeyPath);
            List<GroupModel> Groups = GlobalConfig.Connection.GetGroup_All();
            foreach (AnswerKeyModel model in answerKeys)
            {
                ExamGroupModel examGroup = new ExamGroupModel();
                model.Group = Groups.Find(g => g.Name == model.Group.Name);
                examGroup.Group = model.Group;
                int i = 1;
                foreach (char c in model.AnswersList)
                {
                    QuestionModel question = new QuestionModel();
                    question.Question = i.ToString();
                    examGroup.Questions.Add(question);
                    i++;
                }
                Exam.ExamGroups.Add(examGroup);
            }
            answersOutcomesExpander.IsEnabled = true;
            answersOutcomesExpander.IsExpanded = true;
            //Exam.Assignment.Course.Id = 7;
            GlobalConfig.Connection.GetCourseOutcomes_ById(Exam.Assignment.Course);
            test.ItemsSource = Exam.ExamGroups;
        }
        
        private void ChooseAnswersListBtn_Click(object sender, RoutedEventArgs e)
        {
            string answersListPath = GetFilePath();
        }

        private void CreateExamBtn_Click(object sender, RoutedEventArgs e)
        {
            test.ItemsSource = null;
            test.Items.Clear();
            Exam.ExamGroups.Clear();
            //Exam.ExamType = (ExamTypeModel)examTypesCombobox.SelectedItem;
            //Exam.Date = (DateTime)examDate.SelectedDate;
        }

        private void CancelExamBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
