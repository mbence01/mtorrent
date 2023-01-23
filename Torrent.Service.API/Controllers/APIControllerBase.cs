using Microsoft.AspNetCore.Mvc;
using Torrent.Common.Model.Exception;
using Torrent.Service.Logic;

namespace Torrent.Service.API.Controllers;

public class APIControllerBase : ControllerBase
{
	protected readonly ServiceManager _serviceManager;

	public APIControllerBase(IConfiguration configuration)
	{
		#region Set the connection string
		string? connectionString = configuration.GetConnectionString("DefaultSQLConnection");

		if (connectionString == null)
		{
			throw new MTorrentApiException("APIBase", "Constructor", "The given connection string was null.");
		}
		#endregion
			
		this._serviceManager = new ServiceManager(connectionString, configuration.GetValue<int>("MaxSQLConnectionAttempts"));
	}
}