using Torrent.Service.Access.Common;

namespace Torrent.Service.Access
{
	/// <summary>
	/// The database layer of the service.
	/// </summary>
	public partial class ServiceAccess
	{
		private readonly string _connectionString;

		public ServiceAccess()
		{
			this._connectionString = AccessConfig.ConnectionString;
		}
	}
}