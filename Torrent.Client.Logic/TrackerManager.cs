using System;
using Torrent.Client.Communication;
using Torrent.Client.Logic.RequestBuilder;
using Torrent.Client.Model.Communication.Request;
using Torrent.Client.Model.Communication.Response;
using Torrent.Client.Model.DotTorrent;
using Torrent.Client.Model.Interface;

namespace Torrent.Client.Logic
{
    public class TrackerManager
    {
        #region Private variables
        private readonly Config _configuration;
        private readonly TorrentFile _torrent;
        private readonly ITrackerCommunication _trackerCommunication;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new <see cref="TrackerManager"/> instance and sets the value of the tracker URL
        /// </summary>
        /// <param name="torrent">Parsed torrent file</param>
        /// <param name="config">Application settings used</param>
        public TrackerManager(TorrentFile torrent, Config config)
        {
            _configuration = config;
            _torrent = torrent;
            _trackerCommunication = new HttpTrackerCommunication();
        }
        #endregion

        public AnnounceResponse SendAnnounce(string peerId)
        {
            AnnounceRequest requestMessage = new AnnounceRequestBuilder().BuildAnnounceRequest(_torrent, peerId, _configuration.StartPort);
            AnnounceResponse response = _trackerCommunication.SendAnnounce(requestMessage);

            response.InfoHash = requestMessage.InfoHash;

            return response;
        }

        public void SendScrape()
        {
            ScrapeRequest requestMessage = new ScrapeRequestBuilder().BuildScrapeRequest(_torrent);
            _trackerCommunication.SendScrape(requestMessage);
        }
    }
}
