namespace Torrent.Service.Access
{
	/// <summary>
	/// The database layer of the service.
	/// </summary>
	public partial class ServiceAccess
	{
		private readonly string _connectionString;
		private readonly int _maxConnectionAttempts;

		public ServiceAccess(string connectionString, int maxConnectionAttempts)
		{
			this._connectionString = connectionString;
			this._maxConnectionAttempts = maxConnectionAttempts;

			AccessConfig.ConnectionString = connectionString;
			AccessConfig.MaxAttempts = maxConnectionAttempts;
		}
	}
}