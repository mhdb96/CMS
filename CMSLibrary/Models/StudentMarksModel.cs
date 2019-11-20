using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMSLibrary.Models
{
    public class StudentMarksModel
    {
        public StudentModel Student { get; set; }
        public QuestionModel Question { get; set; }
        public ResultModel Result { get; set; }
    }
}
