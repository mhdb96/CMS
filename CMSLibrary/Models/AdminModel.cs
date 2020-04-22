namespace CMSLibrary.Models
{
    public class AdminModel
    {
        public int Id { get; set; }
        public int RegNo { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public UserModel User { get; set; }
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
