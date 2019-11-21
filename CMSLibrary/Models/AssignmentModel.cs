using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMSLibrary.Models
{
    public class AssignmentModel
    {
        public int Id { get; set; }
        public TeacherModel Teacher { get; set; }
        public CourseModel Course { get; set; } = new CourseModel();
        public DepartmentModel Department { get; set; }
        public ActiveTermModel ActiveTerm { get; set; }
    }
}
