using System;

namespace Torrent.Common.Model.Exception
{
	/// <summary>
	/// Exception class for situations when a user tries to reach one of the API emdpoints without successfully authorized itself before.
	/// </summary>
	public class UnauthorizedException : System.Exception
	{
		/// <summary>
		/// Initializes an exception class for situations when a user tries to reach one of the API emdpoints without successfully authorized itself before.
		/// </summary>
		public UnauthorizedException() : base("User was not authorized. Invalid or expired API authorization token given.") {}
	}
}