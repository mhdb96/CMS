using CMSLibrary.DataAccess;
using System.Configuration;
using System.IO;
using System.Text;

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
            try
            {
                string[] data = File.ReadAllLines($"{System.AppDomain.CurrentDomain.BaseDirectory}info.txt", Encoding.GetEncoding("iso-8859-9"));
                string[] info = data[0].Split(';');
                Ip = info[0];
                Port = info[1];
                Username = info[2];
                Password = info[3];
                return $"Data Source = {Ip},{Port}; Network Library = DBMSSOCN; Initial Catalog = CMS; User ID = {Username}; Password = {Password};";
            }
            catch (System.Exception)
            {
                return ConfigurationManager.ConnectionStrings[name].ConnectionString;
            }
        }
        public static string Ip = "192.168.1.26";
        public static string Port = "1433";
        public static string Username = "sa";
        public static string Password = "alpha86";
    }
}
