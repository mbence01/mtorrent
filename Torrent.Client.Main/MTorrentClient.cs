using System;
using System.Net.Sockets;
using System.Text;
using Torrent.Client.Logic;
using Torrent.Client.Model;
using Torrent.Client.Model.Communication.Response;
using Torrent.Client.Model.DotTorrent;

namespace Torrent.Client.Main
{
    /// <summary>
    /// The main entry point of the torrent client
    /// </summary>
    public class MTorrentClient
    {
        private string _peerId;
        private byte[] _fileContent;
        private TrackerManager _trackerManager;
        private TorrentFile _parsedTorrent;
        private Config _configuration;

        public MTorrentClient(string path)
        {
            _fileContent = System.IO.File.ReadAllBytes(path);
            _peerId = GenerateUniquePeerId();
        }

        public MTorrentClient(byte[] fileContent)
        {
            _fileContent = fileContent;
            _peerId = GenerateUniquePeerId();
        }

        public void ConfigureClient(Config config)
        {
            _configuration = config;
        }

        public void StartTorrent()
        {
            CheckConfig();

            _parsedTorrent = _configuration.Parser.ParseTorrent(_fileContent);
            _trackerManager = new TrackerManager(_parsedTorrent, _configuration);

            AnnounceResponse response = _trackerManager.SendAnnounce(_peerId);

            foreach(Peer peer in response.Peers)
            {
                try
                {
                    using (TcpClient client = new TcpClient())
                    {
                        Console.WriteLine("PEER " + peer.Id);
                        client.Connect(peer.Ip, peer.Port);

                        Console.WriteLine(client.Connected ? "connected" : "not connected");

                        NetworkStream stream = client.GetStream();

                        stream.Write(Encoding.UTF8.GetBytes("hello world"), 0, Encoding.UTF8.GetBytes("hello world").Length);
                        Console.WriteLine("sent hello world");
                        byte[] buffer = new byte[1024];
                        stream.Read(buffer, 0, buffer.Length);
                        Console.WriteLine("received: " + Encoding.UTF8.GetString(buffer));
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }

            _trackerManager.SendScrape();
        }

        private string GenerateUniquePeerId()
        {
            string alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789§'+!%/=()-.,;>*";
            Random random = new Random();
            StringBuilder peerId = new StringBuilder();

            for (int i = 0; i < 20; i++)
            {
                peerId.Append(alphabet[random.Next(alphabet.Length)]);
            }

            return peerId.ToString();
        }

        private void CheckConfig()
        {
            if (_configuration == null)
                this.UseDefaultConfig();
        }

        private void UseDefaultConfig()
        {
            _configuration = new Config();
        }
    }
}
