using System;
using Torrent.Common.Model.Attribute;

namespace Torrent.Common.Model;

public class Torrent : IModel
{
	[PrimaryKey]
	public int TorrentID { get; set; }
	public string Name { get; set; }
	public double Length { get; set; }
	public bool IsPrivate { get; set; }
	public string Source { get; set; }
	public string TrackerServerUrl { get; set; }
	public string Encoding { get; set; }
	public DateTime CreationDate { get; set; }
	public string CreationProgram { get; set; }
	public int UserID { get; set; }
	public string CurrentStatus { get; set; }
	public double CurrentDownloadPercentage { get; set; }
	public double CurrentUploadPercentage { get; set; }
}