namespace CMSLibrary.Models
{
    public class StudentMarksModel
    {
        public StudentModel Student { get; set; }
        public QuestionModel Question { get; set; }
        public ResultModel Result { get; set; }
    }
}
