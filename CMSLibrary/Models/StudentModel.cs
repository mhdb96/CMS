namespace CMSLibrary.Models
{
    public class StudentModel
    {
        public int Id { get; set; }
        public int RegNo { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DepartmentModel Department { get; set; }
        public EducationalYearModel EduYear { get; set; }
        public string FullName
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }
        public string ErrorType { get; set; }
        public string AnswersList { get; set; }

    }
}
