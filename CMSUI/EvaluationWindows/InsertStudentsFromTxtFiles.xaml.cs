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
using CMSLibrary.Models;
using CMSLibrary.Evaluation;

namespace CMSUI.EvaluationWindows
{
    /// <summary>
    /// Interaction logic for InsertStudentsFromTxtFiles.xaml
    /// </summary>
    public partial class InsertStudentsFromTxtFiles
    {
        List<DepartmentModel> Departments;
        List<EducationalYearModel> EduYears;
        string StudentListPath;
        string[] results;        
        
        List<StudentDataModel> data = new List<StudentDataModel>();
        public InsertStudentsFromTxtFiles()
        {
            InitializeComponent();
            Departments = GlobalConfig.Connection.GetDepartment_All();
            departmentsCombobox.ItemsSource = Departments;
            EduYears = GlobalConfig.Connection.GetEducationalYear_ALL();
            eduYearsCombobox.ItemsSource = EduYears;
        }

        public string GetFilePath()
        {
            OpenFileDialog file = new OpenFileDialog();
            file.Filter = ".txt Dosyası |*.txt";
            file.ShowDialog();
            string path = file.FileName;
            return path;
        }

        private void GetStudentsData()
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
            
            
        }

        private void ChooseStudentsFile_Click(object sender, RoutedEventArgs e)
        {
            GetStudentsData();
        }

        private void InsertStudents_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<StudentModel> studentModels = new List<StudentModel>();
                foreach (StudentDataUserControl student in students.Children)
                {
                    StudentDataModel model = new StudentDataModel();
                    model.FirstName = student.firstName.Text;
                    model.LastName = student.lastName.Text;
                    model.RegNo = Int32.Parse(student.regNo.Text);
                    StudentModel s = new StudentModel
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        RegNo = model.RegNo,
                        Department = (DepartmentModel)departmentsCombobox.SelectedItem,
                        EduYear = (EducationalYearModel)eduYearsCombobox.SelectedItem
                    };
                    studentModels.Add(s);
                }

                StringBuilder sb = new StringBuilder();
                foreach (StudentModel s in studentModels)
                {
                    string err = GlobalConfig.Connection.CreateStudent(s);
                    if(err != null)
                    {
                        sb.Append(err);
                    }
                }
                string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                File.AppendAllText(path + "\\log.txt", sb.ToString());
                sb.Clear();

            }
            catch (Exception er)
            {
                System.Windows.MessageBox.Show(er.Message);
            }
        }
    }
}
