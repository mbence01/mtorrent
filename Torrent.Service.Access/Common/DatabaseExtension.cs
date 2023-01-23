using System.Data;
using System.Data.SqlClient;
using System.Numerics;
using System.Reflection;
using Torrent.Common.Extension;
using Torrent.Common.Logging;
using Torrent.Common.Model;
using Torrent.Common.Model.Attribute;

namespace Torrent.Service.Access.Common
{
	public static class DatabaseExtension
	{
		/// <summary>
		/// Selects all of the found <typeparamref name="T"/> entities from the database.
		/// </summary>
		/// <typeparam name="T">Type of entities</typeparam>
		/// <returns>A List of <typeparamref name="T"/> type entities</returns>
		public static List<T> Select<T>() where T : IModel, new() => Select<T>(SqlWhereClause.CreateEmpty());

        /// <summary>
        /// Selects all of the found <typeparamref name="T"/> entities from the database that satisfies the <paramref name="where"/> condition.
        /// </summary>
        /// <typeparam name="T">Type of entities</typeparam>
        /// <param name="where"><see cref="SqlWhereClause"/> object, which describes an SQL where clause</param>
        /// <returns>A List of <typeparamref name="T"/> type entities</returns>
        /// <exception cref="ArgumentException"></exception>
        public static List<T> Select<T>(SqlWhereClause where) where T : IModel, new()
		{
			const string selectStmtTemplate = "SELECT * FROM {0} {1}";
			string tableName = $"[dbo].[{typeof(T).Name}]";
			List<T> returnModels = new List<T>();

			try
			{
				using (DatabaseConnector connector = new DatabaseConnector())
				{
					SqlCommand command = String.Format(selectStmtTemplate, tableName, where).GetCommand(connector, where.GetParameters());
					command.Prepare();

					using (SqlDataReader reader = command.ExecuteReader())
					{
						while (reader.Read())
							returnModels.Add(reader.CreateModel<T>());
					}
				}
			}
			catch (Exception ex)
			{
				Logger.As(Log.Error).Exception(ex).Method("DatabaseExtension.Select<T>").Write();
				throw new ArgumentException(
					$"An error occurred when trying to select from {tableName}. Message: {ex.Message}, Stack trace: {ex.StackTrace}");
			}

			return returnModels;
		}

		/// <summary>
		/// Inserts a <see cref="T"/> record into the database described in the config file. It skips properties that have <see cref="PrimaryKeyAttribute"/> or <see cref="SkipColumnAttribute"/> attributes.
		/// </summary>
		/// <param name="obj">Object to insert</param>
		/// <typeparam name="T">Type of object</typeparam>
		/// <returns><param name="obj"></param> with the primary key injected into the property that has <see cref="PrimaryKeyAttribute"/>.</returns>
		/// <exception cref="ArgumentException">Property list is null or empty.</exception>
		public static T Insert<T>(this T obj) where T : IModel
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
					List<SqlParameter> parameters = new List<SqlParameter>
					{
						new SqlParameter("@tableName", tableName),
						new SqlParameter("@columnList", columnList),
						new SqlParameter("@valuesList", valuesList)
					};

					SqlCommand sqlCmd = "INSERT INTO @tableName (@columnList) VALUES (@valuesList)".GetCommand(connector, parameters);
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
		public static bool Delete<T>(this T obj) where T : IModel
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
		public static bool Delete<T>(this T obj, IEnumerable<string> whereClause) where T : IModel
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

		public static T CreateModel<T>(this SqlDataReader reader) where T : IModel, new()
		{
			List<Type> numberTypes = new List<Type> { typeof(int), typeof(Int16), typeof(Int32), typeof(Int64), typeof(BigInteger) };
			List<Type> textTypes   = new List<Type> { typeof(String), typeof(string) };
			List<Type> dateTypes   = new List<Type> { typeof(DateTime), typeof(DateOnly) };
			List<Type> boolTypes   = new List<Type> { typeof(Boolean), typeof(bool) };

			T model = new T();
			PropertyInfo nextProp = typeof(T).GetProperties()[0];

			foreach (PropertyInfo prop in typeof(T).GetProperties().ToList())
			{
				try
				{
					nextProp = prop;
					
					if (textTypes.Contains(prop.PropertyType))
						prop.SetValue(model, reader.GetString(reader.GetOrdinal(prop.Name)));

					if (numberTypes.Contains(prop.PropertyType))
						prop.SetValue(model, reader.GetInt32(reader.GetOrdinal(prop.Name)));

					if (dateTypes.Contains(prop.PropertyType))
						prop.SetValue(model, reader.GetDateTime(reader.GetOrdinal(prop.Name)));

					if (boolTypes.Contains(prop.PropertyType))
						prop.SetValue(model, reader.GetBoolean(reader.GetOrdinal(prop.Name)));
				}
				catch (Exception ex)
				{
					Logger.As(Log.Warning).Exception(ex).Method("DatabaseExtension.CreateModel<T>")
						.Message($"Warning when trying to parse rows from database. Cannot set value of property '{nextProp.Name}'.").Write();
				}
			}

			return model;
		}

		#region Different overloads for GetCommand()
		public static SqlCommand GetCommand(this string command, SqlConnection connection) =>
			GetCommand(connection, null, command, null, null);

		public static SqlCommand GetCommand(this string command, DatabaseConnector connector) =>
			GetCommand(null, connector, command, null, null);

		public static SqlCommand GetCommand(this string command, SqlConnection connection, CommandType commandType) =>
			GetCommand(connection, null, command, commandType, null);
		
		public static SqlCommand GetCommand(this string command, DatabaseConnector connector, CommandType commandType) =>
			GetCommand(null, connector, command, commandType, null);

		public static SqlCommand GetCommand(this string command, SqlConnection connection, IEnumerable<SqlParameter> parameters) => 
			GetCommand(connection, null, command, null, parameters);
		
		public static SqlCommand GetCommand(this string command, DatabaseConnector connector, IEnumerable<SqlParameter> parameters) => 
			GetCommand(null, connector, command, null, parameters);

		public static SqlCommand GetCommand(this string command, SqlConnection connection, CommandType commandType, IEnumerable<SqlParameter> parameters) =>
			GetCommand(connection, null, command, commandType, parameters);
		
		public static SqlCommand GetCommand(this string command, DatabaseConnector connector, CommandType commandType, IEnumerable<SqlParameter> parameters) =>
			GetCommand(null, connector, command, commandType, parameters);
		#endregion
		
		private static SqlCommand GetCommand(SqlConnection? connection, DatabaseConnector? connector, string command, CommandType? commandType, IEnumerable<SqlParameter>? parameters)
		{
			SqlCommand cmd = null;

			if (connection != null)
				cmd = connection.CreateCommand();
			if (connector != null)
				cmd = connector.GetConnection().CreateCommand();

			if (cmd == null)
				throw new ArgumentException(
					$"DatabaseExtensions.GetCommand: Both the given connection and connector were null. At least one must have a value.");


			cmd.CommandText = command;
			cmd.CommandType = commandType == null ? CommandType.Text : commandType.Value;
			
			if(parameters.HasItems())
				cmd.Parameters.AddRange(parameters.ToArray());

			return cmd;
		}
	}
}