using BencodeNET.Objects;
using BencodeNET.Parsing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using Torrent.Client.Communication.ResponseBuilder;
using Torrent.Client.Model.Communication.Request;
using Torrent.Client.Model.Communication.Response;
using Torrent.Client.Model.Interface;

namespace Torrent.Client.Communication
{
    public class HttpTrackerCommunication : ITrackerCommunication
    {
        public ScrapeResponse SendScrape(ScrapeRequest request)
        {
            try
            {
                StringBuilder stringBuilder = new StringBuilder();

                #region Build the request URL
                stringBuilder.Append(request.Uri.ToString().Replace("announce", "scrape"));
                stringBuilder.Append($"?info_hash={request.InfoHash}");
                #endregion

                string requestUrl = stringBuilder.ToString();

                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(requestUrl);

                webRequest.Method = "GET";
                webRequest.KeepAlive = false;
                webRequest.ContentType = "text/plain";
                webRequest.AllowAutoRedirect = false;
                webRequest.Timeout = (int)(TimeSpan.FromMinutes(1).TotalMilliseconds);

                HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();

                byte[] buff = new byte[1024];
                var resp = response.GetResponseStream().ReadAsync(buff, 0, buff.Length);
                string respStr = Encoding.UTF8.GetString(buff);
             
                return new ScrapeResponse();
            }
            catch (Exception ex)
            {
                return new ScrapeResponse
                {
                    Errors = new List<string> { ex.Message, ex.StackTrace }
                };
            }
        }

        public AnnounceResponse SendAnnounce(AnnounceRequest request)
        {
            try
            {
                StringBuilder stringBuilder = new StringBuilder();

                #region Build the request URL
                stringBuilder.Append(request.Uri.ToString());
                stringBuilder.Append($"?info_hash={request.InfoHash}");
                stringBuilder.Append($"&peer_id={HttpUtility.UrlEncode(request.PeerId)}");
                
                if(!String.IsNullOrEmpty(request.Ip))
                    stringBuilder.Append($"&ip={request.Ip}");

                stringBuilder.Append($"&port={request.Port}");
                stringBuilder.Append($"&uploaded={request.Uploaded}");
                stringBuilder.Append($"&downloaded={request.Downloaded}");
                stringBuilder.Append($"&left={request.Left}");
                stringBuilder.Append($"&compact={request.Compact}");

                if (!String.IsNullOrEmpty(request.Event))
                    stringBuilder.Append($"&event={request.Event}");

                stringBuilder.Append($"&numwant={request.NumWant}");
                #endregion

                string requestUrl = stringBuilder.ToString();

                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(requestUrl);

                webRequest.Method = "GET";
                webRequest.KeepAlive = false;
                webRequest.ContentType = "text/plain";
                webRequest.AllowAutoRedirect = false;
                webRequest.Timeout = (int)(TimeSpan.FromMinutes(1).TotalMilliseconds);

                HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();

                byte[] buff = new byte[1024];
                response.GetResponseStream().ReadAsync(buff, 0, buff.Length);

                return new AnnounceResponseBuilder().BuildAnnounceResponse(buff);
            }
            catch(Exception ex)
            {
                return new AnnounceResponse
                {
                    Errors = new List<string> { ex.Message, ex.StackTrace }
                };
            }
        }

        public ConnectResponse ConnectToTracker(ConnectRequest request)
        {
            try
            {
                HttpClient client = new HttpClient();

                string requestUrl = $"{request.Uri.ToString()}?connection_id={request.ProtocolId}&action={request.Action}&transaction_id={request.TransactionId}";
                HttpResponseMessage response = client.GetAsync(requestUrl).Result;

                string res = response.Content.ReadAsStringAsync().Result;

                WebRequest trackerReq = WebRequest.Create(requestUrl);
                trackerReq.Method = "GET";                
                
                StreamReader respReader = new StreamReader(trackerReq.GetResponse().GetResponseStream());

                //string response = respReader.ReadToEnd();

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
