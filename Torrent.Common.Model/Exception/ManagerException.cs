using System;
using Torrent.Common.Model.Enum;

namespace Torrent.Common.Model.Exception
{
	/// <summary>
	/// Exception class for situations when a user error occurs in the manager layer. This exception will be thrown to the API layer that handles the error code given in parameter.
	/// </summary>
	public class ManagerException : System.Exception
	{
		public ManagerError ErrorCode { get; private set;  }

		/// <summary>
		/// Initializes an exception class for situations when a user error occurs in the manager layer. This exception will be thrown to the API layer that handles the error code given in parameter.
		/// </summary>
		public ManagerException(ManagerError code) : 
			base("User error has occurred in the manager layer.")
		{
			ErrorCode = code;
		}
	}
}