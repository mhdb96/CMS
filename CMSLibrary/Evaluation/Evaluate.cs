using CMSLibrary.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMSLibrary.Evaluation
{
    public class Evaluate
    {
        public List<StudentAnswersModel> StudentsAnswers = new List<StudentAnswersModel>();
        public List<StudentAnswersModel> StudentsAnswersWithErrors = new List<StudentAnswersModel>();

        public List<AnswerKeyModel> AnswerKeys = new List<AnswerKeyModel>();
        string StudentListPath;
        string AnswersKeyPath;
        string[] answerKeys;
        string[] results;
        public List<AnswerKeyModel> GetAnswersKeys(string answerPath)
        {
            AnswersKeyPath = answerPath;
            answerKeys = File.ReadAllLines(AnswersKeyPath, Encoding.GetEncoding("iso-8859-9"));
            foreach (string answersString in answerKeys)
            {
                AnswerKeyModel a = new AnswerKeyModel();
                a.Group.Name = answersString.Substring(0, 1);
                a.QuestionCount = answersString.Length - 1;
                a.AnswersList = answersString.Substring(1, a.QuestionCount);
                AnswerKeys.Add(a);
            }
            return AnswerKeys;
        }

        public void FixingErrors()
        {
            List<StudentAnswersModel> err = new List<StudentAnswersModel>();
            foreach(StudentAnswersModel ans in StudentsAnswersWithErrors)
            {
                bool isDuplicate = false;
                StudentModel model = GlobalConfig.Connection.GetStudent_ByRegNo(ans.Student.RegNo);
                var duplicate = StudentsAnswers.Find(s => s.Student.RegNo == model.RegNo);
                if (duplicate != null)
                {
                    model = null;
                    isDuplicate = true;
                    duplicate.ErrorType = "Duplicated Value, Fix RegNo";
                    err.Add(duplicate);
                    StudentsAnswers.Remove(duplicate);
                }
                if (model == null)
                {
                    if (isDuplicate)
                    {
                        ans.ErrorType = "Duplicated Value, Fix RegNo";
                    }
                    else
                    {
                        ans.ErrorType = "Student not found in DB";
                    }
                    err.Add(ans);
                }
                else
                {
                    ans.Student = model;
                    StudentsAnswers.Add(ans);
                }
            }
            StudentsAnswersWithErrors = err;
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

        public List<StudentAnswersModel> GetStudentsAnswers(string studentListPath)
        {
            StudentsAnswersWithErrors.Clear();
            StudentListPath = studentListPath;
            results = File.ReadAllLines(StudentListPath, Encoding.GetEncoding("iso-8859-9"));
            foreach (string listString in results)
            {
                string line = listString.Replace(" ", "");
                if(line == "")
                {
                    continue;
                }
                StudentAnswersModel studentAnswers = new StudentAnswersModel();
                StudentModel model;
                int regNo = 0;
                bool isDuplicate = false;
                if (Int32.TryParse(listString.Substring(24, 9), out regNo))
                {
                    model = GlobalConfig.Connection.GetStudent_ByRegNo(regNo);

                    if(model != null)
                    {
                        if (model.FirstName != NamesFixer(listString.Substring(0, 12)))
                        {
                            model = null;
                        }
                    }
                    

                    if (model != null)
                    {                        
                        var duplicate = StudentsAnswers.Find(s => s.Student.RegNo == model.RegNo);
                        if (duplicate != null)
                        {
                            model = null;
                            isDuplicate = true;
                            duplicate.ErrorType = "Duplicated Value, Fix RegNo";
                            StudentsAnswersWithErrors.Add(duplicate);
                            StudentsAnswers.Remove(duplicate);
                        }
                    }                    
                }
                else
                {
                    model = null;
                }
                if (model == null)
                {
                    if(isDuplicate)
                    {
                        studentAnswers.ErrorType = "Duplicated Value, Fix RegNo";
                    }
                    else
                    {
                        studentAnswers.ErrorType = "Student not found in DB";
                    }
                    
                    studentAnswers.Student.FirstName = listString.Substring(0, 12);
                    studentAnswers.Student.LastName = listString.Substring(12, 12);
                    studentAnswers.Student.RegNo = regNo;
                    studentAnswers.Group.Name = listString.Substring(33, 1);
                    foreach (AnswerKeyModel answerKey in AnswerKeys)
                    {
                        if (answerKey.Group.Name == studentAnswers.Group.Name)
                        {
                            studentAnswers.AnswersList = listString.Substring(34, answerKey.QuestionCount);
                            break;
                        }
                    }
                    StudentsAnswersWithErrors.Add(studentAnswers);
                }
                else
                {
                    studentAnswers.Student = model;
                    //studentAnswers.Student.FirstName = listString.Substring(0, 12);
                    //studentAnswers.Student.LastName = listString.Substring(12, 12);
                    //studentAnswers.Student.RegNo = Int32.Parse(listString.Substring(24, 9));
                    studentAnswers.Group.Name = listString.Substring(33, 1);
                    foreach (AnswerKeyModel answerKey in AnswerKeys)
                    {
                        if (answerKey.Group.Name == studentAnswers.Group.Name)
                        {
                            studentAnswers.AnswersList = listString.Substring(34, answerKey.QuestionCount);
                            break;
                        }
                    }
                    StudentsAnswers.Add(studentAnswers);
                }
                
            }
            if(StudentsAnswersWithErrors.Count > 0)
            {
                return null;
            }
            return StudentsAnswers;
        }
        public List<StudentAnswersModel> GetRightAnswers()
        {
            int correct;
            int counter;
            foreach (StudentAnswersModel studentAnswers in StudentsAnswers)
            {
                correct = 0;
                counter = 0;
                foreach (AnswerKeyModel answerKey in AnswerKeys)
                {
                    if (studentAnswers.Group.Name == answerKey.Group.Name)
                    {
                        while (counter != answerKey.QuestionCount)
                        {
                            if (studentAnswers.AnswersList[counter].ToString() == answerKey.AnswersList.Substring(counter, 1))
                            {
                                correct++;
                            }
                            counter++;
                        }
                        break;
                    }
                }
                studentAnswers.CorrectAnswersCount = correct;
            }
            return StudentsAnswers;
        }
    }
}
