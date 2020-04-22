using System.Collections.Generic;

namespace CMSLibrary.Models
{
    public class ExamGroupModel
    {
        public int Id { get; set; }
        public GroupModel Group { get; set; }
        public int ExamId { get; set; }
        public List<QuestionModel> Questions { get; set; } = new List<QuestionModel>();
    }
}
