using System.Data;

namespace Server.Model.Data
{
    /// <summary>
    /// Interface représentant une base de données.
    /// </summary>
    public interface IDatabase
    {
        /// <summary>
        /// Établit une connexion à la base de données.
        /// </summary>
        public void Connect();

        /// <summary>
        /// Ferme la connexion à la base de données.
        /// </summary>
        public void Disconnect();

        /// <summary>
        /// Démarre une transaction.
        /// </summary>
        public void BeginTransaction();

        /// <summary>
        /// Valide (commit) la transaction actuelle.
        /// </summary>  
        public void CommitTransaction();

        /// <summary>
        /// Annule (rollback) la transaction actuelle.
        /// </summary>
        public void RollbackTransaction();

        /// <summary>
        /// Exécute une requête SQL sans retour de résultat (INSERT, UPDATE, DELETE).
        /// </summary>
        /// <param name="query">La requête SQL à exécuter.</param>
        /// <param name="parameters">Les paramètres de la requête.</param>
        public void ExecuteNonQuery(string query, Dictionary<string, object> parameters);

        /// <summary>
        /// Exécute une requête SQL avec retour de résultat (SELECT).
        /// </summary>
        /// <param name="query">La requête SQL à exécuter.</param>
        /// <param name="parameters">Les paramètres de la requête.</param>
        /// <returns>Le résultat de la requête sous forme de DataTable.</returns>
        public DataTable ExecuteQuery(string query, Dictionary<string, object> parameters);
    }
}
