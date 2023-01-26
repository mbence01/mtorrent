namespace Torrent.Client.Model.Communication.Request
{
    public class PeerHandshakeRequest : BaseRequest
    {
        public int Pstrlen { get; set; } = 19;
        public string Pstr { get; set; } = "BitTorrent protocol";
        public byte[] Reserved { get; set; } = new byte[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
        public byte[] InfoHash { get; set; }
        public string PeerId { get; set; }
    }
}
