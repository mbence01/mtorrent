using System.Collections.Generic;

namespace Torrent.Client.Model.DotTorrent
{
    /// <summary>
    /// Describes the value of "info" key in a .torrent file (e.g. pieces, title, length)
    /// TorrentFile > Info
    /// </summary>
    public class TorrentInfo
    {
        /// <summary>
        /// Files that the .torrent file contains
        /// </summary>
        public List<TorrentPiece> Files { get; set; }
        /// <summary>
        /// Name of the torrent
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Size of file
        /// </summary>
        public long Length { get; set; }
        /// <summary>
        /// Size of the torrent
        /// </summary>
        public long PieceLength { get; set; }
        /// <summary>
        /// A byte array that is in the info
        /// </summary>
        public byte[] Pieces { get; set; }
        /// <summary>
        /// Private torrent or not
        /// </summary>
        public bool Private { get; set; }
        /// <summary>
        /// The source where the torrent was downloaded from
        /// </summary>
        public string Source { get; set; }
    }
}
