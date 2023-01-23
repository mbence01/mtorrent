using Torrent.Common.Model;
using Torrent.Service.Access.Common;

namespace Torrent.Service.Access
{
    public partial class ServiceAccess
    {
        /// <summary>
        /// Selects all of the torrents from the database associated with the given <paramref name="userID"/>
        /// </summary>
        /// <param name="userID">User ID</param>
        /// <returns>List of torrents</returns>
        public List<Torrent.Common.Model.Torrent> GetUserTorrents(int userID)
        {
            return 
                DatabaseExtension.Select<Torrent.Common.Model.Torrent>(SqlWhereClause.From("UserID", Operator.Equals, userID.ToString()));
        }
    }
}
