using Torrent.Common.Model;
using Torrent.Service.Access.Common;

namespace Torrent.Service.Access
{
    public partial class ServiceAccess
    {
        public List<User> GetUserById(int id)
        {
            SqlWhereClause whereClause = new SqlWhereClause();
            
            whereClause.AddCondition("UserID", Operator.GreaterThan, id.ToString());
            
            return DatabaseExtension.Select<User>(whereClause);
        }
    }
}
