using System;
using Torrent.Common.Model.Attribute;

namespace Torrent.Common.Model;

public class UserPassword : IModel
{
	[PrimaryKey]
	public int UserPasswordID { get; set; }
	public int UserID { get; set; }
	public string Password { get; set; }
	public DateTime CreationDate { get; set; }
}