using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMSLibrary.Models
{
    public class ExamModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string FilePath { get; set; }
        public AssignmentModel Assignment { get; set; }
        public ExamTypeModel ExamType { get; set; }
    }
}
