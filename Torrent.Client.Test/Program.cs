using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Torrent.Client.Logic;
using Torrent.Client.Logic.Parser;
using Torrent.Client.Model.DotTorrent;
using Torrent.Client.Model.Interface;

namespace Torrent.Client.Main
{
    internal class Program
    {
        private static string _peerId;
        private static System.Collections.Specialized.NameValueCollection _appSettings;
        private static ITorrentFileParser _fileParser;

        static void Main(string[] args)
        {
            InitializeComponents();

            TorrentFile torrent = _fileParser.ParseTorrentFile(@"C:\Users\Acer Ultrabook\Desktop\a.torrent");
            TrackerManager manager = new TrackerManager(torrent, _appSettings);

            //manager.ConnectToTracker();
            //manager.SendAnnounce(_peerId);
            manager.SendScrape();
            manager.SendAnnounce(_peerId);
        }

        private static void InitializeComponents()
        {
            _appSettings = ConfigurationManager.AppSettings;
            _fileParser = new TorrentFileParser(_appSettings);

            Random rand = new Random();
            _peerId = new string(Enumerable.Range(0, 20).Select(_ => (char)('A' + rand.Next(25))).ToArray()); // Generates a random 20 length string for peer id
        }
    }
}
