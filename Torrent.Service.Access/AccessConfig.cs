namespace Torrent.Service.Access;

/// <summary>
/// Contains definitions for classes in Service.Access, for example connection string and attempt count.
/// </summary>
public static class AccessConfig
{
	/// <summary>
	/// Connection string of the database the program has to connect to.
	/// </summary>
	public static string? ConnectionString { get; set; }
	
	/// <summary>
	/// Maximum connection attempts.
	/// </summary>
	public static int MaxAttempts { get; set; }
}