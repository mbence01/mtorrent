using Torrent.Client.Logic.Parser;
using Torrent.Client.Model.Interface;
using Torrent.Client.Model.Interface.Main;

namespace Torrent.Client.Main
{
    public class Config : IConfig
    {
        public ITorrentFileParser Parser { get; set; } = new TorrentFileParser();
    }
}
