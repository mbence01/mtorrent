using BencodeNET;
using BencodeNET.Objects;
using BencodeNET.Parsing;
using System.Collections.Generic;
using System;
using Torrent.Client.Model.Communication.Response;
using Torrent.Client.Model.Exception;
using Torrent.Client.Model;
using System.Linq;
using System.Text;

namespace Torrent.Client.Communication.ResponseBuilder
{
    public class AnnounceResponseBuilder
    {
        public AnnounceResponse BuildAnnounceResponse(byte[] response)
        {
            BDictionary parsedResponse = new BencodeParser().Parse<BDictionary>(response);

            if (parsedResponse.TryGetValue("failure reason", out IBObject reason))
                throw new TrackerFailureResponseException(reason.ToString(), "announce");

            AnnounceResponse respObject = new AnnounceResponse();

            if (parsedResponse.ContainsKey("complete"))
                respObject.Complete = parsedResponse.Get<BNumber>("complete");

            if (parsedResponse.ContainsKey("incomplete"))
                respObject.Incomplete = parsedResponse.Get<BNumber>("incomplete");

            if (parsedResponse.ContainsKey("interval"))
                respObject.Interval = parsedResponse.Get<BNumber>("interval");

            if (parsedResponse.ContainsKey("min interval"))
                respObject.MinimumInterval = parsedResponse.Get<BNumber>("min interval");

            if(parsedResponse.ContainsKey("peers"))
            {
                byte[] peers = parsedResponse.Get<BString>("peers").Value.ToArray();
                List<Peer> peerList = new List<Peer>();
                int peerCount = 0;

                for (int i = 0; i < peers.Length / 6; i++)
                {
                    Byte[] ipAddr = new byte[4];
                    int port;

                    ipAddr[0] = Convert.ToByte(peers[6 * i + 0]);
                    ipAddr[1] = Convert.ToByte(peers[6 * i + 1]);
                    ipAddr[2] = Convert.ToByte(peers[6 * i + 2]);
                    ipAddr[3] = Convert.ToByte(peers[6 * i + 3]);

                    port = BitConverter.ToUInt16(new byte[] { peers[6 * i + 4], peers[6 * i + 5] }, 0);

                    peerList.Add(new Peer
                    {
                        Id = ++peerCount,
                        Ip = String.Join(".", ipAddr),
                        Port = port
                    });
                }

                respObject.Peers = peerList.ToList();
            }
            return respObject;
        }
    }
}
