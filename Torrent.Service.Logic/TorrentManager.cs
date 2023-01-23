namespace Torrent.Service.Logic
{
    public partial class ServiceManager
    {
        /// <summary>
        /// Returns all of the torrent models associated with the given <paramref name="userID"/>
        /// </summary>
        /// <param name="userID">User ID</param>
        /// <returns>A list of torrent files</returns>
        public List<Torrent.Common.Model.Torrent> GetUserTorrents(int userID)
        {
            return _serviceAccess.GetUserTorrents(userID);
        }
    }
}