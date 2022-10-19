using System;
using System.Collections.Generic;

namespace Torrent.Client.Model.Communication.Response
{
    /// <summary>
    /// Represents a scrape message from the tracket
    /// </summary>
    public class ScrapeResponse : BaseResponse
    {
        public int Interval { get; set; }
        public List<Peer> Peers { get; set; }
    }
}
