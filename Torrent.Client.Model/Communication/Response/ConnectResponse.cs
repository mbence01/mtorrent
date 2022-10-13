using System;
using System.Collections.Generic;

namespace Torrent.Client.Model.Communication.Response
{
    /// <summary>
    /// Represents a response message from the tracket
    /// </summary>
    public class ConnectResponse : BaseResponse
    {
        public Int32 Action { get; set; } = 0;
        public Int32 TransactionId { get; set; }
        public Int64 ConnectionId { get; set; }
    }
}
