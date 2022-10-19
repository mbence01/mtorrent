using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Torrent.Client.Model.DotTorrent;

namespace Torrent.Client.Model.Interface
{
    public interface ITorrentFileParser
    {
        #region Properties
        /// <summary>
        /// The AppSettings that should be passed in the constructor
        /// </summary>
        System.Collections.Specialized.NameValueCollection AppSettings { get; set; }
        #endregion

        #region Functions that must be declared in the derived classes
        /// <summary>
        /// Parses the given .torrent file and creates an instance of <see cref="TorrentFile"/> with properties from the file.
        /// </summary>
        /// <param name="fileContent">Content of the .torrent file</param>
        /// <returns>A <see cref="TorrentFile"/> instance</returns>
        TorrentFile ParseTorrent(string fileContent);
        #endregion
    }
}
