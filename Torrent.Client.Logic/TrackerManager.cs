using Torrent.Client.Communication;
using Torrent.Client.Logic.RequestBuilder;
using Torrent.Client.Model.Communication.Request;

namespace Torrent.Client.Logic
{
    public class TrackerManager
    {
        #region Private variables
        private readonly string _trackerUrl;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new <see cref="TrackerManager"/> instance and sets the value of the tracker URL
        /// </summary>
        /// <param name="trackerUrl">Tracker URL</param>
        public TrackerManager(string trackerUrl)
        {
            _trackerUrl = trackerUrl;
        }
        #endregion

        public void ConnectToTracker()
        {
            ConnectRequestBuilder builder = new ConnectRequestBuilder();
            TrackerCommunication communication = new TrackerCommunication();

            ConnectRequest requestMessage = builder.BuildConnectRequest();
        }
    }
}
