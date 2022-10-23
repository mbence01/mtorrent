using System;

namespace Torrent.Client.Model.Communication.Request
{
    /// <summary>
    /// Represents an announce message sent to the tracker
    /// </summary>
    public class AnnounceRequest : BaseRequest
    {
        public string InfoHash { get; set; }
        public string PeerId { get; set; }
        public string Ip { get; set; } = "";
        public int Port { get; set; }
        public long Uploaded { get; set; }
        public long Downloaded { get; set; }
        public long Left { get; set; }
        public int Compact { get; set; } = 1;
        public string Event { get; set; }
        public int NumWant { get; set; } = 50;
    }
}
