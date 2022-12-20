using Torrent.Common.Model.Enum;

namespace Torrent.Common.Model.Response;

public class LoginResponse
{
	public StatusCodes StatusCode { get; set; }
	public string Message { get; set; }
	public User User { get; set; }
}