namespace Torrent.Client.Model.Interface.Main
{
    public interface IConfig
    {
        ITorrentFileParser Parser { get; set; }
    }
}
