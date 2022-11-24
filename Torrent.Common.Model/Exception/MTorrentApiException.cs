using System;

namespace Torrent.Common.Model.Exception
{
	/// <summary>
	/// Exception class for situations when an unexpected error has occurred during the run of API.
	/// </summary>
	public class MTorrentApiException : System.Exception
	{
		/// <summary>
		/// Initializes an exception class for situations when an unexpected error has occurred during the run of API.
		/// </summary>
		public MTorrentApiException(string endpointName, string methodName, string message) : base($"Unexpected API error has occurred on {endpointName}/{methodName} endpoint. {message}") {}
	}
}