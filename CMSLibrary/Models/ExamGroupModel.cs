using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMSLibrary.Models
{
    public class ExamGroupModel
    {
        public int Id { get; set; }
        public GroupModel Group { get; set; }
        public List<QuestionModel> Questions { get; set; } = new List<QuestionModel>();
    }
}
