using System.Configuration;
using System.Data.SqlClient;
using System.Reflection;
using Torrent.Common.Extension;
using Torrent.Common.Logging;
using Torrent.Common.Model.Attribute;

namespace Torrent.Service.Access.Common
{
	public static class DatabaseExtension
	{
		/// <summary>
		/// Inserts a <see cref="T"/> record into the database described in the config file. It skips properties that have <see cref="PrimaryKeyAttribute"/> or <see cref="SkipColumnAttribute"/> attributes.
		/// </summary>
		/// <param name="obj">Object to insert</param>
		/// <typeparam name="T">Type of object</typeparam>
		/// <returns><param name="obj"></param> with the primary key injected into the property that has <see cref="PrimaryKeyAttribute"/>.</returns>
		/// <exception cref="ArgumentException">Property list is null or empty.</exception>
		public static T Insert<T>(this T obj)
		{
			obj.CannotBeNull();

			PropertyInfo primaryKey = obj.GetType().GetProperties().ToList()
				.First(prop => Attribute.IsDefined(prop, typeof(PrimaryKeyAttribute)));

			List<PropertyInfo> properties = obj.GetType().GetProperties().ToList()
				.Where(prop => !Attribute.IsDefined(prop, typeof(PrimaryKeyAttribute)) && !Attribute.IsDefined(prop, typeof(SkipColumnAttribute)))
				.ToList();

			if (!properties.HasItems())
				throw new ArgumentException("DatabaseExtension.Insert<T>(): Parameter 'obj' has no available items or it was null.");


			string tableName = $"[dbo].[{obj.GetType().Name}]";
			string columnList = String.Join(", ", properties.Select(prop => prop.Name));
			string valuesList = String.Join(", ", properties.Select(prop => prop.GetValue(obj).AsColumn()));

			try
			{
				using (DatabaseConnector connector =
				       new DatabaseConnector(AccessConfig.ConnectionString))
				{
					SqlCommand sqlCmd = connector.GetConnection().CreateCommand();
					List<SqlParameter> parameters = new List<SqlParameter>
					{
						new SqlParameter("@tableName", tableName),
						new SqlParameter("@columnList", columnList),
						new SqlParameter("@valuesList", valuesList)
					};

					sqlCmd.CommandText = $"INSERT INTO {tableName} ({columnList}) VALUES ({valuesList})";
					//sqlCmd.Parameters.AddRange(parameters.ToArray());
					sqlCmd.Prepare();
					int newId = (int)sqlCmd.ExecuteNonQuery();
					primaryKey.SetValue(obj, newId);
				}
			}
			catch (Exception ex)
			{
				Logger.As(Log.Error).Exception(ex).Method("DatabaseExtension.Insert<T>").Write();
				throw new ArgumentException(
					$"An error occurred when trying to insert 'obj' into the database. Message: {ex.Message}, Stack trace: {ex.StackTrace}");
			}
			
			return obj;
		}

		/// <summary>
		/// Deletes a <see cref="T"/> record from the database described in the config file. It uses the property which has <see cref="PrimaryKeyAttribute"/> as condition in WHERE clause.
		/// </summary>
		/// <param name="obj">Object to delete</param>
		/// <typeparam name="T">Type of object</typeparam>
		/// <returns>True if deletion was successful, otherwise false.</returns>
		public static bool Delete<T>(this T obj)
		{
			obj.CannotBeNull();

			string primaryKeyColumn = obj.GetType().GetProperties().ToList()
				.First(prop => Attribute.IsDefined(prop, typeof(PrimaryKeyAttribute))).Name;

			return obj.Delete(new[] { primaryKeyColumn });
		}

		
		/// <summary>
		/// Deletes a <see cref="T"/> record from the database described in the config file. It uses strings in <paramref name="whereClause"/> as conditions in WHERE clause.
		/// </summary>
		/// <param name="obj">Object to delete</param>
		/// <param name="whereClause">Conditions placed into WHERE clause in format like "ColumnName = 'Value'"</param>
		/// <typeparam name="T">Type of object</typeparam>
		/// <returns>True if deletion was successful, otherwise false.</returns>
		public static bool Delete<T>(this T obj, IEnumerable<string> whereClause)
		{
			obj.CannotBeNull();

			string tableName = obj.GetType().Name;
			string whereCondition = String.Join(", ", obj.GetType().GetProperties().ToList()
				.Where(prop => whereClause.Contains(prop.Name))
				.Select(prop => $"{prop.Name} = '{prop.GetValue(obj)}'").ToList());
			
			try
			{
				using (DatabaseConnector connector =
				       new DatabaseConnector(AccessConfig.ConnectionString))
				{
					SqlCommand sqlCmd = connector.GetConnection().CreateCommand();
					List<SqlParameter> parameters = new List<SqlParameter>
					{
						new SqlParameter("@tableName", tableName),
						new SqlParameter("@whereCondition", whereCondition)
					};

					sqlCmd.CommandText = "DELETE FROM @tableName WHERE @whereCondition";
					sqlCmd.Parameters.AddRange(parameters.ToArray());

					return sqlCmd.ExecuteNonQuery() > 0;
				}
			}
			catch (Exception ex)
			{
				Logger.As(Log.Error).Exception(ex).Method("DatabaseExtension.Delete<T>").Write();
				throw new ArgumentException(
					$"An error occurred when trying to delete 'obj' from the database. Message: {ex.Message}, Stack trace: {ex.StackTrace}");
			}
		}

		public static SqlCommand GetCommand(this string command)
		{
			return null;
		}
	}
}