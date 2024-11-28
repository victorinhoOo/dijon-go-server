using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoLogic.Goban
{
    /// <summary>
    /// Represents the game board and its operations
    /// </summary>
    public interface IBoard
    {
        /// <summary>
        /// Gets the size of the board (width/height)
        /// </summary>
        int Size { get; }

        /// <summary>
        /// Tour actuel, Noir ou Blanc
        /// </summary>
        public StoneColor CurrentTurn { get; set; }

        /// <summary>
        /// Pierre noire qui ont été capturée
        /// </summary>
        public int CapturedBlackStones { get; set; }

        /// <summary>
        /// Pierre blanche qui ont été capturée
        /// </summary>
        public int CapturedWhiteStones { get; set; }

        /// <summary>
        /// Gets the stone at the specified coordinates
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <returns>The stone at the specified position</returns>
        Stone GetStone(int x, int y);

        /// <summary>
        /// Places a stone at the specified coordinates
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <param name="color">Color of the stone to place</param>
        /// <returns>A new board state with the stone placed</returns>
        void ChangeStoneColor(int x, int y, StoneColor color);

        /// <summary>
        /// Checks if the coordinates are within the board boundaries
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <returns>True if coordinates are valid, false otherwise</returns>
        bool IsValidCoordinate(int x, int y);

        /// <summary>
        /// Creates a deep copy of the current board state
        /// </summary>
        /// <returns>A new instance of IBoard with the same state</returns>
        IBoard Clone();

        /// <summary>
        /// Récupères les pierres voisines de celle spécifiée
        /// </summary>
        /// <param name="stone">La pierre dont on cherche les voisines</param>
        /// <returns>Liste des pierres voisines</returns>
        public List<Stone> GetNeighbors(Stone stone);

        /// <summary>
        /// Vérifie si l'état actuel du plateau correspond à l'état précédent (règle de Ko).
        /// Pour éviter que le jeu tourne en boucle
        /// </summary>
        /// <returns>Vraie si le coup ne respecte pas la règle, faux sinon</returns>
        public bool IsKoViolation();
    }
}
