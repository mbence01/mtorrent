namespace Torrent.Common.Logging
{
	public enum FolderDepth
	{
		FirstType,
		FirstDate
	}

	/// <summary>
	/// Config file that the Logger object will use.
	/// </summary>
	public static class LogConfig
	{
		/// <summary>
		/// Determines if the Logger is enabled or not.
		/// </summary>
		public static bool LoggerEnabled = true;

		/// <summary>
		/// Log files will be created to this folder. Default: Logs.
		/// </summary>
		public static string LogBaseDir = "Logs";

		/// <summary>
		/// Create different directories to different log types or place them in a single file.
		/// </summary>
		public static bool SeparateDifferentLogTypes = true;

		/// <summary>
		/// Available formats:<br />
		/// FirstType: LogBaseDir will look like this: Logs/Error/2022, Logs/Warning/2023<br />
		/// FirstDate: LogBaseDir will look like this: Logs/2022/Error, Logs/2023/Warning
		/// </summary>
		public static FolderDepth SeparateLogTypesFolderDepth = FolderDepth.FirstType;

		/// <summary>
		/// Log files will be placed in different directories based on a date and the below given format.<br />
		/// For example: - yyyy-MM-dd will create a directory structure something like this: Logs/2022/01/01<br />
		///              - yyyy-MMdd will create a directory structure something like this: Logs/2022/0101
		/// </summary>
		public static string DateFolderFormat = "yyyy-MM";

		/// <summary>
		/// Log files' name will be formatted like this.<br />
		/// ddd = day (01, 10, 30), ttt = log type (error, warning, info)
		/// </summary>
		public static string LogFilenameFormat = "ddd-ttt.txt";

		/// <summary>
		/// Format of timestamp written in log files. Default: HH:mm:ss
		/// </summary>
		public static string TimestampFormat = "HH:mm:ss";
	}	
}