using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMSUI
{
    public class TagData
    {
        public bool IsDeletable;
        public int Id;
        public bool IsNew;
        public OutcomeType Type;

    }
    public enum OutcomeType
    {
        CourseOutcome,
        DepartmentOutcome
    }
}
