using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocket.Model
{
    /// <summary>
    /// Représente la configuration d'une partie
    /// </summary>
    public class GameConfiguration
    {
        private int size;
        private string rule;
        private float komi;
        private string name;
        private int handicap;
        private string handicapColor;

        /// <summary>
        /// Obtient la taille du plateau de jeu.
        /// </summary>
        public int Size => size;

        /// <summary>
        /// Obtient la règle du jeu.
        /// </summary>
        public string Rule => rule;

        /// <summary>
        /// Obtient le komi du jeu.
        /// </summary>
        public float Komi => komi;

        /// <summary>
        /// Obtient le nom du jeu.
        /// </summary>
        public string Name => name;

        /// <summary>
        /// Obtient le handicap du jeu.
        /// </summary>
        public int Handicap => handicap;

        /// <summary>
        /// Obtient la couleur du handicap.
        /// </summary>
        public string HandicapColor => handicapColor;

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="GameConfiguration"/>.
        /// </summary>
        /// <param name="size">La taille du plateau de jeu.</param>
        /// <param name="rule">La règle du jeu.</param>
        /// <param name="komi">Le komi du jeu.</param>
        /// <param name="name">Le nom du jeu.</param>
        /// <param name="handicap">Le handicap du jeu.</param>
        /// <param name="handicapColor">La couleur du handicap.</param>
        public GameConfiguration(int size, string rule, float komi, string name, int handicap, string handicapColor)
        {
            this.size = size;
            this.rule = rule;
            this.komi = komi;
            this.name = name;
            this.handicap = handicap;
            this.handicapColor = handicapColor;
        }

        /// <summary>
        /// Crée une configuration de jeu par défaut.
        /// </summary>
        /// <returns>Une instance de <see cref="GameConfiguration"/> avec les valeurs par défaut.</returns>
        public static GameConfiguration Default()
        {
            return new GameConfiguration(19, "j", 6.5f, "matchmaking-game", 0, "white");
        }
    }

}
