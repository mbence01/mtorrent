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
	public class UserController : APIControllerBase
	{
		public UserController(IConfiguration configuration) : base(configuration) {}
	
		/// <summary>
		/// Returns a <see cref="User"/> object associated with the given <paramref name="id"/>.
		/// </summary>
		/// <param name="token">Personal access token</param>
		/// <param name="id">User ID</param>
		/// <returns><see cref="User"/> object or null if not found</returns>
		[HttpGet]
		public User GetUserById(string token, int id)
		{
			_serviceManager.ThrowIfUnauthorized(token);
			
			return _serviceManager.GetUserById(id);
		}

		/// <summary>
		/// Returns all of the users associated with the given <paramref name="usernames"/>. It is going to search with the LIKE operator if the <paramref name="exactResults"/> parameter's value is false.
		/// </summary>
		/// <param name="token">Personal access token</param>
		/// <param name="usernames">List of usernames</param>
		/// <param name="exactResults">Exact results only or username parts included</param>
		/// <returns>List of <see cref="User"/></returns>
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
					Message = $"Welcome {user.Username}! You have logged in.",
					User = user
				};
			}
			catch (ManagerException ex) when (ex.ErrorCode == ManagerError.LoginUserObjectWasNull ||
			                                  ex.ErrorCode == ManagerError.LoginInvalidCredentials)
			{
				return new LoginResponse
				{
					StatusCode = StatusCodes.LoginFailed,
					Message = "Look like you have entered a wrong username or password. Please try again."
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
					Message = "Oops! Something went wrong in our side. Please try again a few minutes later."
				};
			}
		}
	};	
}