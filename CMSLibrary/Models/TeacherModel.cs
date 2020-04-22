namespace CMSLibrary.Models
{
    public class TeacherModel
    {
        public int Id { get; set; }
        public int RegNo { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public UserModel User { get; set; } = new UserModel();
        public string FullName
        {
            get
            {
                return $"{RegNo} - {FirstName} {LastName}";
            }
        }
        public string Full
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }
    }
}
