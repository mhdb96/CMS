using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMSLibrary;
using CMSLibrary.Models;

namespace CMSLibrary
{
    public class WriteToExcel
    {
        List<StudentMarksModel> StudentMark = new List<StudentMarksModel>();
        //List<QuestionModel> Questions;
        List<CourseOutcomeModel> courseOutcomes;

        //sheet 3
        //List<CourseModel> courseModel = GlobalConfig.Connection.GetCourse_All();
        //sheet 3

        //Excel excel = new Excel(@"C:\Users\onurk\Desktop\Test.xlsx", 1);
        Excel excel = new Excel();


        public WriteToExcel()
        {            
            ExamModel exam = GlobalConfig.Connection.GetExam_ById(20);
            exam.Assignment.Course.CourseOutcomes = GlobalConfig.Connection.GetCourseOutcome_GetByExamId(exam.Id);


            //GlobalConfig.Connection.GetCourseOutcomes_ById(courseModel[0]);
            //exam.Assignment.Course.Id = courseModel[0].Id;



            excel.CreateNewFile();
            excel.CreateNewSheet();
            excel.CreateNewSheet();

            WriteDataToExcelSheet1(exam);

            excel.SaveAs(@"C:\Users\mhdb\Desktop\Test.xlsx");
            excel.Close();
        }

        private void WriteDataToExcelSheet1(ExamModel exam)
        {
            int i = 0, j = 0;
            int k = 0, l = 0;
            int p = 0, r = 0;

            GlobalConfig.Connection.GetExamGroup_ByExamId(exam);
            foreach (ExamGroupModel examGroup in exam.ExamGroups)
            {
                //sheet3
                List<StudentModel> students = GlobalConfig.Connection.GetStudent_GetByExamGroupId(examGroup.Id);
                if (students.Count == 0)
                {
                    continue;
                }

                //courseOutcomes = GlobalConfig.Connection.GetQuestionOutcomes_GetByCourseIdAndExamGroupsId(examGroup.Id, exam.Assignment.Course.Id);
                courseOutcomes = exam.Assignment.Course.CourseOutcomes;

                excel.SelectWorkSheet(3);

                excel.WriteToCell(p, r, examGroup.Group.Name);
                p++;
                excel.WriteToCell(p, r, "Kazanım Adı");
                r++;
                excel.WriteToCell(p, r, "Ortalaması (Puan)");
                r++;
                excel.WriteToCell(p, r, "Başarımı(%)");
                p++;
                r = 0;

                foreach (var courseOutcome in courseOutcomes)
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
                            qtoplam += question.Mark;
                        }
                    }
                    excel.WriteToCell(p, r, courseOutcome.Name);
                    r++;
                    excel.WriteToCell(p, r, (toplam / students.Count).ToString());
                    r++;
                    excel.WriteToCell(p, r, (((toplam / students.Count) / qtoplam) * 100).ToString());
                    p++;
                    r = 0;
                }

                excel.SelectWorkSheet(1);

                //sheet3

                examGroup.Questions = GlobalConfig.Connection.GetQuestions_GetByExamGroupId(examGroup.Id);


                excel.WriteToCell(i, j, examGroup.Group.Name);
                i++;
                excel.WriteToCell(i, j, "Student Reg No");
                j++;
                excel.WriteToCell(i, j, "Full Name");
                j++;

                foreach (var question in examGroup.Questions)
                {
                    excel.WriteToCell(i, j, $"Soru {question.Name}");
                    j++;
                }
                excel.WriteToCell(i, j, "Sum");

                //sheet 2
                excel.SelectWorkSheet(2);

                excel.WriteToCell(k, l, examGroup.Group.Name);
                k++;
                excel.WriteToCell(k, l, "Soru Numarası");
                l++;
                excel.WriteToCell(k, l, "Ortalaması (Puan)");
                l++;
                excel.WriteToCell(k, l, "Başarımı(%)");
                k++;
                l = 0;
                excel.SelectWorkSheet(1);

                //sheet 2

                //List<StudentModel> students = GlobalConfig.Connection.GetStudent_GetByExamGroupId(examGroup.Id);
                i++;
                decimal[,] studentMarks = new decimal[students.Count, examGroup.Questions.Count];
                int m = 0, n = 0;
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
                    m++;
                    //j++;
                    excel.WriteToCell(i, j, markSum.ToString());
                    i++;
                }
                j = 0;
                excel.WriteToCell(i, j, "Average");

                j += 2;

                //sheet2
                n = 0;
                //sheet2
                //for (n = 0; n < examGroup.Questions.Count; n++)
                foreach (QuestionModel question in examGroup.Questions)
                {
                    decimal questionAVG = 0;
                    for (m = 0; m < students.Count; m++)
                    {
                        questionAVG += studentMarks[m, n];
                    }
                    excel.WriteToCell(i, j, (questionAVG / students.Count).ToString());
                    j++;
                    //sheet 2
                    excel.SelectWorkSheet(2);
                    excel.WriteToCell(k, l, (question.Name).ToString());
                    l++;
                    excel.WriteToCell(k, l, (questionAVG / students.Count).ToString());
                    l++;
                    excel.WriteToCell(k, l, "%" + (((questionAVG / students.Count) / question.Mark) * 100).ToString());
                    k++;
                    l = 0;
                    excel.SelectWorkSheet(1);

                    //sheet 2
                    n++;
                }
                j = 0;
                i++;


            }

        }

    }
}

        

        

