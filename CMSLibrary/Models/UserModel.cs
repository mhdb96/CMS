namespace CMSLibrary.Models
{
    public class UserModel
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public RoleModel Role { get; set; }
    }
}
