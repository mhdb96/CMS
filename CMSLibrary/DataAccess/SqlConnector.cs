using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMSLibrary.Models;
using Dapper;

namespace CMSLibrary.DataAccess
{
    public class SqlConnector : IDataConnection
    {
        public static string databaseName = "CMS";


        public void DeleteActiveTerms(int id)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(databaseName)))
            {
                var p = new DynamicParameters();
                p.Add("@ActiveTermsId", id);
                connection.Execute("dbo.spActiveTerms_Delete", p, commandType: CommandType.StoredProcedure);
            }
        }

        public List<CourseModel> GetCourses_Valid()
        {
            List<CourseModel> output;
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(databaseName)))
            {
                output = connection.Query<CourseModel>("dbo.spCourses_Valid").ToList();
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
                foreach(DepartmentOutcomeModel dO in model.Outcomes)
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
                output = connection.Query<CourseModel>("dbo.spCourses_GetAll").ToList();
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
            throw new NotImplementedException();
        }

        public void CreateQuestion(QuestionModel model)
        {
            throw new NotImplementedException();
        }

        public void CreateResult(ResultModel model)
        {
            throw new NotImplementedException();
        }

        public void GetDepartmentOutcomes_ById(DepartmentModel model)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(databaseName)))
            {
                var p = new DynamicParameters();
                p.Add("@DepartmentId", model.Id);
                model.Outcomes = connection.Query<DepartmentOutcomeModel>("dbo.spDepartmentOutcomes_GetById",p , commandType: CommandType.StoredProcedure).ToList();
                
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
                    => {
                        assignment.Department = department;
                        activeTerm.Term = term;
                        activeTerm.Year = year;
                        assignment.ActiveTerm = activeTerm; 
                        assignment.Course = course;
                        assignment.Teacher = teacher;
                        return assignment; }).ToList();
            }
            return output;
        }
    }
}
