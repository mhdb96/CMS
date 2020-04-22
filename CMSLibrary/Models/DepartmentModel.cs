using System.Collections.Generic;

namespace CMSLibrary.Models
{
    public class DepartmentModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<DepartmentOutcomeModel> Outcomes { get; set; } = new List<DepartmentOutcomeModel>();
    }
}
