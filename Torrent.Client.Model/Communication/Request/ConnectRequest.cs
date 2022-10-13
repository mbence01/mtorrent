using System;

namespace Torrent.Client.Model.Communication.Request
{
    /// <summary>
    /// Represents a request message sent to the tracker
    /// </summary>
    public class ConnectRequest : BaseRequest
    {
        public Int64 ProtocolId { get; set; }
        public Int32 Action { get; set; }
        public Int32 TransactionId { get; set; }
    }
}
