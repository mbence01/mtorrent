using System.Collections.Generic;
using System.Linq;

namespace Torrent.Common.Extension
{
	public static class EnumerableExtension
	{
		public static bool HasItems<T>(this IEnumerable<T> collection)
		{
			return collection != null && collection.Any();
		}
	}
}