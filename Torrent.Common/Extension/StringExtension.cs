using System.Collections.Generic;

namespace Torrent.Common.Extension
{
	public static class StringExtension
	{
		public static List<string> ToList(this string value)
			=> new List<string> { value };
	}
}