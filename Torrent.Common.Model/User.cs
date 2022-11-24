using System;
using Torrent.Common.Model.Attribute;

namespace Torrent.Common.Model
{
	public class User : IModel
	{
		[PrimaryKey]
		public int UserId { get; set; }
		public string Username { get; set; }
		public string Name { get; set; }
		public string EmailAddress { get; set; }
		public DateTime BirthDate { get; set; }
	}
}