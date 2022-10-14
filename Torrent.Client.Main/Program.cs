using System.Configuration;
using Torrent.Client.Logic;
using Torrent.Client.Logic.Parser;
using Torrent.Client.Model.DotTorrent;
using Torrent.Client.Model.Interface;

namespace Torrent.Client.Main
{
    internal class Program
    {
        private static System.Collections.Specialized.NameValueCollection _appSettings;
        private static ITorrentFileParser _fileParser;

        static void Main(string[] args)
        {
            InitializeComponents();

            TorrentFile torrent = _fileParser.ParseTorrentFile(@"C:\Users\Acer Ultrabook\Desktop\r.torrent");

            string trackerServerUrl = torrent.Announce;

            TrackerManager manager = new TrackerManager(trackerServerUrl);
            manager.ConnectToTracker();
        }

        private static void InitializeComponents()
        {
            _appSettings = ConfigurationManager.AppSettings;
            _fileParser = new TorrentFileParser(_appSettings);
        }
    }
}
