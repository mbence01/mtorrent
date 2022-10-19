namespace Torrent.Client.Model.Exception
{
    /// <summary>
    /// Exception class for situations when the user gives a file path which does not exist or code has not enough permissions to read the file.<br></br>
    /// This exception is going to be thrown when one of the following exceptions has already been thrown:<br></br>
    /// <see cref="System.IO.FileNotFoundException" />, 
    /// <see cref="System.IO.DirectoryNotFoundException" />, 
    /// <see cref="System.IO.IOException" />, 
    /// <see cref="System.UnauthorizedAccessException" />
    /// </summary>
    public class TorrentFileOpenException : System.Exception
    {
        /// <summary>
        /// Initializes an exception class for situations when the user gives a file path which does not exist or code has not enough permissions to read the file.<br></br>
        /// This exception is going to be thrown when one of the following exceptions has already been thrown:<br></br>
        /// <see cref="System.IO.FileNotFoundException" />, 
        /// <see cref="System.IO.DirectoryNotFoundException" />, 
        /// <see cref="System.IO.IOException" />, 
        /// <see cref="System.UnauthorizedAccessException" />
        /// </summary>
        public TorrentFileOpenException(string content, System.Exception innerException) 
            : base($"Cannot read bencoded data: '{content}', while trying to parse into a dictionary", innerException) { }
    }
}
