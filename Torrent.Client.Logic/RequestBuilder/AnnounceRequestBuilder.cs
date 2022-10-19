using BencodeNET.Objects;
using System;
using System.Security.Cryptography;
using Torrent.Client.Model.Communication.Request;

namespace Torrent.Client.Logic.RequestBuilder
{
    public class AnnounceRequestBuilder
    {
        public AnnounceRequest BuildAnnounceRequest(string trackerUrl, IBObject infoDict, string peerId, int port, int length, string eventStatus, int urlCount = 0, string ip = null)
        {
            string infoHash;

            using(SHA1 sha1 = SHA1.Create())
            {
                byte[] hash = sha1.ComputeHash(infoDict.EncodeAsBytes());
                infoHash = Convert.ToBase64String(hash);
            }

            return new AnnounceRequest
            {
                InfoHash = infoHash,
                PeerId = peerId,
                Ip = ip ?? "",
                Port = port,
                Uploaded = 0,
                Downloaded = 0,
                Left = length,
                Event = eventStatus,
                NumWant = urlCount == 0 ? 50 : urlCount,
                Url = trackerUrl
            };
        }
    }
}
