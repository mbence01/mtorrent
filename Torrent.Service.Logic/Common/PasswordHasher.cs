using System.Security.Cryptography;
using System.Text;

namespace Torrent.Service.Logic.Common;

public static class PasswordHasher
{
	private const int MAGIC_CONSTANT = 1445;
	
	public static string HashPassword(string password)
	{
		string shaHashedPassword = "";
		
		using (SHA256 hash = SHA256.Create())
		{
			byte[] hashedBytes = hash.ComputeHash(Encoding.UTF8.GetBytes(password));
			shaHashedPassword = Encoding.UTF8.GetString(hashedBytes);
		}


		StringBuilder hashedPassword = new StringBuilder();

		for (int i = 0; i < shaHashedPassword.Length; i++)
		{
			hashedPassword.Append(shaHashedPassword[i] + (MAGIC_CONSTANT % i));
		}

		return hashedPassword.ToString();
	}

	private static string ShiftPassword(string password)
	{
		int len = password.Length;
		StringBuilder shiftedPassword = new StringBuilder();

		for (int i = 0; i < len; i++)
		{
			shiftedPassword.Append(password[i] + (len - i));
		}

		return shiftedPassword.ToString();
	}
}