using System;
using System.Net;
using System.Net.NetworkInformation;
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
        private DownloadManager _downloadManager;
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
            _downloadManager = new DownloadManager(_configuration);

            int i = 0;

            AnnounceResponse response = _trackerManager.SendAnnounce(_peerId);



            foreach(Peer peer in response.Peers)
            {
                //string resp = _downloadManager.SendHandshake(peer, response.InfoHash, _peerId);

                Console.WriteLine(peer.Ip + ":" + peer.Port);

                //try
                //{
                //    Ping pingSender = new Ping();
                //    PingOptions options = new PingOptions();

                //    // Use the default Ttl value which is 128,
                //    // but change the fragmentation behavior.
                //    options.DontFragment = true;

                //    // Create a buffer of 32 bytes of data to be transmitted.
                //    string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
                //    byte[] buffer = Encoding.ASCII.GetBytes(data);
                //    int timeout = 120;
                //    PingReply reply = pingSender.Send(peer.Ip, timeout, buffer, options);
                //    if (reply.Status == IPStatus.Success)
                //    {
                //        i++;
                //        Console.WriteLine("Address: {0}", reply.Address.ToString());
                //        Console.WriteLine("RoundTrip time: {0}", reply.RoundtripTime);
                //        Console.WriteLine("Time to live: {0}", reply.Options.Ttl);
                //        Console.WriteLine("Don't fragment: {0}", reply.Options.DontFragment);
                //        Console.WriteLine("Buffer size: {0}", reply.Buffer.Length);
                //    }
                //    else Console.WriteLine("CANNOT REACH DESTINATION");

                //    Console.WriteLine("==================================================");
                //}
                //catch(Exception ex)
                //{
                //    Console.WriteLine("Error: " + ex.Message);
                //}
            }

            Console.WriteLine($"Reached {i} of {response.Peers.Count}");
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
