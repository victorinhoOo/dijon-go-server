﻿using System.Data;
using System.Data.SQLite; 
using System.Collections.Generic;

namespace Server.Model.Data
{
    /// <summary>
    /// Implémente l'interface IDatabase pour une base de données SQLite.
    /// </summary>
    public class SQLiteDatabase : IDatabase
    {
        private string connectionString;
        private SQLiteConnection connection;
        private SQLiteTransaction transaction;

        public SQLiteDatabase(string connectionString)
        {
            this.connectionString = connectionString;
        }

        /// <inheritdoc/>
        public void Connect()
        {
            connection = new SQLiteConnection(connectionString);
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
            using (var command = new SQLiteCommand(query, connection, transaction))
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
            using (var command = new SQLiteCommand(query, connection, transaction))
            {
                if (parameters != null)
                {
                    foreach (var param in parameters)
                    {
                        command.Parameters.AddWithValue(param.Key, param.Value);
                    }
                }
                using (var adapter = new SQLiteDataAdapter(command))
                {
                    var result = new DataTable();
                    adapter.Fill(result);
                    return result;
                }
            }
        }
    }
}
