using System;

namespace Torrent.Common.Extension
{
	public static class IntegerExtension
	{
		/// <summary>
		/// Check if <paramref name="checkNum"/> is greater than <paramref name="num"/>
		/// </summary>
		/// <param name="checkNum">Number that should be greater than the other.</param>
		/// <param name="num">The other number.</param>
		/// <exception cref="System.ArgumentException"></exception>
		public static void MustBeGreaterThan(this int checkNum, int num)
		{
			if (checkNum <= num)
				throw new ArgumentException($"checkNum ({checkNum}) must be greater than num ({num}).");
		}
		
		/// <summary>
		/// Check if <paramref name="checkNum"/> is greater than or equal <paramref name="num"/>
		/// </summary>
		/// <param name="checkNum">Number that should be greater than or equal to the other.</param>
		/// <param name="num">The other number.</param>
		/// <exception cref="System.ArgumentException"></exception>
		public static void MustBeGreaterThanOrEqual(this int checkNum, int num)
		{
			if (checkNum < num)
				throw new ArgumentException($"checkNum ({checkNum}) must be greater than or equal to num ({num}).");
		}
		
		/// <summary>
		/// Check if <paramref name="checkNum"/> is less than <paramref name="num"/>
		/// </summary>
		/// <param name="checkNum">Number that should be less than the other.</param>
		/// <param name="num">The other number.</param>
		/// <exception cref="System.ArgumentException"></exception>
		public static void MustBeLessThan(this int checkNum, int num)
		{
			if (checkNum >= num)
				throw new ArgumentException($"checkNum ({checkNum}) must be less than num ({num}).");
		}
		
		/// <summary>
		/// Check if <paramref name="checkNum"/> is less than or equal <paramref name="num"/>
		/// </summary>
		/// <param name="checkNum">Number that should be less than or equal to the other.</param>
		/// <param name="num">The other number.</param>
		/// <exception cref="System.ArgumentException"></exception>
		public static void MustBeLessThanOrEqual(this int checkNum, int num)
		{
			if (checkNum > num)
				throw new ArgumentException($"checkNum ({checkNum}) must be less than or equal to num ({num}).");
		}
	}	
}