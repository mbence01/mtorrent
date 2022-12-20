using Torrent.Common.Model.Attribute;

namespace Torrent.Common.Model;

public class Token : IModel
{
	[PrimaryKey]
	public int TokenId { get; set; }
	public int UserId { get; set; }
	public string TokenValue { get; set; }
}