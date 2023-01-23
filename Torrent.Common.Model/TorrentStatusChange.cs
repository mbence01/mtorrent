using System;
using Torrent.Common.Model.Attribute;

namespace Torrent.Common.Model;

public class TorrentStatusChange : IModel
{
	[PrimaryKey] 
	public int TorrentStatusChangeID { get; set; }

	public int TorrentID { get; set; }
	public string PreviousStatus { get; set; }
	public string NewStatus { get; set; }
	public DateTime CreationDate { get; set; }
}