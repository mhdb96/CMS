using System.Collections.Generic;

namespace CMSLibrary.Models
{
    public class CourseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public EducationalYearModel EduYear { get; set; }
        public List<CourseOutcomeModel> CourseOutcomes { get; set; } = new List<CourseOutcomeModel>();
        public string Full
        {
            get
            {
                return $"{Code} - {Name}";

            }
        }
    }
}
