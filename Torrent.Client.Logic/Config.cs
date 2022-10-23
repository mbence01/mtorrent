using Torrent.Client.Logic.Parser;
using Torrent.Client.Model.Interface;

namespace Torrent.Client.Logic
{
    public class Config
    {
        public ITorrentFileParser Parser { get; set; } = new TorrentFileParser();
        public int StartPort { get; set; } = 6881;
    }
}
