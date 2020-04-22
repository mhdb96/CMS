using CMSLibrary.Enums;
using CMSLibrary.Models;

namespace CMSLibrary
{
    public class Login
    {
        public static AuthenticationState Check(UserModel localUser)
        {

            UserModel databaseUser = GlobalConfig.Connection.GetUser_ByUserName(localUser.UserName);
            if (databaseUser == null)
            {
                return AuthenticationState.UserNotFound;
            }
            if (databaseUser.Password != localUser.Password)
            {
                return AuthenticationState.WrongPassword;
            }
            localUser.Id = databaseUser.Id;
            localUser.Role = databaseUser.Role;
            return AuthenticationState.Authenticated;
        }
    }
}
