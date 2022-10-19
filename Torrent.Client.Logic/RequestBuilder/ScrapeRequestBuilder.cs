using BencodeNET.Objects;
using System;
using System.Security.Cryptography;
using Torrent.Client.Model.Communication.Request;

namespace Torrent.Client.Logic.RequestBuilder
{
    public class ScrapeRequestBuilder
    {
        public ScrapeRequest BuildScrapeRequest(string trackerUrl, IBObject infoDict)
        {
            string infoHash;

            using (SHA1 sha1 = SHA1.Create())
            {
                byte[] hash = sha1.ComputeHash(infoDict.EncodeAsBytes());
                infoHash = Convert.ToBase64String(hash);
            }

            return new ScrapeRequest
            {
                InfoHash = infoHash,
                Url = trackerUrl
            };
        }
    }
}
