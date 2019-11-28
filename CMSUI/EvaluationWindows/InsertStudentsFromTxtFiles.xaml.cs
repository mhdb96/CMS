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
using MahApps.Metro.Controls.Dialogs;

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
        List<StudentModel> StudentsDataWithErrors = new List<StudentModel>();
        List<StudentModel> StudentModels = new List<StudentModel>();
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
            studentsList.Children.Clear();
            foreach (string listString in results)
            {                              
                if(listString.Replace(" ","") == "")
                {
                    continue;
                }
                StudentDataUserControl sd = new StudentDataUserControl();    
                sd.number.Text = i.ToString();
                sd.lastName.Text = NamesFixer(listString.Substring(12, 12));
                sd.regNo.Text = listString.Substring(24, 9);
                sd.firstName.Text = NamesFixer(listString.Substring(0, 12));
                studentsList.Children.Add(sd);
                i++;
            }
        }

        private void FixStudentsData()
        {
            studentsList.Children.Clear();
            int i = 1;
            foreach (StudentModel student in StudentsDataWithErrors)
            {
                StudentDataUserControl sd = new StudentDataUserControl();
                sd.number.Text = i.ToString();
                sd.lastName.Text = student.LastName;
                sd.regNo.Text = student.RegNo.ToString();
                sd.firstName.Text = student.FirstName;
                studentsList.Children.Add(sd);
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
                if(t.First() == ' ')
                {
                    t = t.Remove(0, 1);
                }
            }            
            return t;
        }

        private void DeleteStudentBtn_Click(object sender, RoutedEventArgs e)
        {

        }
        

        private void ChooseStudentsFile_Click(object sender, RoutedEventArgs e)
        {
            GetStudentsData();
        }

        private async void InsertStudents_Click(object sender, RoutedEventArgs e)
        {
            string studentNo = "";
            if(departmentsCombobox.SelectedItem == null || eduYearsCombobox.SelectedItem == null)
            {
                return;
            }
            try
            {                
                bool isDuplicate = false;
                StudentsDataWithErrors.Clear();
                foreach (StudentDataUserControl student in studentsList.Children)
                {
                    studentNo = student.number.Text;
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
                    var duplicate = StudentModels.Find(st => st.RegNo == s.RegNo);
                    if (duplicate != null)
                    {
                        isDuplicate = true;
                        duplicate.ErrorType = "Duplicated Value, Fix RegNo";
                        StudentsDataWithErrors.Add(duplicate);
                        StudentsDataWithErrors.Add(s);
                        StudentModels.Remove(duplicate);
                        continue;
                    }                    
                    StudentModels.Add(s);
                }
                if(isDuplicate)
                {
                    FixStudentsData();
                    return;
                }

                bool hasErrors = false;
                int errorsCount = 0;
                StringBuilder sb = new StringBuilder();
                sb.Append($"At {DateTime.Now.ToString()} new error log was created{Environment.NewLine}");
                foreach (StudentModel s in StudentModels)
                {
                    string err = GlobalConfig.Connection.CreateStudent(s);
                    if(err != null)
                    {
                        hasErrors = true;
                        errorsCount++;
                        sb.Append(err);
                    }
                }
                if(hasErrors)
                {                    
                    sb.Append($"==================================================================={Environment.NewLine}");
                    string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    File.AppendAllText(path + "\\log.txt", sb.ToString());
                    sb.Clear();
                    await this.ShowMessageAsync("Error in the Creating Porccess", $"The creating proccess finished with {errorsCount} errors{Environment.NewLine}" +
                        $"Please check Log file on the desktop for more information", MessageDialogStyle.Affirmative, null);
                }                
                this.Close();
            }
            catch (Exception er)
            {
                error.Visibility = Visibility.Visible;
                errorText.Text = $"Error at {studentNo} line: {er.Message}";
            }
        }
    }
}
