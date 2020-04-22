namespace CMSLibrary.Models
{
    public class DepartmentOutcomeModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int DepartmentId { get; set; }
        public string Full
        {
            get
            {
                return $"{Name} - {Description}";
            }
        }

    }
}
