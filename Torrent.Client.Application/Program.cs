﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Torrent.Client.Main;

namespace Torrent.Client.Application
{
    internal class Program
    {
        static void Main(string[] args)
        {
            byte[] content = File.ReadAllBytes(@"C:\Users\madra\Downloads\test.torrent");

            MTorrentClient client = new MTorrentClient(content);

            client.StartTorrent();

            Console.ReadKey();
        }
    }
}
