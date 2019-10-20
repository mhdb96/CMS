using CMSLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMSLibrary
{
    public interface IDataConnection
    {
        void CreateActiveTerm(ActiveTermModel model);
        void CreateAssignment(AssignmentModel model);
        void CreateCourse(CourseModel model);
        void CreateCourseOutcome(CourseOutcomeModel model);
        void CreateDepaetment(DepartmentModel model);
        void CreateDepartmentOutcome(DepartmentOutcomeModel model);
        void CreateExam(ExamModel model);
        void CreateQuestion(QuestionModel model);
        void CreateResult(ResultModel model);
        void CreateTeacher(TeacherModel model);
    }
}
