using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocket.Model
{
    /// <summary>
    /// Gère la création des parties
    /// </summary>
    public static class GameFactory
    {
        /// <summary>
        /// Crée une partie personnalisée
        /// </summary>
        /// <param name="size">taille de la grille</param>
        /// <param name="rule">règles du jeu</param>
        /// <param name="komi">komi choisi</param>
        /// <param name="name">nom de la partie</param>
        /// <param name="handicap">handicap choisi</param>
        /// <returns>La partie créee</returns>
        public static Game CreateCustomGame(int size, string rule, float komi, string name, int handicap)
        {
            return new Game(size, rule, komi, name, handicap);
        }

        /// <summary>
        /// Créé une partie de matchmaking (avec des paramètres par défaut)
        /// </summary>
        /// <returns>La partie créee</returns>
        public static Game CreateMatchmakingGame()
        {
            return new Game(19, "j", 6.5f, "matchmaking-game", 0);
        }
    }
}
