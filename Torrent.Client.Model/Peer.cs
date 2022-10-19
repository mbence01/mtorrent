namespace Torrent.Client.Model
{
    /// <summary>
    /// Represents a peer
    /// </summary>
    public class Peer
    {
        public int Id { get; set; }
        public string Ip { get; set; }
        public int Port { get; set; }
    }
}
