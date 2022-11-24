using System;

namespace Torrent.Common.Extension
{	
	public static class ObjectExtension
	{
		/// <summary>
		/// Throws an <see cref="ArgumentException"/> if the given variable is null.
		/// </summary>
		/// <param name="value">The given variable.</param>
		/// <exception cref="ArgumentException"></exception>
		public static void CannotBeNull(this object value)
		{
			if (value == null)
				throw new ArgumentException($"Variable cannot be null.");
		}
		
		/// <summary>
		/// Throws an <see cref="ArgumentException"/> if the given variable is not null.
		/// </summary>
		/// <param name="value">The given variable.</param>
		/// <exception cref="ArgumentException"></exception>
		public static void MustBeNull(this object value)
		{
			if (value != null)
				throw new ArgumentException($"Variable must be null.");
		}

		/// <summary>
		/// Determines if the given value is equal to null.
		/// </summary>
		/// <param name="value">The given value.</param>
		/// <returns>True if the value was null, otherwise false.</returns>
		public static bool IsNull(this object value)
		{
			return value == null;
		}

		/// <summary>
		/// Determines if the given value is not null.
		/// </summary>
		/// <param name="value">The given value.</param>
		/// <returns>True if the value was not null, otherwise false.</returns>
		public static bool IsNotNull(this object value)
		{
			return value != null;
		}

		/// <summary>
		/// Converts the given <param name="value"></param> to a database compatible formatted string.
		/// </summary>
		/// <param name="value">The given value</param>
		/// <returns>Database compatible string.</returns>
		public static string AsColumn(this object value)
		{
			if (value == null)
				return "null";
			
			if (value is DateTime)
				return $"'{Convert.ToDateTime(value).ToString("yyyy-MM-dd HH:mm:ss")}'";

			return $"'{value.ToString()}'";
		}
	}
}