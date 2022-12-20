using Torrent.Common.Model;
using Torrent.Common.Model.Exception;

namespace Torrent.Service.Logic;

public partial class ServiceManager
{
	/// <summary>
	/// Checks if the given <paramref name="token"/> is valid at the current timestamp or not.
	/// </summary>
	/// <param name="token">The token string</param>
	/// <returns>True if the given token is valid, otherwise false.</returns>
	public bool IsTokenValid(string token)
	{
		return _serviceAccess.GetUserByToken(token) != null;
	}

	/// <summary>
	/// Throws an <see cref="UnauthorizedException"/> if the given <paramref name="token"/> was not found in the database or was expired.
	/// </summary>
	/// <param name="token">The token string</param>
	/// <exception cref="UnauthorizedException"></exception>
	public void ThrowIfUnauthorized(string token)
	{
		if (GetUserByToken(token) == null)
			throw new UnauthorizedException();
	}

	/// <summary>
	/// Returns the associated User object with the given <paramref name="token"/> or null if no user was found.
	/// </summary>
	/// <param name="token">The token associated with a user</param>
	/// <returns>User object or null</returns>
	public User? GetUserByToken(string token)
	{
		return _serviceAccess.GetUserByToken(token);
	}
}