using BencodeNET.Objects;
using System;
using System.Security.Cryptography;
using Torrent.Client.Logic.Common;
using Torrent.Client.Model.Communication.Request;
using Torrent.Client.Model.DotTorrent;

namespace Torrent.Client.Logic.RequestBuilder
{
    public class ScrapeRequestBuilder
    {
        public ScrapeRequest BuildScrapeRequest(TorrentFile torrent)
        {
            return new ScrapeRequest
            {
                InfoHash = SHA1Hasher.CreateHash(torrent.BencodedDictionary["info"]),
                Uri = torrent.Announce
            };
        }
    }
}
