using Torrent.Common.Model;
using Torrent.Service.Access;
using Torrent.Service.Access.Common;

namespace Torrent.Service.Logic
{
	public partial class ServiceManager
	{
		public List<User> GetUserById(int id)
		{
			return _serviceAccess.GetUserById(id);
		}
	}
}