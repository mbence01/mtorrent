using System;

namespace Torrent.Client.Model.Communication.Request
{
    /// <summary>
    /// Represents a scrape message sent to the tracker
    /// </summary>
    public class ScrapeRequest : BaseRequest
    {
        public string InfoHash { get; set; }
    }
}
