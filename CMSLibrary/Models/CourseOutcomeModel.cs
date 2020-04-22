namespace CMSLibrary.Models
{
    public class CourseOutcomeModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CourseId { get; set; }
        public string Full
        {
            get
            {
                return $"{Name} - {Description}";

            }
        }
    }
}
