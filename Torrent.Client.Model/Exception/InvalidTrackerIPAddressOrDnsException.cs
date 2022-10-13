namespace Torrent.Client.Model.Exception
{
    /// <summary>
    /// Exception class for situations when a parser tried to convert tracker host to IP address but it failed.
    /// </summary>
    public class InvalidTrackerIPAddressOrDnsException : System.Exception
    {
        /// <summary>
        /// Initializes an exception class for situations when a parser tried to convert tracker host to IP address but it failed.
        /// </summary>
        public InvalidTrackerIPAddressOrDnsException(string hostname, System.Exception innerException)
            : base($"Failed to convert '{hostname}' to IPAddress. See the inner exception for more information", innerException) { }
    }
}
