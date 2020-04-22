using CMSLibrary.Evaluation;
using CMSLibrary.Models;
using CMSUI.Requesters;
using CMSUI.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace CMSUI.EvaluationWindows
{
    /// <summary>
    /// Interaction logic for FixStudentsDataWindow.xaml
    /// </summary>
    public partial class FixStudentsDataWindow
    {
        Evaluate Evaluator;
        IFixStudentsDataWindowRequester CallingWindow;
        bool IsNonCloseButtonClicked = false;
        public FixStudentsDataWindow(IFixStudentsDataWindowRequester caller, Evaluate ev)
        {
            InitializeComponent();
            CallingWindow = caller;
            Evaluator = ev;
            GetStudentsAnswers();
        }


        List<StudentDataModel> data = new List<StudentDataModel>();

        public void GetStudentsAnswers()
        {
            int i = 1;
            foreach (StudentAnswersModel ans in Evaluator.StudentsAnswersWithErrors)
            {
                StudentDataUserControl sd = new StudentDataUserControl();
                sd.number.Text = i.ToString();
                sd.lastName.Text = NamesFixer(ans.Student.LastName);
                sd.regNo.Text = ans.Student.RegNo.ToString();
                sd.firstName.Text = NamesFixer(ans.Student.FirstName);
                sd.errorType.Visibility = Visibility.Visible;
                sd.errorTypeText.Text = ans.ErrorType;
                sd.group.Visibility = Visibility.Visible;
                sd.group.Text = ans.Group.Name;
                sd.deleteStudentData.Visibility = Visibility.Collapsed;
                students.Children.Add(sd);
                i++;
            }
        }

        private string NamesFixer(string name)
        {
            string t = name;
            t = t.Replace("  ", "");
            if (t.Count() > 0)
            {
                if (t.Last() == ' ')
                {
                    t = t.Remove(t.Length - 1);
                }
            }
            return t;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int i = 0;
            foreach (StudentDataUserControl item in students.Children)
            {
                Evaluator.StudentsAnswersWithErrors[i].Student.RegNo = int.Parse(item.regNo.Text);
                Evaluator.StudentsAnswersWithErrors[i].Student.FirstName = item.firstName.Text;
                Evaluator.StudentsAnswersWithErrors[i].Student.LastName = item.lastName.Text;
                Evaluator.StudentsAnswersWithErrors[i].Group.Name = item.group.Text;
                i++;
            }
            Evaluator.FixingErrors();

            IsNonCloseButtonClicked = true;
            this.Close();
        }

        private void MetroWindow_Closed(object sender, EventArgs e)
        {
            if (!IsNonCloseButtonClicked)
            {
                return;
            }
            else
            {
                if (Evaluator.StudentsAnswersWithErrors.Count > 0)
                {
                    CallingWindow.FixComplete(false);
                }
                else CallingWindow.FixComplete(true);
            }

        }
    }
}
