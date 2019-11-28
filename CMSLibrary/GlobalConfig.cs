using CMSLibrary.DataAccess;

namespace CMSLibrary
{
    public static class GlobalConfig
    {
        public static IDataConnection Connection { get; private set; }
        public static void InitializeConnections()
        {
            Connection = new SqlConnector();
        }
        public static string CnnString(string name)
        {              
            return $"Data Source = {Ip},{Port}; Network Library = DBMSSOCN; Initial Catalog = CMS; User ID = {Username}; Password = {Password};";
        }
        public static string Ip = "192.168.1.8";
        public static string Port = "1433";
        public static string Username = "onur";
        public static string Password = "1903forza";
    }
}
