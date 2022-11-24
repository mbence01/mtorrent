using Torrent.Service.Access;

namespace Torrent.Service.Logic
{
	/// <summary>
	/// The business logic layer of the service.
	/// </summary>
	public partial class ServiceManager
	{
		private readonly ServiceAccess _serviceAccess;

		public ServiceManager(string connectionString, int maxConnectionAttempts)
		{
			this._serviceAccess = new ServiceAccess(connectionString, maxConnectionAttempts);
		}
	}
}