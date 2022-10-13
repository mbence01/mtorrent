using BencodeNET.Objects;
using BencodeNET.Parsing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Torrent.Client.Model.DotTorrent;
using Torrent.Client.Model.Exception;
using Torrent.Client.Model.Interface;

namespace Torrent.Client.Logic.Parser
{
    /// <summary>
    /// This class implements a torrent file parser function which transforms a file with .torrent extension to a <see cref="TorrentFile"/> instance.
    /// </summary>
    public class TorrentFileParser : ITorrentFileParser
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

        #region Public functions
        public TorrentFile ParseTorrentFile(string path)
        {
            try
            {
                using(Stream stream = new FileStream(path, FileMode.Open))
                {
                    BencodeParser parser = new BencodeParser();
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
        #endregion

        #region Private functions
        /// <summary>
        /// Gets the dictionary keys and values from the given <paramref name="properties"/> parameter and sets the new <see cref="TorrentFile"/>'s properties based on these.
        /// </summary>
        /// <param name="properties">A <see cref="BDictionary"/> object that contains all the data parsed from the .torrent file</param>
        /// <returns>A <see cref="TorrentFile"/> object based on <paramref name="properties"/></returns>
        /// <exception cref="DictionaryToModelConversionFailedException"></exception>
        private TorrentFile GenerateModelForTorrentFile(BDictionary properties)
        {
            TorrentFile torrent = new TorrentFile();

            try
            {
                // These two properties are required according to the BitTorrent file structure
                torrent.Announce        = properties.Get<BString>(AppSettings["PropAnnounce"]).ToString();
                torrent.Info            = GenerateModelForTorrentInfo(properties.Get<BDictionary>(AppSettings["PropInformation"]));

                // These are optional keys
                torrent.CreatedBy       = properties.Get<BString>(AppSettings["PropCreatedBy"])?.ToString();
                torrent.CreationDate    = new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(properties.Get<BNumber>(AppSettings["PropCreationDate"]) ?? 0);
                torrent.Encoding        = System.Text.Encoding.GetEncoding(properties.Get<BString>(AppSettings["PropEncoding"])?.ToString() ?? AppSettings["DefaultTorrentEncoding"]);
            }
            catch(Exception ex)
            {
                throw new DictionaryToModelConversionFailedException(torrent, ex);
            }
            
            return torrent;
        }

        /// <summary>
        /// Generates a <see cref="TorrentInfo"/> object from the given torrent info dictionary
        /// </summary>
        /// <param name="properties">A <see cref="BDictionary"/> object that contains the info field from the .torrent file</param>
        /// <returns>A <see cref="TorrentInfo"/> object based on <paramref name="properties" /></returns>
        /// <exception cref="DictionaryToModelConversionFailedException"></exception>
        private TorrentInfo GenerateModelForTorrentInfo(BDictionary properties)
        {
            TorrentInfo info = new TorrentInfo();

            try
            {
                // These are required keys
                info.Files = GenerateModelForTorrentFilePiecesList(properties.Get<BList>(AppSettings["PropInfoFiles"]));
                info.Name = properties.Get<BString>(AppSettings["PropInfoName"]).ToString();
                info.PieceLength = properties.Get<BNumber>(AppSettings["PropInfoPieceLength"]);

                // These are optional keys
                info.Length = properties.Get<BNumber>(AppSettings["PropInfoLength"]) ?? 0;
                info.Private = Convert.ToBoolean(Convert.ToInt32(properties.Get<BNumber>(AppSettings["PropInfoPrivate"])?.ToString() ?? "0"));
                info.Source = properties.Get<BString>(AppSettings["PropInfoSource"])?.ToString();
            }
            catch(Exception ex)
            {
                throw new DictionaryToModelConversionFailedException(info, ex);
            }

            return info;
        }

        /// <summary>
        /// Generates a list of <see cref="TorrentPiece"/> objects from the given torrent files dictionary
        /// </summary>
        /// <param name="fileProps">A <see cref="BList"/> object that contains the files from the .torrent file</param>
        /// <returns>A list of <see cref="TorrentPiece"/> objects based on <paramref name="fileProps" /></returns>
        /// <exception cref="DictionaryToModelConversionFailedException"></exception>
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

        /// <summary>
        /// Generates a <see cref="TorrentPiece"/> object from the given dictionary
        /// </summary>
        /// <param name="properties">A <see cref="BDictionary"/> object that contains the data of a torrent piece</param>
        /// <returns>A <see cref="TorrentPiece"/> object based on <paramref name="properties" /></returns>
        /// <exception cref="DictionaryToModelConversionFailedException"></exception>
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
        #endregion
    }
}
