using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Torrent.Client.Model.Communication.Request;

namespace Torrent.Client.Communication
{
    public class PeerCommunication
    {
        private const int MAX_TRIES = 10;

        public string SendHandshake(PeerHandshakeRequest request)
        {
            byte[] packet = new byte[] { (byte)request.Pstrlen }
                                    .Concat(Encoding.UTF8.GetBytes(request.Pstr))
                                    .Concat(request.Reserved)
                                    .Concat(request.InfoHash)
                                    .Concat(Encoding.UTF8.GetBytes(request.PeerId))
                                    .ToArray();




            var listener = new TcpClient();

            try
            {
                listener.Connect(request.Uri.Host, request.Uri.Port);
                var stream = listener.GetStream();
                stream.Write(packet, 0, packet.Length);
                listener.Client.Send(packet);

                var data = new byte[1000];
                var responseData = string.Empty;

                var bytes = stream.Read(data, 0, data.Length);
                responseData = Encoding.ASCII.GetString(data, 0, bytes);
                listener.Client.Close();
                listener.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                listener.Close();
            }
            

            return "connect error";
        }
    }
}
