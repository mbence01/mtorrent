using System;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using Torrent.Client.Logic.Common;
using Torrent.Client.Model;
using Torrent.Client.Model.Communication.Request;
using static System.Net.Mime.MediaTypeNames;

namespace Torrent.Client.Logic.RequestBuilder
{
    public class HandshakeRequestBuilder
    {
        public static PeerHandshakeRequest BuildHandshakeRequest(Peer peer, string infoHash, string peerId)
        {
            byte[] hash = new byte[20];

            using (SHA1 sha1 = SHA1.Create())
                hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(infoHash));

            return new PeerHandshakeRequest
            {
                InfoHash = hash,
                PeerId = peerId,
                Uri = new UriBuilder("tcp", peer.Ip, peer.Port).Uri
            };
        }
    }
}
