using Torrent.Common.Model;
using Torrent.Service.Access.Common;

namespace Torrent.Service.Access
{
    public partial class ServiceAccess
    {
        public List<User> GetUserById(int id)
        {
            SqlWhereClause whereClause = new SqlWhereClause();
            
            whereClause.AddCondition("UserID", Operator.Equals, id.ToString());
            
            return DatabaseExtension.Select<User>(whereClause);
        }

        public List<User> GetUsersByUsername(List<string> usernames)
        {
            SqlWhereClause whereClause = new SqlWhereClause("OR");

            usernames.ForEach(name => whereClause.AddCondition("Username", Operator.Like, name));
            
            return DatabaseExtension.Select<User>(whereClause);
        }

        /// <summary>
        /// Gets the <paramref name="user"/>'s all passwords.
        /// </summary>
        /// <param name="user">User whose passwords will be returned.</param>
        /// <returns>A list of passwords</returns>
        public List<UserPassword> GetUserPasswords(User user)
        {
            return DatabaseExtension.Select<UserPassword>(SqlWhereClause.From("UserID", Operator.Equals, user.UserID.ToString()));
        }
    }
}
