using Microsoft.AspNetCore.Mvc;
using Torrent.Common.Model;
using Torrent.Common.Model.Exception;
using Torrent.Service.Logic;

namespace Torrent.Service.API.Controllers
{
	[ApiController]
	[Route("[controller]/[action]")]
	public class UserController : ControllerBase
	{
		private readonly IConfiguration _configuration;
		private readonly ServiceManager _serviceManager;

		public UserController(IConfiguration configuration)
		{
			this._configuration = configuration;

			#region Set the connection string
			string? connectionString = configuration.GetConnectionString("DefaultSQLConnection");

			if (connectionString == null)
			{
				throw new MTorrentApiException("User", "Constructor", "The given connection string was null.");
			}
			#endregion
			
			this._serviceManager = new ServiceManager(connectionString, configuration.GetValue<int>("MaxSQLConnectionAttempts"));
		}
	
		[HttpGet]
		public List<User> GetUserById(int id)
		{
			return _serviceManager.GetUserById(id);
		}
	};	
}