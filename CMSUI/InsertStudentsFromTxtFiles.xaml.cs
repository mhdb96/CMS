using CMSLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CMSUI.UserControls;

namespace CMSUI
{
    /// <summary>
    /// Interaction logic for InsertStudentsFromTxtFiles.xaml
    /// </summary>
    public partial class InsertStudentsFromTxtFiles
    {        
        string StudentListPath;
        string AnswersKeyPath;
        string[] answerKeys;
        string[] results;        
        
        List<StudentDataModel> data = new List<StudentDataModel>();
        public InsertStudentsFromTxtFiles()
        {
            InitializeComponent();            
        }

        public string GetFilePath()
        {
            OpenFileDialog file = new OpenFileDialog();
            file.Filter = ".txt Dosyası |*.txt";
            file.ShowDialog();
            string path = file.FileName;
            return path;
        }

        private void GetStudentsAnswers()
        {
            string studentsAnswersListPath = GetFilePath();
            if (studentsAnswersListPath == "")
            {
                return;
            }
            StudentListPath = studentsAnswersListPath;
            results = File.ReadAllLines(StudentListPath, Encoding.GetEncoding("iso-8859-9"));
            int i = 1;
            foreach (string listString in results)
            {                                   
                StudentDataUserControl sd = new StudentDataUserControl();    
                sd.number.Text = i.ToString();
                sd.lastName.Text = NamesFixer(listString.Substring(12, 12));
                sd.regNo.Text = listString.Substring(24, 9);
                sd.firstName.Text = NamesFixer(listString.Substring(0, 12));
                students.Children.Add(sd);
                i++;
            }
        }

        private string NamesFixer (string name)
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

        private void DeleteStudentBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GetStudentsAnswers();   
        }
    }
}
