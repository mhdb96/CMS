using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMSLibrary.DataAccess;

namespace CMSLibrary
{
    public static class GlobalConfig
    {
        public static List<IDataConnection> Connections { get; private set; } = new List<IDataConnection>();
        public static void InitializeConnections () 
        {
            SqlConnector sql = new SqlConnector();
            Connections.Add(sql);
        }
    }
}
