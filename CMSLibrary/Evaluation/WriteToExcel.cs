using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CMSLibrary;
using CMSLibrary.Evaluation;
using CMSLibrary.Models;

namespace CMSLibrary.Evaluation
{
    public class WriteToExcel
    {
        List<StudentMarksModel> StudentMark = new List<StudentMarksModel>();
        //List<CourseOutcomeModel> courseOutcomes;

        Excel excel = new Excel();
        ExamModel Exam;
        int[] startingLines = new int[] { 0, 0, 0 };

        public WriteToExcel()
        {
            Exam = GlobalConfig.Connection.GetExam_ById(20);
            Exam.Assignment.Course.CourseOutcomes = GlobalConfig.Connection.GetCourseOutcome_GetByExamId(Exam.Id);
            excel.CreateNewFile();
            excel.CreateNewSheet();
            excel.CreateNewSheet();
            //WriteDataToExcelSheet1(Exam);
            WriteDataToExcelSheet();
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.ShowDialog();
            excel.SaveAs(saveFile.FileName);
            excel.Close();
        }
        public WriteToExcel(ExamModel model)
        {
            Exam = model;
            Exam.Assignment.Course.CourseOutcomes = GlobalConfig.Connection.GetCourseOutcome_GetByExamId(Exam.Id);
            excel.CreateNewFile();
            excel.CreateNewSheet();
            excel.CreateNewSheet();
            WriteDataToExcelSheet();      
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.ShowDialog();
            excel.SaveAs(saveFile.FileName);
            excel.Close();
            model.FilePath = saveFile.FileName;
        }
        private void WriteDataToExcelSheet()
        {
            GlobalConfig.Connection.GetExamGroup_ByExamId(Exam);
            foreach (ExamGroupModel examGroup in Exam.ExamGroups)
            {
                List<StudentModel> students = GlobalConfig.Connection.GetStudent_GetByExamGroupId(examGroup.Id);
                if (students.Count == 0)
                {
                    continue;
                }
                examGroup.Questions = GlobalConfig.Connection.GetQuestions_GetByExamGroupId(examGroup.Id);

                startingLines[1] = WriteQuestionsData(examGroup, startingLines[1]);
                startingLines[2] = WriteOutcomesData(examGroup, students.Count,startingLines[2]);
                startingLines[0] = WriteStudentsData(students, examGroup, startingLines[0], startingLines[1]);
            }
        }

        private int WriteOutcomesData(ExamGroupModel examGroup, int studentsCount, int startingLine)
        {
            excel.SelectWorkSheet(3);
            int p = startingLine;
            excel.WriteToCell(p, 0, examGroup.Group.Name);
            p++;
            excel.WriteToCell(p, 0, "Kazanım Adı");
            excel.WriteToCell(p, 1, "Ortalaması (Puan)");
            excel.WriteToCell(p, 2, "Başarımı(%)");
            p++;
            foreach (var courseOutcome in Exam.Assignment.Course.CourseOutcomes)
            {
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
                }
                excel.WriteToCell(p, 0, courseOutcome.Name);
                excel.WriteToCell(p, 1, (toplam / studentsCount).ToString());
                excel.WriteToCell(p, 2, (((toplam / studentsCount) / qtoplam) * 100).ToString());
                p++;
            }
            return p;
        }
        
        private int WriteQuestionsData(ExamGroupModel examGroup, int startingLine)
        {
            excel.SelectWorkSheet(2);
            int k = startingLine;
            excel.WriteToCell(k, 0, examGroup.Group.Name);
            k++;
            excel.WriteToCell(k, 0, "Soru Numarası");
            excel.WriteToCell(k, 1, "Ortalaması (Puan)");            
            excel.WriteToCell(k, 2, "Başarımı(%)");
            k++;            
            return k;
        }

        private int WriteAVGsToQuestions(QuestionModel question, int studentsCount, decimal questionAVG, int startingLine)
        {
            excel.SelectWorkSheet(2);
            int k = startingLine;                        
            excel.WriteToCell(k, 0, (question.Name).ToString());
            excel.WriteToCell(k, 1, (questionAVG / studentsCount).ToString());
            excel.WriteToCell(k, 2, "%" + (((questionAVG / studentsCount) / question.Mark) * 100).ToString());
            k++;
            return k;
        }

