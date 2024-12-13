using Microsoft.Data.Sqlite;
using SQLitePCL;
using System.Data;
using WebSocket.Model.DAO;

namespace Server.Model.Data
{
    /// <summary>
    /// Implémente l'interface IDatabase pour une base de données SQLite.
    /// </summary>
    public class SQLiteDatabase : IDatabase
    {
        private string connectionString;
        private SqliteConnection connection;
        private SqliteTransaction transaction;
        private static readonly object _lock = new object();

        public SQLiteDatabase(string connectionString)
        {
            this.connectionString = connectionString;
            Batteries_V2.Init();
        }

        /// <inheritdoc/>
        public void Connect()
        {
            connection = new SqliteConnection(connectionString);
            connection.Open();
        }

        /// <inheritdoc/>
        public void Disconnect()
        {
            if (connection != null && connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }

        /// <inheritdoc/>
        public void BeginTransaction()
        {
            if (connection.State == ConnectionState.Open)
            {
                transaction = connection.BeginTransaction();
            }
        }

        /// <inheritdoc/>
        public void CommitTransaction()
        {
            if (transaction != null)
            {
                transaction.Commit();
                transaction = null;
            }
        }

        /// <inheritdoc/>
        public void RollbackTransaction()
        {
            if (transaction != null)
            {
                transaction.Rollback();
                transaction = null;
            }
        }

        /// <inheritdoc/>
        public void ExecuteNonQuery(string query, Dictionary<string, object> parameters)
        {
            using (var command = new SqliteCommand(query, connection, transaction))
            {
                if (parameters != null)
                {
                    foreach (var param in parameters)
                    {
                        command.Parameters.AddWithValue(param.Key, param.Value);
                    }
                }
                command.ExecuteNonQuery();
            }
        }

        /// <inheritdoc/>
        public DataTable ExecuteQuery(string query, Dictionary<string, object> parameters)
        {
            lock (_lock)
            {
                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new SqliteCommand(query, connection))
                    {
                        if (parameters != null)
                        {
                            foreach (var param in parameters)
                            {
                                command.Parameters.AddWithValue(param.Key, param.Value);
                            }
                        }
                        using (var reader = command.ExecuteReader())
                        {
                            var result = new DataTable();
                            result.Load(reader); // Remplit le DataTable avec les résultats du reader
                            return result;
                        }
                    }
                }
            }
        }
    }
}
