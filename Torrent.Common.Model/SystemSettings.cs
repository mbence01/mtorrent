namespace Torrent.Common.Model;

public class SystemSettings : IModel
{
	public string SettingKey { get; set; }
	public string SettingValue { get; set; }
	public string Description { get; set; }
}