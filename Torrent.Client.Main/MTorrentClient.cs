using System.Collections.Generic;
using Torrent.Client.Model.DotTorrent;
using Torrent.Client.Model.Interface.Main;

namespace Torrent.Client.Main
{
    /// <summary>
    /// The main entry point of the torrent client
    /// </summary>
    public class MTorrentClient
    {
        private string _peerId;
        private string _fileContent;
        private TorrentFile _parsedTorrent;
        private IConfig _configuration;

        public MTorrentClient(string fileContent)
        {
            this._fileContent = fileContent;
            this._peerId = "";
        }

        public void ConfigureClient(IConfig config)
        {
            this._configuration = config;
        }

        public void StartTorrent()
        {
            CheckConfig();

            this._parsedTorrent = this._configuration.Parser.ParseTorrent(this._fileContent);
        }

        private void CheckConfig()
        {
            if (this._configuration == null)
                this.UseDefaultConfig();
        }

        private void UseDefaultConfig()
        {
            this._configuration = new Config();
        }
    }
}
