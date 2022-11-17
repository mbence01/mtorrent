using System;

namespace Torrent.Common.Model.Exception
{
    /// <summary>
    /// Exception class which occurs usually when the Logger class tries to write a log file but it fails.
    /// </summary>
    public class LoggerException : System.Exception
    {
        /// <summary>
        /// Initializes a new <see cref="LoggerException"/> exception class.
        /// </summary>
        /// <param name="path">The path which the Logger tried to read/write</param>
        /// <param name="innerException">inner exception</param>
        public LoggerException(string path, System.Exception innerException) 
            : base($"An error has occurred when trying to use Logger. " +
                   $"Check if '{path}' exists and Logger has enough privileges to read and write this resource." +
                   $"If the problem keeps occurring, try to disable the Logger.", innerException) {}

        /// <summary>
        /// Initializes a new <see cref="LoggerException"/> exception class.
        /// </summary>
        public LoggerException()
            : base($"An error has occurred when trying to use Logger. Probably you forgot to enable Logger in the configuration file.") { }
    }
}