        private int WriteStudentsData(List<StudentModel> students, ExamGroupModel examGroup, int startingLine, int questionStartingLine)
        {
            excel.SelectWorkSheet(1);
            int i = startingLine, j=0;            
            excel.WriteToCell(i, 0, examGroup.Group.Name);
            i++;
            excel.WriteToCell(i, 0, "Student Reg No");

            excel.WriteToCell(i, 1, "Full Name");
            j=2;

            foreach (var question in examGroup.Questions)
            {
                excel.WriteToCell(i, j, $"Question {question.Name}");
                j++;
            }
            excel.WriteToCell(i, j, "Sum");
            i++;
            decimal[,] studentMarks = new decimal[students.Count, examGroup.Questions.Count];
            int m = 0, n;
            foreach (StudentModel student in students)
            {
                j = 0;
                n = 0;
                excel.WriteToCell(i, j, student.RegNo.ToString());
                j++;
                excel.WriteToCell(i, j, student.FullName);
                j++;
                decimal markSum = 0;
                foreach (QuestionModel question in examGroup.Questions)
                {
                    ResultModel result = GlobalConfig.Connection.GetResults_GetByStudentIdAndQuestionId(student.Id, question.Id);
                    if (result.IsTrue)
                    {
                        markSum += question.Mark;
                        excel.WriteToCell(i, j, question.Mark.ToString());
                        studentMarks[m, n] = question.Mark;
                    }
                    else
                    {
                        excel.WriteToCell(i, j, "0");
                        studentMarks[m, n] = 0;
                    }
                    j++;
                    n++;
                }                
                excel.WriteToCell(i, j, markSum.ToString());
                m++;
                i++;
            }
            j = 0;
            excel.WriteToCell(i, j, "Average");
            j += 2;
            n = 0;
            foreach (QuestionModel question in examGroup.Questions)
            {
                decimal questionAVG = 0;
                for (m = 0; m < students.Count; m++)
                {
                    questionAVG += studentMarks[m, n];
                }                
                excel.WriteToCell(i, j, (questionAVG / students.Count).ToString());
                questionStartingLine = WriteAVGsToQuestions(question, students.Count, questionAVG, questionStartingLine);
                excel.SelectWorkSheet(1);
                j++;
                n++;
            }
            j = 0;
            i+=2;
            startingLines[1] = questionStartingLine;
            return i;
        }

        //private void WriteDataToExcelSheet1(ExamModel exam)
        //{
        //    int i = 0, j = 0;
        //    int k = 0, l = 0;
        //    int p = 0, r = 0;

        //    GlobalConfig.Connection.GetExamGroup_ByExamId(exam);
        //    foreach (ExamGroupModel examGroup in exam.ExamGroups)
        //    {
        //        //sheet3
        //        List<StudentModel> students = GlobalConfig.Connection.GetStudent_GetByExamGroupId(examGroup.Id);
        //        if (students.Count == 0)
        //        {
        //            continue;
        //        }

        //        //courseOutcomes = GlobalConfig.Connection.GetQuestionOutcomes_GetByCourseIdAndExamGroupsId(examGroup.Id, exam.Assignment.Course.Id);
        //        courseOutcomes = exam.Assignment.Course.CourseOutcomes;

        //        excel.SelectWorkSheet(3);

        //        excel.WriteToCell(p, r, examGroup.Group.Name);
        //        p++;
        //        excel.WriteToCell(p, r, "Kazanım Adı");
        //        r++;
        //        excel.WriteToCell(p, r, "Ortalaması (Puan)");
        //        r++;
        //        excel.WriteToCell(p, r, "Başarımı(%)");
        //        p++;
        //        r = 0;

        //        foreach (var courseOutcome in courseOutcomes)
        //        {
        //            decimal toplam = 0;
        //            decimal qtoplam = 0;
        //            List<QuestionModel> questions;
        //            questions = GlobalConfig.Connection.GetQuestion_GetByCourseOutcomesIdAndExamGroupsId(examGroup.Id, courseOutcome.Id);
        //            foreach (var question in questions)
        //            {
        //                List<ResultModel> results;
        //                results = GlobalConfig.Connection.GetResults_GetByQuestionId(question.Id);
        //                foreach (var result in results)
        //                {
        //                    if (result.IsTrue)
        //                    {
        //                        toplam += question.Mark;
        //                    }
        //                }
        //                qtoplam += question.Mark;
        //            }
        //            excel.WriteToCell(p, r, courseOutcome.Name);
        //            r++;
        //            excel.WriteToCell(p, r, (toplam / students.Count).ToString());
        //            r++;
        //            excel.WriteToCell(p, r, (((toplam / students.Count) / qtoplam) * 100).ToString());
        //            p++;
        //            r = 0;
        //        }


