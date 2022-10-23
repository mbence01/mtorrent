using System.Collections.Generic;

namespace Torrent.Client.Model.DotTorrent
{
    /// <summary>
    /// Describes the value of an element of "files" key list in a .torrent file
    /// TorrentFile > Info > Files List
    /// </summary>
    public class TorrentPiece
    {
        /// <summary>
        /// The length of the piece
        /// </summary>
        public long Length { get; set; }
        /// <summary>
        /// Path to the piece to place (subdirectories will be placed in a list e.g. { "A-subdir", "B-subdir", "file" })
        /// </summary>
        public List<string> Path { get; set; }
    }
}
