using CMSLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMSLibrary.DataAccess
{
    public interface IDataConnection
    {

        bool DeleteAssignment_ById(int id);
        bool DeleteCourse_ById(int id);
        bool DeleteTeacher_ById(int id);
        bool DeleteDepartment_ById(int id);
        bool DeleteActiveTerm_ById(int id);
        List<CourseModel> GetCourse_ValidByDepartmentIdAndActiveTermId(int DepartmentId, int ActiveTermId);
        List<TermModel> GetTerm_ValidByYearId(int id);

        void CreateActiveTerm(ActiveTermModel model);
        List<YearModel> GetYear_ALL();
        List<TermModel> GetTerm_ALL();

        void CreateAssignment(AssignmentModel model);
        List<DepartmentModel> GetDepartment_All();
        List<ActiveTermModel> GetActiveTerm_All();
        List<CourseModel> GetCourse_All();
        List<TeacherModel> GetTeacher_All();

        void CreateCourse(CourseModel model);
        void CreateCourseOutcome(CourseOutcomeModel model);
        List<EducationalYearModel> GetEducationalYear_ALL();
        void GetCourseOutcomes_ById(CourseModel model);

        void CreateDepartment(DepartmentModel model);
        void CreateDepartmentOutcome(DepartmentOutcomeModel model);
        void GetDepartmentOutcomes_ById(DepartmentModel model);

        void CreateExam(ExamModel model);
        void CreateQuestion(QuestionModel model);
        void CreateResult(ResultModel model);

        void CreateTeacher(TeacherModel model);

        List<TeacherModel> GetFullTeacher_All();
        List<AssignmentModel> GetAssignment_All();

    }
}
