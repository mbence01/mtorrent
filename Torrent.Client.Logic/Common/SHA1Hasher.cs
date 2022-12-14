using BencodeNET.Objects;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Torrent.Client.Logic.Common
{
    public static class SHA1Hasher
    {
        public static string CreateHash(IBObject text, bool urlEncode = true)
        {
            string infoHash;

            using (SHA1 sha1 = SHA1.Create())
            {
                byte[] info = text.EncodeAsBytes();
                byte[] hash = sha1.ComputeHash(info);

                infoHash = string.Concat(hash.Select(b => b.ToString("x2")));
                Enumerable.Range(1, infoHash.Length / 2).Select(x => infoHash = infoHash.Insert(x * 2, "%"));
            }

            if(urlEncode)
            {
                StringBuilder encodedHash = new StringBuilder();

                for (int i = 0; i < infoHash.Length; i++)
                {
                    if (i % 2 == 0)
                        encodedHash.Append("%");

                    encodedHash.Append(infoHash[i]);
                }

                infoHash = encodedHash.ToString();
            }

            return infoHash;
        }
    }
}
