using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMSLibrary.Models
{
    public class StudentAnswersModel
    {
        public StudentModel Student { get; set; } = new StudentModel();
        public GroupModel Group { get; set; } = new GroupModel();
        public string AnswersList { get; set; }
        public int CorrectAnswersCount { get; set; }
    }
}
