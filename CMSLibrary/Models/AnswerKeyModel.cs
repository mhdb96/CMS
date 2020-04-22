namespace CMSLibrary.Models
{
    public class AnswerKeyModel
    {
        public GroupModel Group { get; set; } = new GroupModel();
        public string AnswersList { get; set; }
        public int QuestionCount { get; set; }

    }
}
