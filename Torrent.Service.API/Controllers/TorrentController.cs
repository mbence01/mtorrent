using Microsoft.AspNetCore.Mvc;
using Torrent.Common.Model;

namespace Torrent.Service.API.Controllers
{
	[ApiController]
	[Route("[controller]/[action]")]
	public class TorrentController : APIControllerBase
	{
		public TorrentController(IConfiguration configuration) : base(configuration) {}

		/// <summary>
		/// Returns all of the torrents associated with the given <paramref name="user"/>'s ID
		/// </summary>
		/// <param name="token">Personal access token</param>
		/// <param name="user"><see cref="User"/> object</param>
		/// <returns>List of torrents associated with the user</returns>
		[HttpPost]
		public List<Common.Model.Torrent> GetUserTorrents(string token, [FromBody] User user)
		{
			_serviceManager.ThrowIfUnauthorized(token);

			return _serviceManager.GetUserTorrents(user.UserID);
		}

        /// <summary>
        /// Returns all of the torrents associated with the given <paramref name="userID"/>
        /// </summary>
        /// <param name="token">Personal access token</param>
        /// <param name="userID">A <see cref="User"/>'s ID</param>
        /// <returns>List of torrents associated with the user ID</returns>
        [HttpPost]
		public List<Common.Model.Torrent> GetUserTorrents(string token,  [FromBody] int userID)
		{
			_serviceManager.ThrowIfUnauthorized(token);

			return _serviceManager.GetUserTorrents(userID);
		}
	};	
}