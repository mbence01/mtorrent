using System;
using System.Collections.Generic;

namespace Torrent.Client.Model.Communication.Response
{
    /// <summary>
    /// Represents an announce message from the tracket
    /// </summary>
    public class AnnounceResponse : BaseResponse
    {
        public int Complete { get; set; }
        public int Incomplete { get; set; }
        public int Interval { get; set; }
        public int MinimumInterval { get; set; }
        public List<Peer> Peers { get; set; }
    }
}
