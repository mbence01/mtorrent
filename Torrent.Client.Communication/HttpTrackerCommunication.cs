using BencodeNET.Objects;
using BencodeNET.Parsing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
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
                HttpClient httpClient = new HttpClient();
                string requestUrl = $"{request.Url.Replace("announce", "scrape")}?info_hash=%61%a4%3f%fc%dd%ae%05%01%fa%8c%d9%d0%d8%59%77%d4%80%7e%84%11";

                HttpResponseMessage response = httpClient.GetAsync(requestUrl).Result;

                Stream res = response.Content.ReadAsStreamAsync().Result;
                BDictionary dict = new BencodeParser().Parse<BDictionary>(res);
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
                HttpClient httpClient = new HttpClient();
                StringBuilder stringBuilder = new StringBuilder();
                string hash = "%61%a4%3f%fc%dd%ae%05%01%fa%8c%d9%d0%d8%59%77%d4%80%7e%84%11";
                #region Build the request URL
                stringBuilder.Append(request.Url);
                stringBuilder.Append($"?info_hash={hash}");
                //stringBuilder.Append($"&peer_id={HttpUtility.UrlEncode(request.PeerId)}");
                
                //if(!String.IsNullOrEmpty(request.Ip))
                //    stringBuilder.Append($"&ip={request.Ip}");

                //stringBuilder.Append($"&port={request.Port}");
                //stringBuilder.Append($"&uploaded={request.Uploaded}");
                //stringBuilder.Append($"&downloaded={request.Downloaded}");
                //stringBuilder.Append($"&left={request.Left}");
                
                //if(!String.IsNullOrEmpty(request.Event))
                //    stringBuilder.Append($"&event={request.Event}");
                
                //stringBuilder.Append($"&numwant={request.NumWant}");
                #endregion

                string requestUrl = stringBuilder.ToString();
                
                HttpResponseMessage response = httpClient.GetAsync(requestUrl).Result;

                string res = response.Content.ReadAsStringAsync().Result;

                return new AnnounceResponse();
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

                string requestUrl = $"{request.Url}?connection_id={request.ProtocolId}&action={request.Action}&transaction_id={request.TransactionId}";
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
