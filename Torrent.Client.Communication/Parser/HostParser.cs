using System;
using System.Net;
using Torrent.Client.Model.Exception;

namespace Torrent.Client.Communication.Parser
{
    public static class HostParser
    {
        /// <summary>
        /// Parses a given hostname string to <see cref="IPAddress"/> object
        /// </summary>
        /// <param name="uri"><see cref="Uri"/> object</param>
        /// <returns><see cref="IPAddress"/> object</returns>
        /// <exception cref="InvalidTrackerIPAddressOrDnsException"></exception>
        public static IPAddress ParseIPAddress(Uri uri)
        {
            if(uri.HostNameType == UriHostNameType.Dns)
            {
                IPAddress[] ipAddresses = Dns.GetHostAddresses(uri.Host);

                if (ipAddresses == null || ipAddresses.Length == 0)
                    throw new InvalidTrackerIPAddressOrDnsException(uri.Host, null);

                return ipAddresses[0];
            }
            return IPAddress.Parse(uri.Host);
        }
    }
}
