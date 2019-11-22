using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMSLibrary.Models
{
    public class QuestionOutcomeModel
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public int CourseOutcomeId { get; set; }
    }
}
