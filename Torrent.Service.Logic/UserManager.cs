using Torrent.Common.Model;
using Torrent.Service.Access;
using Torrent.Service.Access.Common;

namespace Torrent.Service.Logic
{
	public partial class ServiceManager
	{
		public User TestUserInsert(User user)
		{
			return user.Insert();
		}
	}
}