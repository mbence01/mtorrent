using System;

namespace Torrent.Common.Extension
{
	public static class DateTimeExtension
	{
		/// <summary>
		/// Converts a <see cref="DateTime"/> object into a database compatible formatted string.
		/// </summary>
		/// <param name="date">DateTime object</param>
		/// <returns>String in a correct format.</returns>
		public static string DBFormat(this DateTime date)
		{
			return date.ToString("yyyy-MM-dd HH:mm:ss");
		}
	}
}