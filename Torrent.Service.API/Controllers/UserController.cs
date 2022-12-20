using Microsoft.AspNetCore.Mvc;
using Torrent.Common.Logging;
using Torrent.Common.Model;
using Torrent.Common.Model.Enum;
using Torrent.Common.Model.Exception;
using Torrent.Common.Model.Request;
using Torrent.Common.Model.Response;
using Torrent.Service.Logic;
using StatusCodes = Torrent.Common.Model.Enum.StatusCodes;

namespace Torrent.Service.API.Controllers
{
	[ApiController]
	[Route("[controller]/[action]")]
	public class UserController : ControllerBase
	{
		private readonly ServiceManager _serviceManager;

		public UserController(IConfiguration configuration)
		{
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
		public User GetUserById(string token, int id)
		{
			_serviceManager.ThrowIfUnauthorized(token);
			
			return _serviceManager.GetUserById(id);
		}

		[HttpGet]
		public List<User> GetUsersByUsername(string token, List<string> usernames, bool exactResults = true)
		{
			_serviceManager.ThrowIfUnauthorized(token);

			return _serviceManager.GetUsersByUsername(usernames, exactResults);
		}

		/// <summary>
		/// Logins a user if valid credentials have been passed, otherwise returns error message.
		/// </summary>
		/// <param name="token">Personal access token</param>
		/// <param name="request">Login request object</param>
		/// <returns><see cref="LoginResponse"/> object which contains status code, message and if login was successful a <see cref="User"/> object.</returns>
		[HttpPost]
		public LoginResponse LoginUser(string token, [FromBody] LoginRequest request)
		{
			_serviceManager.ThrowIfUnauthorized(token);

			try
			{
				User user = _serviceManager.LoginUser(request.Username, request.Password);

				return new LoginResponse
				{
					StatusCode = StatusCodes.LoginSuccess,
					Message = $"You have successfully been logged in as {user.Username}",
					User = user
				};
			}
			catch (ManagerException ex) when (ex.ErrorCode == ManagerError.LoginUserObjectWasNull ||
			                                  ex.ErrorCode == ManagerError.LoginInvalidCredentials)
			{
				return new LoginResponse
				{
					StatusCode = StatusCodes.LoginFailed,
					Message = "Invalid username or password. Try again."
				};
			}
			catch (ManagerException ex) when (ex.ErrorCode == ManagerError.LoginPasswordExpired)
			{
				return new LoginResponse
				{
					StatusCode = StatusCodes.LoginSuccessPasswordExpired,
					Message = "Your password has expired. Please change it ASAP."
				};
			}
			catch (Exception ex)
			{
				Logger.As(Log.Error).Exception(ex).From(this).Method("LoginUser").Write();
				
				return new LoginResponse
				{
					StatusCode = StatusCodes.InternalServerError,
					Message = "Unexpected server error has occurred. Please try your request again a few minutes later."
				};
			}
		}
	};	
}