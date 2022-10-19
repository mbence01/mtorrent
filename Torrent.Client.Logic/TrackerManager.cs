using System;
using Torrent.Client.Communication;
using Torrent.Client.Logic.RequestBuilder;
using Torrent.Client.Model.Communication.Request;
using Torrent.Client.Model.DotTorrent;
using Torrent.Client.Model.Interface;

namespace Torrent.Client.Logic
{
    public class TrackerManager
    {
        #region Private variables
        private static System.Collections.Specialized.NameValueCollection _appSettings;
        private readonly TorrentFile _torrent;
        private readonly ITrackerCommunication _trackerCommunication;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new <see cref="TrackerManager"/> instance and sets the value of the tracker URL
        /// </summary>
        /// <param name="torrent">Parsed torrent file</param>
        /// <param name="appSettings">Application settings used</param>
        public TrackerManager(TorrentFile torrent, System.Collections.Specialized.NameValueCollection appSettings)
        {
            _appSettings = appSettings;
            _torrent = torrent;
            _trackerCommunication = new HttpTrackerCommunication();
        }
        #endregion

        public void ConnectToTracker()
        {
            //ConnectRequest requestMessage = new ConnectRequestBuilder().BuildConnectRequest(_trackerUrl);
            //_trackerCommunication.ConnectToTracker(requestMessage);
        }

        public void SendAnnounce(string peerId)
        {
            AnnounceRequest requestMessage = new AnnounceRequestBuilder().BuildAnnounceRequest(_torrent.Announce.ToString(),
                                                _torrent.BencodedDictionary[_appSettings["PropInformation"]],
                                                peerId,
                                                Convert.ToInt32(_appSettings["StartPort"]),
                                                _torrent.Info.Length,
                                                _appSettings["EventStatusStarted"]);

            _trackerCommunication.SendAnnounce(requestMessage);
        }

        public void SendScrape()
        {
            ScrapeRequest requestMessage = new ScrapeRequestBuilder().BuildScrapeRequest(_torrent.Announce.ToString(), _torrent.BencodedDictionary[_appSettings["PropInformation"]]);
            _trackerCommunication.SendScrape(requestMessage);
        }
    }
}
