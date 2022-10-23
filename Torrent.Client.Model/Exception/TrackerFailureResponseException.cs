namespace Torrent.Client.Model.Exception
{
    /// <summary>
    /// Exception class for situations when a request has been sent to the tracker and it responded with a failure reason.
    /// </summary>
    public class TrackerFailureResponseException : System.Exception
    {
        /// <summary>
        /// Initializes an exception class for situations when a request has been sent to the tracker and it responded with a failure reason.
        /// </summary>
        public TrackerFailureResponseException(string failureReason, string requestType)
            : base($"The tracker server responded with a failure reason '{failureReason}' while trying to send {requestType} request.") { }
    }
}
