using CMSLibrary.Models;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CMSLibrary.Evaluation
{
    public class WriteToExcel
    {
        List<StudentMarksModel> StudentMark = new List<StudentMarksModel>();
        Excel excel;
        ExamModel Exam;
        int[] startingLines = new int[] { 0, 0, 0 };

        public WriteToExcel(ExamModel model)
        {
            Exam = model;
            Exam.Assignment.Course.CourseOutcomes = GlobalConfig.Connection.GetCourseOutcome_GetByExamId(Exam.Id);
            WriteDataToExcelSheet();
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = ".xlsx files (*.xlsx)|*.xlsx";
            saveFile.FileName = $"{model.Assignment.Course.Name}-{model.ExamType.Name}-{model.Assignment.ActiveTerm.Name}";
            DialogResult r = saveFile.ShowDialog();
            if (r == DialogResult.Abort || r == DialogResult.Cancel)
            {
            }
            else
            {
                excel.SaveAs(saveFile.FileName);
                model.FilePath = saveFile.FileName;
            }
        }
        private void WriteDataToExcelSheet()
        {
            GlobalConfig.Connection.GetExamGroup_ByExamId(Exam);
            int questionsCount = 0;
            int studentsCount = 0;
            foreach (ExamGroupModel examGroup in Exam.ExamGroups)
            {
                List<StudentModel> students = GlobalConfig.Connection.GetStudent_GetByExamGroupId(examGroup.Id);
                studentsCount += students.Count;
                List<QuestionModel> questions = GlobalConfig.Connection.GetQuestions_GetByExamGroupId(examGroup.Id);
                questionsCount += questions.Count;
            }
            excel = new Excel(studentsCount, questionsCount, Exam.ExamGroups.Count * Exam.Assignment.Course.CourseOutcomes.Count);
            foreach (ExamGroupModel examGroup in Exam.ExamGroups)
            {
                List<StudentModel> students = GlobalConfig.Connection.GetStudent_GetByExamGroupId(examGroup.Id);
                if (students.Count == 0)
                {
                    continue;
                }
                examGroup.Questions = GlobalConfig.Connection.GetQuestions_GetByExamGroupId(examGroup.Id);
                startingLines[1] = WriteQuestionsData(examGroup, startingLines[1]);
                startingLines[2] = WriteOutcomesData(examGroup, students.Count, startingLines[2]);
                startingLines[0] = WriteStudentsData(students, examGroup, startingLines[0], startingLines[1]);
            }
        }

        private int WriteOutcomesData(ExamGroupModel examGroup, int studentsCount, int startingLine)
        {
            int p = startingLine;
            excel.WriteToCell(p, 0, $"Group {examGroup.Group.Name}", 2);
            p++;
            excel.WriteToCell(p, 0, "Outcome's Name", 2);
            excel.WriteToCell(p, 1, "Average (Mark)", 2);
            excel.WriteToCell(p, 2, "Success(%)", 2);
            excel.WriteToCell(p, 3, "Outcome's Questions List", 2);
            excel.WriteToCell(p, 4, "Outcome's Description", 2);
            p++;
            foreach (var courseOutcome in Exam.Assignment.Course.CourseOutcomes)
            {
                string questionsList = "";
                decimal toplam = 0;
                decimal qtoplam = 0;
                List<QuestionModel> questions;
                questions = GlobalConfig.Connection.GetQuestion_GetByCourseOutcomesIdAndExamGroupsId(examGroup.Id, courseOutcome.Id);
                foreach (var question in questions)
                {
                    List<ResultModel> results;
                    results = GlobalConfig.Connection.GetResults_GetByQuestionId(question.Id);
                    foreach (var result in results)
                    {
                        if (result.IsTrue)
                        {
                            toplam += question.Mark;
                        }
                    }
                    qtoplam += question.Mark;
                    questionsList += $"{question.Name}, ";
                }
                excel.WriteToCell(p, 0, courseOutcome.Name, 2);
                excel.WriteToCell(p, 4, courseOutcome.Description, 2);

                if (qtoplam == 0)
                {
                    excel.WriteToCell(p, 1, "0", 2);
                    excel.WriteToCell(p, 2, "0", 2);
                }
                else
                {
                    excel.WriteToCell(p, 1, (toplam / studentsCount / questions.Count).ToString("0.##"), 2);
                    excel.WriteToCell(p, 2, (toplam / studentsCount / qtoplam * 100).ToString("0.##"), 2);
                    excel.WriteToCell(p, 3, questionsList, 2);
                }
                p++;
            }
            p++;
            return p;
        }

        private int WriteQuestionsData(ExamGroupModel examGroup, int startingLine)
        {
            int k = startingLine;
            excel.WriteToCell(k, 0, $"Group {examGroup.Group.Name}", 1);
            k++;
            excel.WriteToCell(k, 0, "Question #", 1);
            excel.WriteToCell(k, 1, "Average (Mark)", 1);
            excel.WriteToCell(k, 2, "Success (%)", 1);
            k++;
            return k;
        }

        private int WriteAVGsToQuestions(QuestionModel question, int studentsCount, decimal questionAVG, int startingLine)
        {
            int k = startingLine;
            excel.WriteToCell(k, 0, (question.Name).ToString(), 1);
            excel.WriteToCell(k, 1, (questionAVG / studentsCount).ToString("0.##"), 1);
            excel.WriteToCell(k, 2, (questionAVG / studentsCount / question.Mark * 100).ToString("0.##"), 1);
            k++;
            return k;
        }

        private int WriteStudentsData(List<StudentModel> students, ExamGroupModel examGroup, int startingLine, int questionStartingLine)
        {
            int i = startingLine, j = 0;
            excel.WriteToCell(i, 0, $"Group {examGroup.Group.Name}", 0);
            i++;
            excel.WriteToCell(i, 0, "Student Reg No", 0);

            excel.WriteToCell(i, 1, "Full Name", 0);
            j = 2;

            foreach (var question in examGroup.Questions)
            {
                excel.WriteToCell(i, j, $"Question {question.Name}", 0);
                j++;
            }
            excel.WriteToCell(i, j, "Sum", 0);
            i++;
            decimal[,] studentMarks = new decimal[students.Count, examGroup.Questions.Count];
            int m = 0, n;
            foreach (StudentModel student in students)
            {
                j = 0;
                n = 0;
                excel.WriteToCell(i, j, student.RegNo.ToString(), 0);
                j++;
                excel.WriteToCell(i, j, student.FullName, 0);
                j++;
                decimal markSum = 0;
                foreach (QuestionModel question in examGroup.Questions)
                {
                    ResultModel result = GlobalConfig.Connection.GetResults_GetByStudentIdAndQuestionId(student.Id, question.Id);
                    if (result.IsTrue)
                    {
                        markSum += question.Mark;
                        excel.WriteToCell(i, j, question.Mark.ToString(), 0);
                        studentMarks[m, n] = question.Mark;
                    }
                    else
                    {
                        excel.WriteToCell(i, j, "0", 0);
                        studentMarks[m, n] = 0;
                    }
                    j++;
                    n++;
                }
                excel.WriteToCell(i, j, markSum.ToString(), 0);
                m++;
                i++;
            }
            j = 0;
            excel.WriteToCell(i, j, "Average", 0);
            j += 2;
            n = 0;
            foreach (QuestionModel question in examGroup.Questions)
            {
                decimal questionAVG = 0;
                for (m = 0; m < students.Count; m++)
                {
                    questionAVG += studentMarks[m, n];
                }
                excel.WriteToCell(i, j, (questionAVG / students.Count).ToString("0.##"), 0);
                questionStartingLine = WriteAVGsToQuestions(question, students.Count, questionAVG, questionStartingLine);
                j++;
                n++;
            }
            j = 0;
            i += 2;
            startingLines[1] = questionStartingLine + 1;
            return i;
        }
    }
}





