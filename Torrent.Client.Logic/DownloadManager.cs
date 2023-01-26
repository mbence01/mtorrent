using Torrent.Client.Communication;
using Torrent.Client.Logic.RequestBuilder;
using Torrent.Client.Model;
using Torrent.Client.Model.Communication.Request;
using Torrent.Client.Model.DotTorrent;
using Torrent.Client.Model.Interface;

namespace Torrent.Client.Logic
{
    public class DownloadManager
    {
        #region Private variables
        private readonly Config _configuration;
        private readonly PeerCommunication _peerCommunication;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new <see cref="DownloadManager"/> instance and sets the required variables' values
        /// </summary>
        /// <param name="config">Application settings used</param>
        public DownloadManager(Config config)
        {
            _configuration = config;
            _peerCommunication = new PeerCommunication();
        }
        #endregion

        public string SendHandshake(Peer peer, string infoHash, string peerId)
        {
            PeerHandshakeRequest handshakeRequest = HandshakeRequestBuilder.BuildHandshakeRequest(peer, infoHash, peerId);

            return _peerCommunication.SendHandshake(handshakeRequest);
        }
    }
}
