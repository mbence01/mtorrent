using Torrent.Client.Model.Communication.Request;
using Torrent.Client.Model.Communication.Response;

namespace Torrent.Client.Model.Interface
{
    public interface ITrackerCommunication
    {
        ConnectResponse ConnectToTracker(ConnectRequest request);
        AnnounceResponse SendAnnounce(AnnounceRequest request);
        ScrapeResponse SendScrape(ScrapeRequest request);
    }
}
