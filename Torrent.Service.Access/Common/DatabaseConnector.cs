using System.Configuration;
using System.Data.SqlClient;
using Torrent.Common.Extension;
using Torrent.Common.Logging;

namespace Torrent.Service.Access.Common
{
    /// <summary>
    /// Provides a safety disposable way to connect to a database.
    /// </summary>
    public class DatabaseConnector : IDisposable
    {
        private readonly int _numberOfTries;
        private readonly string _connectionString;
        private readonly SqlConnection _connection;

        public DatabaseConnector() : this(AccessConfig.ConnectionString, AccessConfig.MaxAttempts) {}

        /// <summary>
        /// Initializes a new <see cref="DatabaseConnector"/> instance with 5 connection attempts, then connects to database with the given <paramref name="connectionString"/>
        /// </summary>
        /// <param name="connectionString">The connection string which the class will connect to the database with.</param>
        public DatabaseConnector(string connectionString) : this(connectionString, AccessConfig.MaxAttempts) {}
        
        /// <summary>
        /// Initializes a new <see cref="DatabaseConnector"/> instance and connects to database with the given <paramref name="connectionString"/>.
        /// </summary>
        /// <param name="connectionString">The connection string which the class will connect to the database with.</param>
        /// <param name="tries">Number of attempts as many time as the program will try to open a connection to the remote SQL Server.</param>
        public DatabaseConnector(string connectionString, int tries)
        {
            this._numberOfTries = tries;
            this._connectionString = connectionString;
            _connection = new SqlConnection(_connectionString);

            Connect(_numberOfTries);
        }

        /// <summary>
        /// Tries a connection to the remote SQL Server with <paramref name="tries"/> attempts.
        /// </summary>
        /// <param name="tries">Number of attempts as many time as the program will try to open a connection.</param>
        /// <returns></returns>
        private void Connect(int tries)
        {
            #region Value checks
            tries.MustBeGreaterThan(0);
            _connection.CannotBeNull();
            #endregion

            while (tries-- != 0)
            {
                try
                {
                    _connection.Open();
                }
                catch (SqlException ex) 
                {
                    Logger.As(Log.Error).Exception(ex).From(this).Method("Connect").Message($"Connection attempt to the remote SQL Server #{tries}...").Write();
                }
                
                if (_connection.State == System.Data.ConnectionState.Open)
                    return;
            }
            
            // Throw an exception because if the code is here, the connection was not successful.
            string errorMessage =
                "Could not connect to the remote SQL Server. The connection string is probably invalid or deprecated. Try again.";
                
            Logger.As(Log.Error).From(this).Method("Constructor").Message(errorMessage).Write();
            throw new Exception(errorMessage);
        }

        /// <summary>
        /// Returns the <see cref="SqlConnection"/> object which will be used in database queries.
        /// </summary>
        /// <returns>An <see cref="SqlConnection"/> instance.</returns>
        public SqlConnection GetConnection()
        {
            _connection.CannotBeNull();
            return _connection;
        }

        public void Dispose()
        {
            if (_connection.IsNotNull() && _connection.State == System.Data.ConnectionState.Open)
                _connection.Close();
        }
    }
}
