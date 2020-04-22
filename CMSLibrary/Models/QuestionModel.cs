using System.Collections.Generic;

namespace CMSLibrary.Models
{
    public class QuestionModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Mark { get; set; }
        public List<CourseOutcomeModel> QuestionOutcomes { get; set; } = new List<CourseOutcomeModel>();
        public List<ResultModel> Results { get; set; } = new List<ResultModel>();
        public int ExamGroupId { get; set; }

    }
}
