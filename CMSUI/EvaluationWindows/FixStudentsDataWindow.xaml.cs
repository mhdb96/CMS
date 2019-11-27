using CMSLibrary;
using CMSLibrary.Evaluation;
using CMSLibrary.Models;
using CMSUI.EvaluationWindows;
using CMSUI.Requesters;
using CMSUI.UserControls;
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
    /// Interaction logic for FixStudentsDataWindow.xaml
    /// </summary>
    public partial class FixStudentsDataWindow
    {
        Evaluate Evaluator;
        IFixStudentsDataWindowRequester CallingWindow;
        public FixStudentsDataWindow(IFixStudentsDataWindowRequester caller, Evaluate ev)
        {
            InitializeComponent();
            CallingWindow = caller;
            Evaluator = ev;
            GetStudentsAnswers();
        }


        List<StudentDataModel> data = new List<StudentDataModel>();
        private CreateExamWindow createExamWindow;
        private Evaluate evaluator;

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
                Evaluator.StudentsAnswersWithErrors[i].Student.RegNo = Int32.Parse(item.regNo.Text);
                Evaluator.StudentsAnswersWithErrors[i].Student.FirstName = item.firstName.Text;
                Evaluator.StudentsAnswersWithErrors[i].Student.LastName = item.lastName.Text;
                i++;
            }
            Evaluator.FixingErrors();

            
            this.Close();
        }

        private void MetroWindow_Closed(object sender, EventArgs e)
        {
            if (Evaluator.StudentsAnswersWithErrors.Count > 0)
            {
                CallingWindow.FixComplete(false);
            }
            else CallingWindow.FixComplete(true);
        }
    }
}
