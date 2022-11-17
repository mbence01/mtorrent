using Microsoft.AspNetCore.Mvc;
using Torrent.Common.Model;
using Torrent.Service.Logic;
using ConfigurationManager = System.Configuration.ConfigurationManager;

namespace Torrent.Service.API.Controllers
{
	[ApiController]
	[Route("[action]")]
	public class UserController : ControllerBase
	{
		private readonly ServiceManager _serviceManager;

		public UserController()
		{
			this._serviceManager = new ServiceManager();
		}
	
		[HttpPost]
		public User TestUserInsert([FromBody] User user)
		{
			return _serviceManager.TestUserInsert(user);
		}
	};	
}