using System.Data;
using System.Data.SqlClient;
using Torrent.Common.Model;
using Torrent.Service.Access.Common;

namespace Torrent.Service.Access;

public partial class ServiceAccess
{
	public User? GetUserByToken(string token)
	{
		using (DatabaseConnector connector = new DatabaseConnector(_connectionString, _maxConnectionAttempts))
		{
			List<SqlParameter> parameters = new List<SqlParameter>()
			{
				new SqlParameter("@tokenValue", token) { DbType = DbType.String, Size = 64 }
			};
			
			SqlCommand sqlCmd = $@"
				SELECT
					Users.*
				FROM
					Token
				INNER JOIN
					[dbo].[User] Users ON Users.UserID = Token.UserID
				WHERE
					TokenValue = @tokenValue AND TokenExpireDate > GETDATE()
			".GetCommand(connector, parameters);

			
			using (SqlDataReader reader = sqlCmd.ExecuteReader())
			{
				while (reader.Read())
					return reader.CreateModel<User>();
			}

			return null;
		}
	}
}