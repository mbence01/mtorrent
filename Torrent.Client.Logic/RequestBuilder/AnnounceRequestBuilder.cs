using Torrent.Client.Logic.Common;
using Torrent.Client.Model.Communication.Request;
using Torrent.Client.Model.DotTorrent;

namespace Torrent.Client.Logic.RequestBuilder
{
    public class AnnounceRequestBuilder
    {
        public AnnounceRequest BuildAnnounceRequest(TorrentFile torrent, string peerId, int port) => BuildAnnounceRequest(torrent, peerId, port, null, 50, 1);
        public AnnounceRequest BuildAnnounceRequest(TorrentFile torrent, string peerId, int port, string ip) => BuildAnnounceRequest(torrent, peerId, port, ip, 50, 1);
        public AnnounceRequest BuildAnnounceRequest(TorrentFile torrent, string peerId, int port, string ip, int urlCount) => BuildAnnounceRequest(torrent, peerId, port, ip, urlCount, 1);
        public AnnounceRequest BuildAnnounceRequest(TorrentFile torrent, string peerId, int port, string ip, int urlCount, int compact)
        {
            return new AnnounceRequest
            {
                InfoHash = SHA1Hasher.CreateHash(torrent.BencodedDictionary["info"]),
                PeerId = peerId,
                Ip = ip,
                Port = port,
                Uploaded = 0,
                Downloaded = 0,
                Left = torrent.Info.PieceLength,
                Compact = compact,
                Event = "started",
                NumWant = urlCount,
                Uri = torrent.Announce
            };
        }
    }
}
