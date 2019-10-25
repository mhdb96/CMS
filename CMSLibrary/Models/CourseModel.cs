using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMSLibrary.Models
{
    public class CourseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public EducationalYearModel EduYear { get; set; }
        public List<CourseOutcomeModel> CourseOutcomes { get; set; } = new List<CourseOutcomeModel>();
    }
}