        //        //sheet3

        //        excel.SelectWorkSheet(1);

        //        examGroup.Questions = GlobalConfig.Connection.GetQuestions_GetByExamGroupId(examGroup.Id);


        //        excel.WriteToCell(i, j, examGroup.Group.Name);
        //        i++;
        //        excel.WriteToCell(i, j, "Student Reg No");
        //        j++;
        //        excel.WriteToCell(i, j, "Full Name");
        //        j++;

        //        foreach (var question in examGroup.Questions)
        //        {
        //            excel.WriteToCell(i, j, $"Soru {question.Name}");
        //            j++;
        //        }
        //        excel.WriteToCell(i, j, "Sum");

        //        //sheet 2
        //        excel.SelectWorkSheet(2);

        //        excel.WriteToCell(k, l, examGroup.Group.Name);
        //        k++;
        //        excel.WriteToCell(k, l, "Soru Numarası");
        //        l++;
        //        excel.WriteToCell(k, l, "Ortalaması (Puan)");
        //        l++;
        //        excel.WriteToCell(k, l, "Başarımı(%)");
        //        k++;
        //        l = 0;
        //        excel.SelectWorkSheet(1);

        //        //sheet 2

        //        //List<StudentModel> students = GlobalConfig.Connection.GetStudent_GetByExamGroupId(examGroup.Id);
        //        i++;
        //        decimal[,] studentMarks = new decimal[students.Count, examGroup.Questions.Count];
        //        int m = 0, n = 0;
        //        foreach (StudentModel student in students)
        //        {
        //            j = 0;
        //            n = 0;
        //            excel.WriteToCell(i, j, student.RegNo.ToString());
        //            j++;
        //            excel.WriteToCell(i, j, student.FullName);
        //            j++;
        //            decimal markSum = 0;
        //            foreach (QuestionModel question in examGroup.Questions)
        //            {


        //                ResultModel result = GlobalConfig.Connection.GetResults_GetByStudentIdAndQuestionId(student.Id, question.Id);
        //                if (result.IsTrue)
        //                {
        //                    markSum += question.Mark;
        //                    excel.WriteToCell(i, j, question.Mark.ToString());
        //                    studentMarks[m, n] = question.Mark;
        //                }
        //                else
        //                {
        //                    excel.WriteToCell(i, j, "0");
        //                    studentMarks[m, n] = 0;
        //                }
        //                j++;
        //                n++;

        //            }
                    
        //            //j++;
        //            excel.WriteToCell(i, j, markSum.ToString());
        //            m++;
        //            i++;
        //        }
        //        j = 0;
        //        excel.WriteToCell(i, j, "Average");

        //        j += 2;

        //        //sheet2
        //        n = 0;
        //        //sheet2

        //        //for (n = 0; n < examGroup.Questions.Count; n++)
        //        foreach (QuestionModel question in examGroup.Questions)
        //        {
        //            decimal questionAVG = 0;
        //            for (m = 0; m < students.Count; m++)
        //            {
        //                questionAVG += studentMarks[m, n];
        //            }
        //            excel.WriteToCell(i, j, (questionAVG / students.Count).ToString());
        //            j++;
        //            //sheet 2
        //            excel.SelectWorkSheet(2);
        //            excel.WriteToCell(k, l, (question.Name).ToString());
        //            l++;
        //            excel.WriteToCell(k, l, (questionAVG / students.Count).ToString());
        //            l++;
        //            excel.WriteToCell(k, l, "%" + (((questionAVG / students.Count) / question.Mark) * 100).ToString());
        //            k++;
        //            l = 0;
        //            excel.SelectWorkSheet(1);

        //            //sheet 2
        //            n++;
        //        }
        //        j = 0;
        //        i++;


        //    }

        //}

    }
}

        

        

