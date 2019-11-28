using CMSLibrary.Models;
using System.Collections.Generic;

namespace CMSLibrary.DataAccess
{
    public interface IDataConnection
    {
        string CheckConniction();


        bool User_ValidByUsername(string username);

        //TODO - control the names
        List<CourseModel> GetCourse_BySearchValue(string searchValue);
        List<ActiveTermModel> GetActiveTerm_BySearchValue(string searchValue);
        List<TeacherModel> GetTeacher_BySearchValue(string searchValue);
        List<DepartmentModel> GetDepartment_BySearchValue(string searchValue);

        string CreateStudent(StudentModel model);


        void DepartmentOutcome_Delete(int id);
        void CourseOutcome_Delete(int id);
        bool CourseOutcome_IsDeletable(int id);
        void UpdateCourseOutcome(CourseOutcomeModel model);
        void UpdateCourse(CourseModel model);
        void UpdateDepartmentOutcome(DepartmentOutcomeModel model);
        void UpdateDepartment(DepartmentModel model);
        void UpdateAssignments(AssignmentModel model);
        void UpdateActiveTerms(ActiveTermModel model);
        void UpdateTeachers(TeacherModel model);

        List<CourseOutcomeModel> GetCourseOutcome_GetByExamId(int examId);
        List<ResultModel> GetResults_GetByQuestionId(int questionId);
        List<QuestionModel> GetQuestion_GetByCourseOutcomesIdAndExamGroupsId(int examGroupId, int courseOutcomeId);
        List<CourseOutcomeModel> GetQuestionOutcomes_GetByCourseIdAndExamGroupsId(int examGroupId, int courseId);
        ResultModel GetResults_GetByStudentIdAndQuestionId(int studentId, int questionId);
        List<StudentModel> GetStudent_GetByExamGroupId(int id);
        List<QuestionModel> GetQuestions_GetByExamGroupId(int id);
        void GetExamGroup_ByExamId(ExamModel model);
        ExamModel GetExam_ById(int id);
        List<QuestionModel> GetQuestion_ALL();
        List<StudentModel> GetStudent_ALL();
        List<StudentMarksModel> GetStudentMark_ALL();

        List<ExamModel> GetExam_ByAssignmentId(int assignmetId);
        void DeleteExam_ById(int id);
        void UpdateExam_ById(ExamModel model);



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
        List<ExamTypeModel> GetExamType_All();

        void CreteQuestionOutcome(QuestionOutcomeModel model);

        void CreateExamGroup(ExamGroupModel model);

        void CreateQuestion(QuestionModel model);
        void CreateResult(ResultModel model);

        StudentModel GetStudent_ByRegNo(int regNo);

        void CreateTeacher(TeacherModel model);

        List<TeacherModel> GetFullTeacher_All();
        List<AssignmentModel> GetAssignment_All();

        UserModel GetUser_ByUserName(string userName);

        AdminModel GetAdmin_ByUserId(int userId);
        TeacherModel GetTeacher_ByUserId(int userId);
        List<AssignmentModel> GetAssignment_ByTeacherId(int techerId);

        List<GroupModel> GetGroup_All();

    }
}
