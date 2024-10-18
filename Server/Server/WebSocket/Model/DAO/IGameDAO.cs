using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocket.Model.DAO
{
    /// <summary>
    /// Définit un standard pour les opérations liées aux parties dans la bdd
    /// </summary>
    public interface IGameDAO
    {
        /// <summary>
        /// Insère une partie en base de données
        /// </summary>
        /// <param name="game">Les informations de la partie</param>
        /// <returns>True si l'insertion a réussie</returns>
        public bool InsertGame(Game game);

        /// <summary>
        /// Supprime une partie en base de données
        /// </summary>
        /// <param name="id">l'id de la partie à supprimer</param>
        public void DeleteGame(int id);
    }
}
