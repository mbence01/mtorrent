using System.Configuration;
using Torrent.Client.Logic.Parser;

namespace Torrent.Client.Main
{
    internal class Program
    {
        private static System.Collections.Specialized.NameValueCollection _appSettings;

        static void Main(string[] args)
        {
            InitializeComponents();

            TorrentFileParser parser = new TorrentFileParser(_appSettings);

            //Model.DotTorrent.TorrentFile file = parser.ParseTorrentFile(@"C:\xampp\htdocs\x.torrent");
            Model.DotTorrent.TorrentFile file = parser.ParseTorrentFile(@"C:\Users\Acer Ultrabook\Desktop\r.torrent");
        }

        private static void InitializeComponents()
        {
            _appSettings = ConfigurationManager.AppSettings;
        }
    }
}
