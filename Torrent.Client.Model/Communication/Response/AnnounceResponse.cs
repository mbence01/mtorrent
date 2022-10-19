using System;
using System.Collections.Generic;

namespace Torrent.Client.Model.Communication.Response
{
    /// <summary>
    /// Represents an announce message from the tracket
    /// </summary>
    public class AnnounceResponse : BaseResponse
    {
        public int Interval { get; set; }
        public List<Peer> Peers { get; set; }
    }
}
