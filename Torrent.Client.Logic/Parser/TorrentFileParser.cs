using BencodeNET.Objects;
using BencodeNET.Parsing;
using BencodeNET.Torrents;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Torrent.Client.Model.DotTorrent;
using Torrent.Client.Model.Exception;

namespace Torrent.Client.Logic.Parser
{
    public class TorrentFileParser
    {
        #region Properties
        /// <summary>
        /// The AppSettings passed in the constructor
        /// </summary>
        public System.Collections.Specialized.NameValueCollection AppSettings { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Creates an instance of the <see cref="TorrentFileParser"/> and set the AppSettings to <paramref name="appSettings"/>
        /// </summary>
        /// <param name="appSettings"></param>
        public TorrentFileParser(System.Collections.Specialized.NameValueCollection appSettings)
        {
            AppSettings = appSettings;
        }
        #endregion

        public TorrentFile ParseTorrentFile(string path)
        {
            BencodeNET.Torrents.Torrent t = new BencodeNET.Torrents.Torrent();

            try
            {
                using(Stream stream = new FileStream(path, FileMode.Open))
                {
                    BencodeParser parser = new BencodeParser();
                    t = parser.Parse<BencodeNET.Torrents.Torrent>(stream);
                    BDictionary parsedTorrentFileAsDictionary = parser.Parse<BDictionary>(stream);

                    return GenerateModelForTorrentFile(parsedTorrentFileAsDictionary);
                }
            }
            catch(Exception ex) when (ex is FileNotFoundException || 
                                      ex is DirectoryNotFoundException ||
                                      ex is IOException ||
                                      ex is UnauthorizedAccessException)
            {
                throw new TorrentFileOpenException(path, ex);
            }
        }

        private TorrentFile GenerateModelForTorrentFile(BDictionary properties)
        {
            TorrentFile torrent = new TorrentFile();

            try
            {
                torrent.Announce        = properties.Get<BString>(AppSettings["PropAnnounce"]).ToString();
                torrent.CreatedBy       = properties.Get<BString>(AppSettings["PropCreatedBy"]).ToString();
                torrent.CreationDate    = new DateTime(properties.Get<BNumber>(AppSettings["PropCreationDate"]));
                torrent.Encoding        = System.Text.Encoding.GetEncoding(properties.Get<BString>(AppSettings["PropEncoding"]).ToString());
                torrent.Info            = GenerateModelForTorrentInfo(properties.Get<BDictionary>(AppSettings["PropInformation"]));
            }
            catch(Exception ex)
            {
                throw new DictionaryToModelConversionFailedException(torrent, ex);
            }
            
            return torrent;
        }

        private TorrentInfo GenerateModelForTorrentInfo(BDictionary properties)
        {
            TorrentInfo info = new TorrentInfo();

            try
            {
                info.Files = GenerateModelForTorrentFilePiecesList(properties.Get<BList>(AppSettings["PropInfoFiles"]));
                info.Name = properties.Get<BString>(AppSettings["PropInfoName"]).ToString();
                info.PieceLength = properties.Get<BNumber>(AppSettings["PropInfoPieceLength"]);
                info.Private = Convert.ToBoolean(Convert.ToInt32(properties.Get<BNumber>(AppSettings["PropInfoPrivate"]).ToString()));
                info.Source = properties.Get<BString>(AppSettings["PropInfoSource"]).ToString();
            }
            catch(Exception ex)
            {
                throw new DictionaryToModelConversionFailedException(info, ex);
            }

            return info;
        }

        private List<TorrentPiece> GenerateModelForTorrentFilePiecesList(BList fileProps)
        {
            List<TorrentPiece> torrentPieces = new List<TorrentPiece>();
            TorrentPiece currPiece = null;

            try
            {
                foreach(BDictionary prop in fileProps)
                {
                    currPiece = GenerateModelForTorrentFilePiece(prop);
                    torrentPieces.Add(currPiece);
                }
            }
            catch(Exception ex)
            {
                throw new DictionaryToModelConversionFailedException(currPiece, ex);
            }

            return torrentPieces;
        }

        private TorrentPiece GenerateModelForTorrentFilePiece(BDictionary properties)
        {
            TorrentPiece piece = new TorrentPiece();

            try
            {
                piece.Length    = properties.Get<BNumber>(AppSettings["PropPieceLength"]);
                piece.Path      = properties.Get<BList>(AppSettings["PropPiecePath"])
                                            .ToList()
                                            .Select(e => e.ToString())
                                            .ToList();
            }
            catch(Exception ex)
            {
                throw new DictionaryToModelConversionFailedException(piece, ex);
            }

            return piece;
        }
    }
}
