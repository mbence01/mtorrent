using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Torrent.Client.Communication.Parser;
using Torrent.Client.Model.Communication.Request;
using Torrent.Client.Model.Communication.Response;

namespace Torrent.Client.Communication
{
    public class TrackerCommunication
    {
        public ConnectResponse ConnectToTracker(ConnectRequest request)
        {
            UdpClient client = new UdpClient();

            try
            { 
                IPAddress ipAddr = HostParser.ParseIPAddress(request.Uri);
                IPEndPoint endPoint = new IPEndPoint(ipAddr, request.Uri.Port);
                IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);

                client.Connect(endPoint);

                int bytesSent = client.Send(request.RawMessage, request.RawMessage.Length);

                byte[] resp = client.Receive(ref remoteEndPoint);

                return new ConnectResponse();
            }
            catch(Exception ex)
            {
                return new ConnectResponse
                {
                    Errors = new List<string> { ex.Message, ex.StackTrace }
                };
            }
        }
    }
}
