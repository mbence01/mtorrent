using System.Data.SqlClient;
using System.IO;
using Torrent.Service.Access.Common;

namespace Torrent.Service.Access
{
    public partial class ServiceAccess
    {
        public void GetUserByUserId(int userID)
        {
            using (DatabaseConnector connector = new DatabaseConnector(_connectionString))
            {
            }
        }
    }
}
