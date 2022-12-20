using Torrent.Common.Extension;
using Torrent.Common.Model;
using Torrent.Common.Model.Enum;
using Torrent.Common.Model.Exception;
using Torrent.Service.Logic.Common;

namespace Torrent.Service.Logic
{
	public partial class ServiceManager
	{
		public User GetUserById(int id)
		{
			return _serviceAccess.GetUserById(id).FirstOrDefault();
		}

		public List<User> GetUsersByUsername(List<string> usernames, bool exactResults)
		{
			if(!exactResults)
				usernames.ForEach(name => name = $"%{name}%");

			return _serviceAccess.GetUsersByUsername(usernames);
		}

		/// <summary>
		/// Checks the database if the given <paramref name="username"/> and <paramref name="password"/> pair was found or not. Throws exceptions on errors.
		/// </summary>
		/// <param name="username">user's username</param>
		/// <param name="password">user's password in plain text format</param>
		/// <exception cref="ManagerException">on any user error</exception>
		public User LoginUser(string username, string password)
		{
			User user = _serviceAccess.GetUsersByUsername(username.ToList()).First();

			if (user == null)
				throw new ManagerException(ManagerError.LoginUserObjectWasNull);

			int pwdExpDays = Convert.ToInt32(_serviceAccess.GetSetting(SystemSettingKeys.PasswordExpirationDays));
			
			string hashedPassword = PasswordHasher.HashPassword(password);
			List<UserPassword> passwords = _serviceAccess.GetUserPasswords(user)
											.OrderByDescending(up => up.CreationDate).ToList();

			foreach (UserPassword pwd in passwords)
			{
				if (pwd.Password != hashedPassword)
					continue;

				if (pwd.CreationDate.AddDays(pwdExpDays) >= DateTime.Now)
					return user;

				throw new ManagerException(ManagerError.LoginPasswordExpired);
			}

			throw new ManagerException(ManagerError.LoginInvalidCredentials);
		}
	}
}