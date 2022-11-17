namespace Torrent.Service.Access;

/// <summary>
/// Contains definitions for classes in Service.Access, for example connection string and attempt count.
/// </summary>
public static class AccessConfig
{
	/// <summary>
	/// Connection string of the database the program has to connect to.
	/// </summary>
	public const string ConnectionString = "Server=localhost\\SQLEXPRESS;Database=TorrentDB;Trusted_Connection=True;";
	
	/// <summary>
	/// Maximum connection attempts.
	/// </summary>
	public const int MaxAttempts = 5;
}