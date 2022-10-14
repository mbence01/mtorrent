using System;
using System.Collections.Generic;
using System.IO;
using Torrent.Client.Model.Communication.Request;

namespace Torrent.Client.Logic.RequestBuilder
{
    public class ConnectRequestBuilder
    {
        private const Int64 PROTOCOL_ID_CONSTANT = 0x41727101980;

        public ConnectRequest BuildConnectRequest(string trackerUrl)
        {
            ConnectRequest request = new ConnectRequest
            {
                ProtocolId = PROTOCOL_ID_CONSTANT,
                Action = 0,
                TransactionId = new Random().Next(Int32.MaxValue)
            };
            
            List<byte> bytes = new List<byte>();

            bytes.AddRange(BitConverter.GetBytes(request.ProtocolId));
            bytes.AddRange(BitConverter.GetBytes(request.Action));
            bytes.AddRange(BitConverter.GetBytes(request.TransactionId));

            request.RawMessage = bytes.ToArray();
            request.Uri = new Uri(trackerUrl);
            return request;
        }
    }
}
