using System;
using Torrent.Client.Model.Communication.Request;

namespace Torrent.Client.Logic.RequestBuilder
{
    public class ConnectRequestBuilder
    {
        private const Int64 PROTOCOL_ID_CONSTANT = 0x41727101980;

        public ConnectRequest BuildConnectRequest()
        {
            byte[] arr;
            
            using(System.IO.MemoryStream stream = new System.IO.MemoryStream())
            {
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                formatter.Serialize(stream, 0x41727101980);
                formatter.Serialize(stream, 0);
                formatter.Serialize(stream, new Random().Next(Int32.MaxValue));

                arr = stream.ToArray();
            }

            return new ConnectRequest
            {
                ProtocolId = PROTOCOL_ID_CONSTANT,
                Action = 0,
                TransactionId = new Random().Next(Int32.MaxValue)
            };
        }
    }
}
