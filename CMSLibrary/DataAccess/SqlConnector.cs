using CMSLibrary.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace CMSLibrary.DataAccess
{
    public class SqlConnector : IDataConnection
    {
        public static string databaseName = "CMS";

        public List<CourseModel> GetCourse_BySearchValue(string searchValue)
        {
            List<CourseModel> output;
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(databaseName)))
            {
                var p = new DynamicParameters();
                p.Add("@SearchValue", searchValue);
                output = connection.Query<CourseModel, EducationalYearModel, CourseModel>("dbo.spCourses_BySearchValue",
                    (course, eduYear) =>
                    {
                        course.EduYear = eduYear;
                        return course;
                    }, p, commandType: CommandType.StoredProcedure).ToList();
            }
            return output;
        }

        public List<ActiveTermModel> GetActiveTerm_BySearchValue(string searchValue)
        {
            List<ActiveTermModel> output;
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(databaseName)))
            {
                var p = new DynamicParameters();
                p.Add("@SearchValue", searchValue);
                output = connection.Query<ActiveTermModel, TermModel, YearModel, ActiveTermModel>("dbo.spActiveTerms_BySearchValue",
                    (activeTerm, term, year) => { activeTerm.Term = term; activeTerm.Year = year; return activeTerm; }, p, commandType: CommandType.StoredProcedure).ToList();
            }
            return output;
        }

        public List<TeacherModel> GetTeacher_BySearchValue(string searchValue)
        {
            List<TeacherModel> output;
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(databaseName)))
            {
                var p = new DynamicParameters();
                p.Add("@SearchValue", searchValue);
                output = connection.Query<TeacherModel, UserModel, TeacherModel>("dbo.spTeachers_BySearchValue",
                    (teacher, user) => { teacher.User = user; return teacher; }, p, commandType: CommandType.StoredProcedure).ToList();
            }
            return output;
        }

        public List<DepartmentModel> GetDepartment_BySearchValue(string searchValue)
        {
            List<DepartmentModel> output;
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(databaseName)))
            {
                var p = new DynamicParameters();
                p.Add("@SearchValue", searchValue);
                output = connection.Query<DepartmentModel>("dbo.spDepartments_BySearchValue", p, commandType: CommandType.StoredProcedure).ToList();
            }
            return output;
        }


        public void DepartmentOutcome_Delete(int id)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(databaseName)))
            {
                var p = new DynamicParameters();
                p.Add("@DepartmentOutcomeId", id);
                connection.Execute("dbo.spDepartmentOutcomes_Delete", p, commandType: CommandType.StoredProcedure);
            }
        }

        public void CourseOutcome_Delete(int id)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(databaseName)))
            {
                var p = new DynamicParameters();
                p.Add("@CourseOutcomeId", id);
                connection.Execute("dbo.CourseOutcomes_Delete", p, commandType: CommandType.StoredProcedure);
            }
        }

        public bool CourseOutcome_IsDeletable(int id)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(databaseName)))
            {
                List<CourseOutcomeModel> courseOutcomes = new List<CourseOutcomeModel>();
                var p = new DynamicParameters();
                p.Add("@CourseOutcomeId", id);
                courseOutcomes = connection.Query<CourseOutcomeModel>("dbo.spCourseOutcomes_HasQuestionOutcomesByCourseOutcomeId", p, commandType: CommandType.StoredProcedure).ToList();

                if (courseOutcomes.Any())
                {
                    return false;
                }
                else
                {
                    return true;
                }

            }
        }

        public void UpdateCourseOutcome(CourseOutcomeModel model)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(databaseName)))
            {
                var p = new DynamicParameters();
                p.Add("@Id", model.Id);
                p.Add("@Name", model.Name);
                p.Add("@Description", model.Description);
                p.Add("@CourseId", model.CourseId);

                connection.Execute("dbo.spCourseOutcomes_UpdateByCourseOutcomesId", p, commandType: CommandType.StoredProcedure);
            }
        }
        public void UpdateCourse(CourseModel model)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(databaseName)))
            {
                var p = new DynamicParameters();

                p.Add("@Id", model.Id);
                p.Add("@Name", model.Name);
                p.Add("@Code", model.Code);
                p.Add("@EduYearId", model.EduYear.Id);

                connection.Execute("dbo.spCourse_UpdateByCourseId", p, commandType: CommandType.StoredProcedure);

                //foreach (CourseOutcomeModel dO in model.CourseOutcomes)
                //{
                //    //dO.DepartmentId = model.Id;
                //    UpdateCourseOutcome(dO);
                //}
            }
        }


        public void UpdateDepartmentOutcome(DepartmentOutcomeModel model)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(databaseName)))
            {
                var p = new DynamicParameters();
                p.Add("@OutcomeId", model.Id);
                p.Add("@Name", model.Name);
                p.Add("@Description", model.Description);

                connection.Execute("dbo.spDepartmentOutcomes_UpdateByDepartmentOutcomesId", p, commandType: CommandType.StoredProcedure);
            }
        }
        public void UpdateDepartment(DepartmentModel model)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(databaseName)))
            {
                var p = new DynamicParameters();

                p.Add("@DepartmentsId", model.Id);
                p.Add("@DepartmentName", model.Name);


                connection.Execute("dbo.spDepartment_UpdateByDepartmentId", p, commandType: CommandType.StoredProcedure);

                //foreach (DepartmentOutcomeModel dO in model.Outcomes)
                //{
                //    //dO.DepartmentId = model.Id;
                //    UpdateDepartmentOutcome(dO);
                //}
            }
        }

        public void UpdateAssignments(AssignmentModel model)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(databaseName)))
            {
                var p = new DynamicParameters();

                p.Add("@AssignmentId", model.Id);
                p.Add("@TeacherId", model.Teacher.Id);
                p.Add("@CourseId", model.Course.Id);
                p.Add("@DepartmentId", model.Department.Id);
                p.Add("@ActiveTermId", model.ActiveTerm.Id);

                connection.Execute("dbo.spAssignment_UpdateByAssignmentId", p, commandType: CommandType.StoredProcedure);
            }
        }

        public void UpdateActiveTerms(ActiveTermModel model)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(databaseName)))
            {
                var p = new DynamicParameters();

                p.Add("@ActiveTermId", model.Id);
                p.Add("@YearId", model.Year.Id);
                p.Add("@TermId", model.Term.Id);

                connection.Execute("dbo.spActiveTerms_UpdateByYearIdAndTermId", p, commandType: CommandType.StoredProcedure);
            }
        }

        public void UpdateTeachers(TeacherModel model)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(databaseName)))
            {
                var p = new DynamicParameters();

                p.Add("@UserId", model.User.Id);
                p.Add("@RegNo", model.RegNo);
                p.Add("@FirstName", model.FirstName);
                p.Add("@LastName", model.LastName);
                p.Add("@UserName", model.User.UserName);
                p.Add("@Password", model.User.Password);

                connection.Execute("dbo.spTeachers_UpdateByUserId", p, commandType: CommandType.StoredProcedure);
            }
        }

        public List<CourseOutcomeModel> GetCourseOutcome_GetByExamId(int examId)
        {
            List<CourseOutcomeModel> output;
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(databaseName)))
            {
                var p = new DynamicParameters();
                p.Add("@ExamId", examId);
                output = connection.Query<CourseOutcomeModel>("dbo.spCourseOutcomes_GetByExamId", p, commandType: CommandType.StoredProcedure).ToList();
            }
            return output;
        }

        public List<ResultModel> GetResults_GetByQuestionId(int questionId)
        {
            List<ResultModel> output;
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(databaseName)))
            {
                var p = new DynamicParameters();
                p.Add("@QuestionId", questionId);
                output = connection.Query<ResultModel>("dbo.spResults_GetByQuestionId", p, commandType: CommandType.StoredProcedure).ToList();
            }
            return output;
        }

        public List<QuestionModel> GetQuestion_GetByCourseOutcomesIdAndExamGroupsId(int ExamGroupId, int CourseOutcomeId)
        {
            List<QuestionModel> output;
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(databaseName)))
            {
                var p = new DynamicParameters();
                p.Add("@ExamGroupId", ExamGroupId);
                p.Add("@CourseOutcomeId", CourseOutcomeId);
                output = connection.Query<QuestionModel>("dbo.spQuestion_GetByCourseOutcomesIdAndExamGroupsId", p, commandType: CommandType.StoredProcedure).ToList();
            }
            return output;
        }

        public List<CourseOutcomeModel> GetQuestionOutcomes_GetByCourseIdAndExamGroupsId(int ExamGroupId, int CourseId)
        {
            List<CourseOutcomeModel> output;
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(databaseName)))
            {
                var p = new DynamicParameters();
                p.Add("@ExamGroupId", ExamGroupId);
                p.Add("@CourseId", CourseId);
                output = connection.Query<CourseOutcomeModel>("dbo.spQuestionOutcomes_GetByCourseOutcomesIdAndExamGroupsId", p, commandType: CommandType.StoredProcedure).ToList();
            }
            return output;
        }

        public ResultModel GetResults_GetByStudentIdAndQuestionId(int studentId, int questionId)
        {
            ResultModel output;
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(databaseName)))
            {
                var p = new DynamicParameters();
                p.Add("@StudentId", studentId);
                p.Add("@QuestionId", questionId);
                output = connection.Query<ResultModel>("dbo.spResults_GetByStudentIdAndQuestionId", p, commandType: CommandType.StoredProcedure).First();
            }
            return output;
        }
        public List<StudentModel> GetStudent_GetByExamGroupId(int id)
        {
            List<StudentModel> output;
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(databaseName)))
            {
                var p = new DynamicParameters();
                p.Add("@ExamGroupId", id);
                output = connection.Query<StudentModel>("dbo.spStudents_GetByExamGroupId", p, commandType: CommandType.StoredProcedure).ToList();
            }
            return output;
        }
        public List<QuestionModel> GetQuestions_GetByExamGroupId(int id)
        {
            List<QuestionModel> output;
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(databaseName)))
            {
                var p = new DynamicParameters();
                p.Add("@ExamGroupId", id);
                output = connection.Query<QuestionModel>("dbo.spQuestions_GetByExamGroupId", p, commandType: CommandType.StoredProcedure).ToList();
            }
            return output;
        }

        public void GetExamGroup_ByExamId(ExamModel model)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(databaseName)))
            {
                var p = new DynamicParameters();
                p.Add("@ExamId", model.Id);
                model.ExamGroups = connection.Query<ExamGroupModel, GroupModel, ExamGroupModel>("dbo.spExamGroups_GetByExamId",
                    (examGroup, group)
                    =>
                    {
                        examGroup.Group = group;
                        return examGroup;

                    }, p, commandType: CommandType.StoredProcedure).ToList();


            }
        }
        public ExamModel GetExam_ById(int id)
        {
            ExamModel output;
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(databaseName)))
            {
                var p = new DynamicParameters();
                p.Add("@ExamId", id);
                output = connection.Query<ExamModel>("dbo.spExams_GetById", p, commandType: CommandType.StoredProcedure).First();

            }
            return output;
        }

        public List<QuestionModel> GetQuestion_ALL()
        {
            List<QuestionModel> output;
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(databaseName)))
            {
                output = connection.Query<QuestionModel>("dbo.spQuestions_GetAll").ToList();
            }
            return output;
        }

        public List<StudentModel> GetStudent_ALL()
        {
            List<StudentModel> output;
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(databaseName)))
            {
                output = connection.Query<StudentModel>("dbo.spStudents_GetAll").ToList();
            }
            return output;
        }

        public List<StudentMarksModel> GetStudentMark_ALL()
        {
            List<StudentMarksModel> output;
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(databaseName)))
            {

                output = connection.Query<StudentModel, ResultModel, QuestionModel, StudentMarksModel>("dbo.spStudentMarks_GetAll",
                    (student, result, question)
                    =>
                    {
                        StudentMarksModel studentMarks = new StudentMarksModel();
                        studentMarks.Student = student;
                        studentMarks.Result = result;
                        studentMarks.Question = question;
                        return studentMarks;
                    }).ToList();
            }
            return output;
        }


        public bool DeleteAssignment_ById(int id)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(databaseName)))
            {
                List<ExamModel> Exam = new List<ExamModel>();
                var p = new DynamicParameters();
                p.Add("@AssignmentId", id);
                Exam = connection.Query<ExamModel>("dbo.spAssignemtns_HasExamByAssignmentId", p, commandType: CommandType.StoredProcedure).ToList();

                if (Exam.Any())
                {
                    return false;
                }
                else
                {
                    connection.Execute("dbo.spAssignments_Delete", p, commandType: CommandType.StoredProcedure);
                    return true;
                }

            }
        }

        public void DeleteExam_ById(int id)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(databaseName)))
            {
                var p = new DynamicParameters();
                p.Add("@ExamId", id);
                connection.Execute("dbo.spExams_Delete", p, commandType: CommandType.StoredProcedure);

            }
        }

        public bool DeleteCourse_ById(int id)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(databaseName)))
            {
                List<CourseModel> Courses = new List<CourseModel>();
                var p = new DynamicParameters();
                p.Add("@CourseId", id);
                Courses = connection.Query<CourseModel>("dbo.spCourses_HasAssignmentByCourseId", p, commandType: CommandType.StoredProcedure).ToList();

                if (Courses.Any())
                {
                    return false;
                }
                else
                {
                    connection.Execute("dbo.spCourses_Delete", p, commandType: CommandType.StoredProcedure);
                    return true;
                }


            }
        }

        public bool DeleteTeacher_ById(int id)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(databaseName)))
            {
                List<ExamModel> Exams = new List<ExamModel>();
                var p = new DynamicParameters();
                p.Add("@TeacherId", id);
                Exams = connection.Query<ExamModel>("dbo.spAssignments_HasExamByTeacherId", p, commandType: CommandType.StoredProcedure).ToList();

                if (Exams.Any())
                {
                    return false;
                }
                else
                {
                    connection.Execute("dbo.spTeachers_Delete", p, commandType: CommandType.StoredProcedure);
                    return true;
                }
            }
        }

        public bool DeleteDepartment_ById(int id)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(databaseName)))
            {

                List<ExamModel> Exams = new List<ExamModel>();
                List<ExamModel> Student = new List<ExamModel>();

                var p = new DynamicParameters();
                p.Add("@DepartmentId", id);
                Exams = connection.Query<ExamModel>("dbo.spAssignments_HasExamByDepartmentId", p, commandType: CommandType.StoredProcedure).ToList();
                Student = connection.Query<ExamModel>("dbo.spDepartments_HasStudentByDepartmentId", p, commandType: CommandType.StoredProcedure).ToList();

                if (Exams.Any() || Student.Any())
                {
                    return false;
                }
                else
                {
                    connection.Execute("dbo.spDepartments_Delete", p, commandType: CommandType.StoredProcedure);
                    return true;
                }
            }
        }

        public bool DeleteActiveTerm_ById(int id)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(databaseName)))
            {
                List<ExamModel> Exams = new List<ExamModel>();
                var p = new DynamicParameters();
                p.Add("@ActiveTermId", id);
                Exams = connection.Query<ExamModel>("dbo.spAssignments_HasExamByActiveTermId", p, commandType: CommandType.StoredProcedure).ToList();

                if (Exams.Any())
                {
                    return false;
                }
                else
                {
                    connection.Execute("dbo.spActiveTerms_Delete", p, commandType: CommandType.StoredProcedure);
                    return true;
                }
            }
        }

        public List<CourseModel> GetCourse_ValidByDepartmentIdAndActiveTermId(int DepartmentId, int ActiveTermId)
        {
            List<CourseModel> output;
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(databaseName)))
            {
                var p = new DynamicParameters();
                p.Add("@DepartmentId", DepartmentId);
                p.Add("@ActiveTermId", ActiveTermId);

                output = connection.Query<CourseModel>("dbo.spCourses_Valid", p, commandType: CommandType.StoredProcedure).ToList();
            }
            return output;
        }

        public List<TermModel> GetTerm_ValidByYearId(int id)
        {
            List<TermModel> myTerms;
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(databaseName)))
            {
                var p = new DynamicParameters();
                p.Add("@Yearid", id);
                myTerms = connection.Query<TermModel>("dbo.spTerms_ValidByYearId", p, commandType: CommandType.StoredProcedure).ToList();
            }
            return myTerms;
        }

        public void CreateActiveTerm(ActiveTermModel model)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(databaseName)))
            {
                var p = new DynamicParameters();
                p.Add("@Yearid", model.Year.Id);
                p.Add("@TermId", model.Term.Id);
                p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
                connection.Execute("dbo.spActiveTerms_Insert", p, commandType: CommandType.StoredProcedure);
                model.Id = p.Get<int>("@id");
            }
        }

        public List<TermModel> GetTerm_ALL()
        {
            List<TermModel> output;
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(databaseName)))
            {
                output = connection.Query<TermModel>("dbo.spTerms_GetAll").ToList();
            }
            return output;
        }

        public List<YearModel> GetYear_ALL()
        {
            List<YearModel> output;
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(databaseName)))
            {
                output = connection.Query<YearModel>("dbo.spYears_GetAll").ToList();
            }
            return output;
        }

        public void CreateDepartment(DepartmentModel model)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(databaseName)))
            {
                var p = new DynamicParameters();
                p.Add("@Name", model.Name);
                p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
                connection.Execute("dbo.spDepartments_Insert", p, commandType: CommandType.StoredProcedure);
                model.Id = p.Get<int>("@id");
                foreach (DepartmentOutcomeModel dO in model.Outcomes)
                {
                    dO.DepartmentId = model.Id;
                    CreateDepartmentOutcome(dO);
                }
            }
        }

        public void CreateDepartmentOutcome(DepartmentOutcomeModel model)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(databaseName)))
            {
                var p = new DynamicParameters();
                p.Add("@Name", model.Name);
                p.Add("@Description", model.Description);
                p.Add("@DepartmentId", model.DepartmentId);
                p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
                connection.Execute("dbo.spDepartmentOutcomes_Insert", p, commandType: CommandType.StoredProcedure);
                model.Id = p.Get<int>("@id");
            }
        }

        public void CreateCourse(CourseModel model)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(databaseName)))
            {
                var p = new DynamicParameters();
                p.Add("@Name", model.Name);
                p.Add("@Code", model.Code);
                p.Add("@EduYearId", model.EduYear.Id);
                p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
                connection.Execute("dbo.spCourses_Insert", p, commandType: CommandType.StoredProcedure);
                model.Id = p.Get<int>("@id");
                foreach (CourseOutcomeModel cO in model.CourseOutcomes)
                {
                    cO.CourseId = model.Id;
                    CreateCourseOutcome(cO);
                }
            }
        }

        public List<EducationalYearModel> GetEducationalYear_ALL()
        {
            List<EducationalYearModel> output;
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(databaseName)))
            {
                output = connection.Query<EducationalYearModel>("dbo.spEducationalYears_GetAll").ToList();
            }
            return output;
        }

        public void CreateCourseOutcome(CourseOutcomeModel model)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(databaseName)))
            {
                var p = new DynamicParameters();
                p.Add("@Name", model.Name);
                p.Add("@Description", model.Description);
                p.Add("@CourseId", model.CourseId);
                p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
                connection.Execute("dbo.spCourseOutcomes_Insert", p, commandType: CommandType.StoredProcedure);
                model.Id = p.Get<int>("@id");
            }
        }

        public void GetCourseOutcomes_ById(CourseModel model)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(databaseName)))
            {
                var p = new DynamicParameters();
                p.Add("@CourseId", model.Id);
                model.CourseOutcomes = connection.Query<CourseOutcomeModel>("dbo.spCourseOutcomes_GetById", p, commandType: CommandType.StoredProcedure).ToList();

            }
        }

        public void CreateTeacher(TeacherModel model)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(databaseName)))
            {
                var p = new DynamicParameters();
                p.Add("@UserName", model.User.UserName);
                p.Add("@Password", model.User.Password);
                p.Add("@RoleId", 1);
                p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
                connection.Execute("dbo.spUsers_Insert", p, commandType: CommandType.StoredProcedure);
                model.User.Id = p.Get<int>("@id");

                p = new DynamicParameters();
                p.Add("@RegNo", model.RegNo);
                p.Add("@FirstName", model.FirstName);
                p.Add("@LastName", model.LastName);
                p.Add("@UserId", model.User.Id);
                p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
                connection.Execute("dbo.spTeachers_Insert", p, commandType: CommandType.StoredProcedure);
                model.Id = p.Get<int>("@id");
            }
        }

        public void CreateAssignment(AssignmentModel model)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(databaseName)))
            {
                var p = new DynamicParameters();
                p.Add("@TeacherId", model.Teacher.Id);
                p.Add("@CourseId", model.Course.Id);
                p.Add("DepartmentId", model.Department.Id);
                p.Add("ActiveTermId", model.ActiveTerm.Id);
                p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
                connection.Execute("dbo.spAssignments_Insert", p, commandType: CommandType.StoredProcedure);
                model.Id = p.Get<int>("@id");
            }
        }

        public List<DepartmentModel> GetDepartment_All()
        {
            List<DepartmentModel> output;
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(databaseName)))
            {
                output = connection.Query<DepartmentModel>("dbo.spDepartments_GetAll").ToList();
            }
            return output;
        }

        public List<ActiveTermModel> GetActiveTerm_All()
        {
            List<ActiveTermModel> output;
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(databaseName)))
            {
                output = connection.Query<ActiveTermModel, TermModel, YearModel, ActiveTermModel>("dbo.spActiveTerms_GetAll",
                    (activeTerm, term, year) => { activeTerm.Term = term; activeTerm.Year = year; return activeTerm; }).ToList();
            }
            return output;
        }

        public List<CourseModel> GetCourse_All()
        {
            List<CourseModel> output;
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(databaseName)))
            {
                output = connection.Query<CourseModel, EducationalYearModel, CourseModel>("dbo.spCourses_GetAll",
                    (course, eduYear) =>
                    {
                        course.EduYear = eduYear;
                        return course;
                    }, commandType: CommandType.StoredProcedure).ToList();
            }
            return output;
        }

        public List<TeacherModel> GetTeacher_All()
        {
            List<TeacherModel> output;
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(databaseName)))
            {
                output = connection.Query<TeacherModel>("dbo.spTeachers_GetAll").ToList();
            }
            return output;
        }



        public void CreateExam(ExamModel model)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(databaseName)))
            {
                var p = new DynamicParameters();
                p.Add("@Date", model.Date);
                p.Add("@FilePath", model.FilePath);
                p.Add("@ExamTypeId", model.ExamType.Id);
                p.Add("@AssignmentId", model.Assignment.Id);
                p.Add("@UserId", model.User.Id);
                p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
                connection.Execute("dbo.spExams_Insert", p, commandType: CommandType.StoredProcedure);
                model.Id = p.Get<int>("@id");
            }
        }

        public void CreateExamGroup(ExamGroupModel model)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(databaseName)))
            {
                var p = new DynamicParameters();
                p.Add("@GroupId", model.Group.Id);
                p.Add("@ExamId", model.ExamId);
                p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
                connection.Execute("dbo.spExamGroups_Insert", p, commandType: CommandType.StoredProcedure);
                model.Id = p.Get<int>("@id");
            }
        }

        public void CreateQuestion(QuestionModel model)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(databaseName)))
            {
                var p = new DynamicParameters();
                p.Add("@Name", model.Name);
                p.Add("@Mark", model.Mark);
                p.Add("@ExamGroupId", model.ExamGroupId);
                p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
                connection.Execute("dbo.spQuestions_Insert", p, commandType: CommandType.StoredProcedure);
                model.Id = p.Get<int>("@id");
            }
        }

        public void CreteQuestionOutcome(QuestionOutcomeModel model)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(databaseName)))
            {
                var p = new DynamicParameters();
                p.Add("@QuestionId", model.QuestionId);
                p.Add("@CourseOutcomeId", model.CourseOutcomeId);
                p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
                connection.Execute("dbo.spQuestionOutcomes_Insert", p, commandType: CommandType.StoredProcedure);
                model.Id = p.Get<int>("@id");
            }
        }

        public void CreateResult(ResultModel model)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(databaseName)))
            {
                var p = new DynamicParameters();
                p.Add("@IsTrue", model.IsTrue);
                p.Add("@StudentId", model.Student.Id);
                p.Add("@QuestionId", model.QuestionId);
                p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
                connection.Execute("dbo.spResults_Insert", p, commandType: CommandType.StoredProcedure);
                model.Id = p.Get<int>("@id");
            }
        }

        public StudentModel GetStudent_ByRegNo(int regNo)
        {
            StudentModel output;
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(databaseName)))
            {
                var p = new DynamicParameters();
                p.Add("@RegNo", regNo);
                output = connection.Query<StudentModel>("dbo.spStudents_GetByRegNo", p, commandType: CommandType.StoredProcedure).FirstOrDefault();

            }
            return output;
        }

        public void GetDepartmentOutcomes_ById(DepartmentModel model)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(databaseName)))
            {
                var p = new DynamicParameters();
                p.Add("@DepartmentId", model.Id);
                model.Outcomes = connection.Query<DepartmentOutcomeModel>("dbo.spDepartmentOutcomes_GetById", p, commandType: CommandType.StoredProcedure).ToList();

            }
        }

        public List<TeacherModel> GetFullTeacher_All()
        {
            List<TeacherModel> output;
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(databaseName)))
            {
                output = connection.Query<TeacherModel, UserModel, TeacherModel>("dbo.spTeachers_Full_GetAll",
                    (teacher, user) => { teacher.User = user; return teacher; }).ToList();
            }
            return output;
        }

        public List<AssignmentModel> GetAssignment_All()
        {
            List<AssignmentModel> output;
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(databaseName)))
            {
                output = connection.Query<AssignmentModel, DepartmentModel, ActiveTermModel, TermModel, YearModel, CourseModel, TeacherModel, AssignmentModel>("dbo.spAssignments_GetAll",
                    (assignment, department, activeTerm, term, year, course, teacher)
                    =>
                    {
                        assignment.Department = department;
                        activeTerm.Term = term;
                        activeTerm.Year = year;
                        assignment.ActiveTerm = activeTerm;
                        assignment.Course = course;
                        assignment.Teacher = teacher;
                        return assignment;
                    }).ToList();
            }
            return output;
        }

        public List<AssignmentModel> GetAssignment_ByTeacherId(int techerId)
        {
            List<AssignmentModel> output;
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(databaseName)))
            {
                var p = new DynamicParameters();
                p.Add("@TeacherId", techerId);
                output = connection.Query<AssignmentModel, DepartmentModel, ActiveTermModel, TermModel, YearModel, CourseModel, TeacherModel, AssignmentModel>("dbo.spAssignments_GetByTeacherId",
                    (assignment, department, activeTerm, term, year, course, teacher)
                    =>
                    {
                        assignment.Department = department;
                        activeTerm.Term = term;
                        activeTerm.Year = year;
                        assignment.ActiveTerm = activeTerm;
                        assignment.Course = course;
                        assignment.Teacher = teacher;
                        return assignment;
                    }, p, commandType: CommandType.StoredProcedure).ToList();
            }
            return output;
        }

        public UserModel GetUser_ByUserName(string userName)
        {
            UserModel output;
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(databaseName)))
            {
                var p = new DynamicParameters();
                p.Add("@UserName", userName);
                output = connection.Query<UserModel, RoleModel, UserModel>("dbo.spUsers_GetByUsername",
                    (user, role) =>
                    { user.Role = role; return user; },
                    p, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
            return output;
        }

        public AdminModel GetAdmin_ByUserId(int userId)
        {
            AdminModel output = new AdminModel();
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(databaseName)))
            {
                var p = new DynamicParameters();
                p.Add("@UserId", userId);
                output = connection.Query<AdminModel>("dbo.spAdmins_GetByUserId", p, commandType: CommandType.StoredProcedure).First();
            }
            return output;
        }

        public TeacherModel GetTeacher_ByUserId(int userId)
        {
            TeacherModel output = new TeacherModel();
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(databaseName)))
            {
                var p = new DynamicParameters();
                p.Add("@UserId", userId);
                output = connection.Query<TeacherModel>("dbo.spTeachers_GetByUserId", p, commandType: CommandType.StoredProcedure).First();
            }
            return output;
        }

        public string CheckConniction()
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(databaseName)))
            {
                try
                {
                    connection.Open();
                    return "";
                }
                catch (Exception ex)
                {

                    return ex.Message;
                }

            }
        }

        public List<ExamTypeModel> GetExamType_All()
        {
            List<ExamTypeModel> output;
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(databaseName)))
            {
                output = connection.Query<ExamTypeModel>("dbo.spExamTypes_GetAll").ToList();
            }
            return output;
        }

        public List<GroupModel> GetGroup_All()
        {
            List<GroupModel> output;
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(databaseName)))
            {
                output = connection.Query<GroupModel>("dbo.spGroups_GetAll").ToList();
            }
            return output;
        }

        public List<ExamModel> GetExam_ByAssignmentId(int assignmetId)
        {
            List<ExamModel> output = new List<ExamModel>();
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(databaseName)))
            {
                var p = new DynamicParameters();
                p.Add("@AssignmentId", assignmetId);
                output = connection.Query<ExamModel, ExamTypeModel, ExamModel>("dbo.spExams_GetByAssignmentId",
                    (exam, examType) =>
                    {
                        exam.ExamType = examType;
                        return exam;
                    }
                    , p, commandType: CommandType.StoredProcedure).ToList();
            }
            return output;
        }

        public void UpdateExam_ById(ExamModel model)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(databaseName)))
            {
                var p = new DynamicParameters();
                p.Add("ExamId", model.Id);
                p.Add("@Date", model.Date);
                p.Add("@ExamTypeId", model.ExamType.Id);
                p.Add("@FilePath", model.FilePath);
                connection.Execute("dbo.spExams_Update", p, commandType: CommandType.StoredProcedure);
            }
        }

        public string CreateStudent(StudentModel model)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(databaseName)))
            {
                var p = new DynamicParameters();
                p.Add("@RegNo", model.RegNo);
                p.Add("@FirstName", model.FirstName);
                p.Add("@LastName", model.LastName);
                p.Add("@DepartmentId", model.Department.Id);
                p.Add("@EduYearId", model.EduYear.Id);
                p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
                try
                {
                    connection.Execute("dbo.spStudents_Insert", p, commandType: CommandType.StoredProcedure);
                    model.Id = p.Get<int>("@id");
                }
                catch (Exception)
                {
                    return $"{model.FullName} Could not be add to the database due to duplication in RegNo {model.RegNo}{System.Environment.NewLine}";
                }
                return null;
            }
        }

        public bool User_ValidByUsername(string username)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(databaseName)))
            {
                List<UserModel> users = new List<UserModel>();
                var p = new DynamicParameters();
                p.Add("@UserName", username);
                users = connection.Query<UserModel>("dbo.spUsers_ValidByUsername", p, commandType: CommandType.StoredProcedure).ToList();

                if (users.Any())
                {
                    return false;
                }
                else
                {
                    return true;
                }

            }
        }
    }
}
