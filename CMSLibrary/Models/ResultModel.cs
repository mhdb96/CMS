namespace CMSLibrary.Models
{
    public class ResultModel
    {
        public int Id { get; set; }
        public bool IsTrue { get; set; }
        public StudentModel Student { get; set; }
        public int QuestionId { get; set; }
    }
}
