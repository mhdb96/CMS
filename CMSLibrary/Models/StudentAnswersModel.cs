namespace CMSLibrary.Models
{
    public class StudentAnswersModel
    {
        public StudentModel Student { get; set; } = new StudentModel();
        public GroupModel Group { get; set; } = new GroupModel();
        public string AnswersList { get; set; }
        public int CorrectAnswersCount { get; set; }
        public string ErrorType { get; set; }
    }
}
