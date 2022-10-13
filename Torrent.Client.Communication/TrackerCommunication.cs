﻿using System;
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

                client.Connect(endPoint);


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
