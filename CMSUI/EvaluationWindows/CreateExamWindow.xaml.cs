using CMSLibrary;
using CMSLibrary.Evaluation;
using CMSLibrary.Models;
using CMSUI.Requesters;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace CMSUI.EvaluationWindows
{
    /// <summary>
    /// Interaction logic for CreateExamWindow.xaml
    /// </summary>
    public partial class CreateExamWindow : IFixStudentsDataWindowRequester
    {
        public static readonly DependencyProperty ExamProperty =
        DependencyProperty.Register("Exam", typeof(ExamModel), typeof(CreateExamWindow), new FrameworkPropertyMetadata(null));

        private ExamModel Exam
        {
            get { return (ExamModel)GetValue(ExamProperty); }
            set { SetValue(ExamProperty, value); }
        }

        public static readonly DependencyProperty EvaluateProperty =
        DependencyProperty.Register("Evaluator", typeof(Evaluate), typeof(CreateExamWindow), new FrameworkPropertyMetadata(null));

        private Evaluate Evaluator
        {
            get { return (Evaluate)GetValue(EvaluateProperty); }
            set { SetValue(EvaluateProperty, value); }
        }

        IExamRequester CallingWindow;
        public CreateExamWindow(IExamRequester caller)
        {
            InitializeComponent();
            CallingWindow = caller;
            Evaluator = new Evaluate();
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
            if (answersKeyPath == "")
            {
                if (Evaluator.AnswersKeyPath == "")
                {
                    errorAsnwerKey.Visibility = Visibility.Visible;
                }

                return;
            }
            errorAsnwerKey.Visibility = Visibility.Collapsed;
            AnswersOutcomesMatrices.ItemsSource = null;
            AnswersOutcomesMatrices.Items.Clear();
            Exam.ExamGroups.Clear();
            Evaluator.AnswerKeys.Clear();
            Evaluator.GetAnswersKeys(answersKeyPath);
            List<GroupModel> Groups = GlobalConfig.Connection.GetGroup_All();
            foreach (AnswerKeyModel model in Evaluator.AnswerKeys)
            {
                ExamGroupModel examGroup = new ExamGroupModel();
                model.Group = Groups.Find(g => g.Name == model.Group.Name);
                examGroup.Group = model.Group;
                int i = 1;
                foreach (char c in model.AnswersList)
                {
                    QuestionModel question = new QuestionModel();
                    question.Name = i.ToString();
                    examGroup.Questions.Add(question);
                    i++;
                }
                Exam.ExamGroups.Add(examGroup);
            }
            answersOutcomesExpander.IsEnabled = true;
            answersOutcomesExpander.IsExpanded = true;
            GlobalConfig.Connection.GetCourseOutcomes_ById(Exam.Assignment.Course);
            AnswersOutcomesMatrices.ItemsSource = Exam.ExamGroups;
        }

        private void ChooseStudentsAnswersListBtn_Click(object sender, RoutedEventArgs e)
        {
            string studentsAnswersListPath = GetFilePath();
            if (studentsAnswersListPath == "")
            {
                if (Evaluator.StudentListPath == "")
                {
                    errorStudentList.Visibility = Visibility.Visible;
                }
                return;
            }
            errorStudentList.Visibility = Visibility.Collapsed;
            Evaluator.StudentsAnswers.Clear();
            Evaluator.GetStudentsAnswers(studentsAnswersListPath);
            if (Evaluator.StudentsAnswersWithErrors.Count > 0)
            {
                FixData();
            }
            else
            {
                ShowData();
            }
        }

        void ShowData()
        {

            studentAnswersListUserControl.Refresh();
            List<GroupModel> Groups = GlobalConfig.Connection.GetGroup_All();
            foreach (StudentAnswersModel model in Evaluator.StudentsAnswers)
            {
                model.Group = Groups.Find(g => g.Name == model.Group.Name);
            }
            studentsAnswersExpander.IsEnabled = true;
            studentsAnswersExpander.IsExpanded = true;
        }

        void FixData()
        {
            FixStudentsDataWindow win = new FixStudentsDataWindow(this, Evaluator);
            win.ShowDialog();
        }
        private void CreateExamBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ValidForm())
            {
                Exam.ExamType = (ExamTypeModel)examTypesCombobox.SelectedItem;
                Exam.Date = (DateTime)examDate.SelectedDate;
                GlobalConfig.Connection.CreateExam(Exam);
                foreach (ExamGroupModel examGroup in Exam.ExamGroups)
                {
                    examGroup.ExamId = Exam.Id;
                    GlobalConfig.Connection.CreateExamGroup(examGroup);
                    AnswerKeyModel answerKey = Evaluator.AnswerKeys.Find(a => a.Group.Name == examGroup.Group.Name);
                    int counter = 0;
                    foreach (QuestionModel question in examGroup.Questions)
                    {
                        question.ExamGroupId = examGroup.Id;
                        GlobalConfig.Connection.CreateQuestion(question);
                        foreach (var studentAnswers in Evaluator.StudentsAnswers)
                        {
                            if (studentAnswers.Group.Name == answerKey.Group.Name)
                            {
                                ResultModel r = new ResultModel
                                {
                                    QuestionId = question.Id
                                };
                                StudentModel model = GlobalConfig.Connection.GetStudent_ByRegNo(studentAnswers.Student.RegNo);
                                r.Student = model;
                                if (studentAnswers.AnswersList[counter].ToString() == answerKey.AnswersList.Substring(counter, 1))
                                {
                                    r.IsTrue = true;
                                }
                                else
                                {
                                    r.IsTrue = false;
                                }
                                GlobalConfig.Connection.CreateResult(r);
                            }
                        }
                        counter++;
                        foreach (CourseOutcomeModel courseOutcome in question.QuestionOutcomes)
                        {
                            QuestionOutcomeModel model = new QuestionOutcomeModel();
                            model.CourseOutcomeId = courseOutcome.Id;
                            model.QuestionId = question.Id;
                            GlobalConfig.Connection.CreteQuestionOutcome(model);
                        }
                    }
                }
                CallingWindow.ExamComplete(Exam);
                this.Close();
            }

        }

        private void CancelExamBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void FixComplete(bool val)
        {
            if (!val)
            {
                FixData();
            }
            else
            {
                ShowData();
            }
        }
        private bool ValidForm()
        {

            if (examDate.SelectedDate == null)
            {
                errorDate.Visibility = Visibility.Visible;
            }
            if (Evaluator.AnswersKeyPath == "")
            {
                errorAsnwerKey.Visibility = Visibility.Visible;
            }
            if (Evaluator.StudentListPath == "")
            {
                errorStudentList.Visibility = Visibility.Visible;
            }
            if (examTypesCombobox.SelectedItem == null)
            {
                errorExamType.Visibility = Visibility.Visible;
            }
            if (errorAsnwerKey.Visibility == Visibility.Visible || errorDate.Visibility == Visibility.Visible || errorExamType.Visibility == Visibility.Visible || errorStudentList.Visibility == Visibility.Visible)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void ExamTypesCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (examTypesCombobox.SelectedItem == null)
            {
                errorExamType.Visibility = Visibility.Visible;
            }
            else
            {
                errorExamType.Visibility = Visibility.Collapsed;
            }
        }

        private void ExamDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (examDate.SelectedDate == null)
            {
                errorDate.Visibility = Visibility.Visible;
            }
            else
            {
                errorDate.Visibility = Visibility.Collapsed;
            }
        }
    }
}
