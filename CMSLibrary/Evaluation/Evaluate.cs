using CMSLibrary.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CMSLibrary.Evaluation
{
    public class Evaluate
    {
        public List<StudentAnswersModel> StudentsAnswers = new List<StudentAnswersModel>();
        public List<StudentAnswersModel> StudentsAnswersWithErrors = new List<StudentAnswersModel>();

        public List<AnswerKeyModel> AnswerKeys = new List<AnswerKeyModel>();
        public string StudentListPath = "";
        public string AnswersKeyPath = "";
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
            foreach (StudentAnswersModel ans in StudentsAnswersWithErrors)
            {
                bool isDuplicate = false;
                bool hasGroup = true;
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
                if (ans.Group.Name == "" || ans.Group.Name == " ")
                {
                    hasGroup = false;
                    model = null;
                }
                if (model == null)
                {
                    if (isDuplicate)
                    {
                        ans.ErrorType = "Duplicated Value, Fix RegNo";
                    }
                    else if (!hasGroup)
                    {
                        ans.ErrorType = "No Group";
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
                    foreach (AnswerKeyModel answerKey in AnswerKeys)
                    {
                        if (answerKey.Group.Name == ans.Group.Name)
                        {
                            ans.AnswersList = ans.AnswersList.Substring(0, answerKey.QuestionCount);
                            break;
                        }
                    }
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
                if (line == "")
                {
                    continue;
                }
                StudentAnswersModel studentAnswers = new StudentAnswersModel();
                StudentModel model;
                int regNo = 0;
                bool isDuplicate = false;
                bool isRightName = true;
                if (int.TryParse(listString.Substring(24, 9), out regNo))
                {
                    model = GlobalConfig.Connection.GetStudent_ByRegNo(regNo);

                    if (model != null)
                    {
                        if (model.FirstName != NamesFixer(listString.Substring(0, 12)))
                        {
                            isRightName = false;
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
                    if (isDuplicate)
                    {
                        studentAnswers.ErrorType = "Duplicated Value, Fix RegNo";
                    }
                    else if (!isRightName)
                    {
                        studentAnswers.ErrorType = "Wrong Name, Check Student List";
                    }
                    else
                    {
                        studentAnswers.ErrorType = "Student not found in DB";
                    }

                    studentAnswers.Student.FirstName = listString.Substring(0, 12);
                    studentAnswers.Student.LastName = listString.Substring(12, 12);
                    studentAnswers.Student.RegNo = regNo;
                    try
                    {
                        StudentDataModel t = new StudentDataModel
                        {
                            Group = listString.Substring(33, 1)
                        };
                        studentAnswers.Group.Name = listString.Substring(33, 1);
                        foreach (AnswerKeyModel answerKey in AnswerKeys)
                        {
                            if (answerKey.Group.Name == studentAnswers.Group.Name)
                            {
                                studentAnswers.AnswersList = listString.Substring(34, answerKey.QuestionCount);
                                break;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        studentAnswers.AnswersList = listString.Substring(34, listString.Length - 34);
                        studentAnswers.ErrorType = "No Group";
                    }
                    StudentsAnswersWithErrors.Add(studentAnswers);
                }
                else
                {
                    try
                    {
                        studentAnswers.Student = model;
                        StudentDataModel t = new StudentDataModel
                        {
                            Group = listString.Substring(33, 1)
                        };
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
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        studentAnswers.AnswersList = listString.Substring(34, listString.Length - 34);
                        studentAnswers.ErrorType = "No Group";
                        StudentsAnswersWithErrors.Add(studentAnswers);
                    }

                }

            }
            if (StudentsAnswersWithErrors.Count > 0)
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
