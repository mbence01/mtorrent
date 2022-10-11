using System;
using System.Text;

namespace Torrent.Client.Model.DotTorrent
{
    /// <summary>
    /// Model class which accurately describes the content of a file with .torrent extension
    /// </summary>
    public class TorrentFile
    {
        /// <summary>
        /// The tracker server URL
        /// </summary>
        public string Announce { get; set; }
        /// <summary>
        /// The program which generated the torrent file
        /// </summary>
        public string CreatedBy { get; set; }
        /// <summary>
        /// Date and time when the file was generated
        /// </summary>
        public DateTime CreationDate { get; set; }
        /// <summary>
        /// Encoding used by the file
        /// </summary>
        public Encoding Encoding { get; set; }
        /// <summary>
        /// Information about the torrent itself
        /// </summary>
        public TorrentInfo Info { get; set; }
    }
}
